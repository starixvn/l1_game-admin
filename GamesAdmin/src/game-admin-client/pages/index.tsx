import Cookies from "js-cookie";
import { NextRouter, withRouter } from "next/router";
import React from "react";
import { Modal, Row } from "reactstrap";

import {
    GameMonitoringInfo,
    getGameInfoByGameId,
    getGameInfoByGameName,
} from "../components/game-monitoring/game-monitoring";
import gameNameMapper from "../components/game-monitoring/game-name-mapper";
import Loading from "../components/game-monitoring/game-template/game-template-loading";
import GameMenu from "../components/game-monitoring/menu/menu";
import SocketIO from "../components/util/game-server-endpoint/socket-io";
import GameWsEndpoint from "../components/util/game-server-endpoint/websocket";
import MessageTransmitter, { MessageTransmitterType } from "../components/util/message-transmitter";

type Props = {
    router: NextRouter;
};

type State = {
    gameId: string | number;
    showLoading: boolean;
    gameName: string;
    token: string;
    showDisconnectedNotice: boolean;
    socket: GameWsEndpoint;
};

const NewSocketGameIds = [2, 22, 24, 25, 26, 27, 28];

export class Home extends React.Component<Props, State> {
    public state: State;
    private messageTransmitter: MessageTransmitterType;
    private isProduction: boolean;
    protected readonly protocol: string;

    constructor(props: Props) {
        super(props);

        const token = process.env.NEXT_PUBLIC_ENV === 'local' ? process.env.NEXT_PUBLIC_TOKEN : Cookies.get('token');
        this.protocol = (globalThis.location || { protocol: '' }).protocol.endsWith('s:') ? 'wss' : 'ws';
        this.isProduction = Cookies.get('env') && Cookies.get('env').toLocaleUpperCase() === 'PRO';
        this.messageTransmitter = MessageTransmitter();

        this.state = {
            gameId: 0,
            showLoading: true,
            gameName: '',
            token,
            showDisconnectedNotice: false,
            socket: null,
        };
    }

    initSocket(gameId: number) {
        const socketInfo = this.getSocketInfo(gameId);

        const socket = NewSocketGameIds.includes(gameId)
            ? new SocketIO(`${this.protocol}://${socketInfo.endpoint}`, socketInfo.route)
            : new GameWsEndpoint(`${this.protocol}://${socketInfo.endpoint}/admin`);

        socket.onDisconnected(this.showWebSocketDisconnected.bind(this));
        socket.addOrReplaceObserver(
            'sessionTimeout',
            this.showWebSocketDisconnected.bind(this),
            (message: { unauth: Object }) => message && message.unauth != null
        );

        return socket;
    }

    getSocketInfo(gameId) {
        switch (gameId) {
            case gameNameMapper.xocxoc:
                return {
                    endpoint:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_SEDIE_SERVER_ENDPOINT
                            : Cookies.get('sedie-server-endpoint'),
                    route:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_SEDIE_SERVER_PROXY_ROUTE
                            : Cookies.get('sedie-server-endpoint-route'),
                };
            case gameNameMapper.vipsedie:
                return {
                    endpoint:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_VIP_SEDIE_SERVER_ENDPOINT
                            : Cookies.get('vip-sedie-server-endpoint'),
                    route:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_VIP_SEDIE_SERVER_PROXY_ROUTE
                            : Cookies.get('vip-sedie-server-endpoint-route'),
                };
            case gameNameMapper.specialsedie:
                return {
                    endpoint:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_SPECIAL_SEDIE_SERVER_ENDPOINT
                            : Cookies.get('special-sedie-server-endpoint'),
                    route:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_SPECIAL_SEDIE_SERVER_PROXY_ROUTE
                            : Cookies.get('special-sedie-server-endpoint-route'),
                };
            case gameNameMapper.vipfishprawncrab:
                return {
                    endpoint:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_VIP_FISH_PRAWN_CRAB_SERVER_ENDPOINT
                            : Cookies.get('vip-fish-prawn-crab-server-endpoint'),
                    route:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_VIP_FISH_PRAWN_CRAB_SERVER_PROXY_ROUTE
                            : Cookies.get('vip-fish-prawn-crab-server-endpoint-route'),
                };
            case gameNameMapper.vipbaccarat:
            return {
                endpoint:
                    process.env.NEXT_PUBLIC_ENV === 'local'
                        ? process.env.NEXT_PUBLIC_VIP_BACCARAT_SERVER_ENDPOINT
                        : Cookies.get('vip-baccarat-server-endpoint'),
                route:
                    process.env.NEXT_PUBLIC_ENV === 'local'
                        ? process.env.NEXT_PUBLIC_VIP_BACCARAT_SERVER_PROXY_ROUTE
                        : Cookies.get('vip-baccarat-server-endpoint-route'),
                };
            case gameNameMapper.sicbo:
                return {
                    endpoint:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_SIC_BO_SERVER_ENDPOINT
                            : Cookies.get('sic-bo-server-endpoint'),
                    route:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_SIC_BO_SERVER_PROXY_ROUTE
                            : Cookies.get('sic-bo-server-endpoint-route'),
                };        
            default:
                return {
                    endpoint:
                        process.env.NEXT_PUBLIC_ENV === 'local'
                            ? process.env.NEXT_PUBLIC_GAME_SERVER_ENDPOINT
                            : Cookies.get('game-server-endpoint'),
                    route: '',
                };
        }
    }

    async componentDidUpdate(prevProps) {
        const { query } = this.props.router;

        if (query !== prevProps.router.query) {
            const gameId = (query.gameId || 0).toString();
            const gameName = (query.game || '').toString();
            const socket = this.initSocket(parseInt(gameId) > 0 ? parseInt(gameId) : parseInt(gameNameMapper[gameName]));

            await socket.connect();

            this.changeLoadingState(false);
            this.setState({ gameId, gameName, socket });
        }
    }

    changeLoadingState(showLoading: boolean) {
        this.setState({ showLoading });
    }

    renderGameMonitoringComponent(info: GameMonitoringInfo) {
        const { gameId, socket } = this.state;
        const { onMessage, selector, initialMessage, endMessage, generateMessage } = info;
        const key = `game-monitoring:${gameId}`;
        const onStopReceiveMessage = () => {
            socket.send(endMessage);
            this.messageTransmitter.clearAllListener();
        };
        const sendMessage = (data: any) => {
            const message = generateMessage(data);

            socket.send(message);
        };

        socket.send(initialMessage);
        socket.addOrReplaceObserver(key, onMessage(this.messageTransmitter.broadcast), selector);

        return (
            <>
                <info.component
                    sendMessage={sendMessage}
                    onMessage={this.messageTransmitter.addListener}
                    onStopReceiveMessage={onStopReceiveMessage}
                    isProduction={this.isProduction}
                />
                <Modal isOpen={this.state.showDisconnectedNotice} className="notice">
                    <Row>Network is disconnected or session timeout.</Row>
                    <Row>Please close this popup and open new popup to monitor again.</Row>
                </Modal>
            </>
        );
    }

    showWebSocketDisconnected() {
        this.setState({ showDisconnectedNotice: true });
    }

    renderLoaderOrGameMenu() {
        const { showLoading } = this.state;
        return showLoading ? <Loading /> : <GameMenu />;
    }

    render() {
        const { gameId, gameName, token, socket } = this.state;
        const info = getGameInfoByGameId(gameId) || getGameInfoByGameName(gameName);

        socket && socket.login({ jwt: token });

        return info ? this.renderGameMonitoringComponent(info) : this.renderLoaderOrGameMenu();
    }
}

export default withRouter(Home);
