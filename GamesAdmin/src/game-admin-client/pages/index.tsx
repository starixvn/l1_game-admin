import React from 'react';
import { getGameInfoByGameId, getGameInfoByGameName, GameMonitoringInfo } from '../components/game-monitoring/game-monitoring';
import Loading from "../components/game-monitoring/game-template/game-template-loading";
import GameMenu from '../components/game-monitoring/menu/menu';
import { NextRouter, withRouter } from "next/router";
import GameServerWS from "../components/util/game-server-endpoint/websocket";
import GameWsEndpoint from "../components/util/game-server-endpoint/websocket";
import MessageTransmitter, { MessageTransmitterType } from "../components/util/message-transmitter";
import Cookies from 'js-cookie';

type Props = {
  router: NextRouter,
};

type State = {
  gameId: string | number,
  showLoading: boolean,
  gameName: string,
  token: string,
};

class Home extends React.Component<Props, State>{
  public state: State;
  private socket: GameServerWS;
  private messageTransmitter: MessageTransmitterType;

  constructor(props: Props) {
    super(props);

    const gameServerEndpoint = process.env.NEXT_PUBLIC_ENV === 'local' ? process.env.NEXT_PUBLIC_GAME_SERVER_ENDPOINT : Cookies.get('game-server-endpoint');
    const token = process.env.NEXT_PUBLIC_ENV === 'local' ? process.env.NEXT_PUBLIC_TOKEN : Cookies.get('token');

    this.socket = new GameWsEndpoint(`ws://${gameServerEndpoint}/admin`);
    this.messageTransmitter = MessageTransmitter();
    this.state = {
      gameId: 0,
      showLoading: true,
      gameName: '',
      token,
    };
  }

  static getDerivedStateFromProps(props: Props, state: State) {
    state.gameId = (props.router.query.gameId || 0).toString();
    state.gameName = (props.router.query.game || '').toString();

    return state;
  }

  changeLoadingState(showLoading: boolean) {
    this.setState({ showLoading });
  }

  async componentDidMount() {
    await this.socket.connect();
    this.changeLoadingState(false);
  }

  renderGameMonitoringComponent(info: GameMonitoringInfo) {
    const { gameId } = this.state;
    const { onMessage, selector, initialMessage, endMessage, generateMessage } = info;
    const key = `game-monitoring:${gameId}`;
    const onStopReceiveMessage = () => {
      this.socket.send(endMessage);
      this.messageTransmitter.clearAllListener();
    };
    const sendMessage = (data: any) => {
      const message = generateMessage(data);

      this.socket.send(message);
    };

    this.socket.send(initialMessage);
    this.socket.addOrReplaceObserver(key, onMessage(this.messageTransmitter.broadcast), selector);

    return <info.component
      sendMessage={sendMessage}
      onMessage={this.messageTransmitter.addListener}
      onStopReceiveMessage={onStopReceiveMessage}
    />;
  }

  renderLoaderOrGameMenu() {
    const { showLoading } = this.state;
    return showLoading ? <Loading /> : <GameMenu />;
  }

  render() {
    const { gameId, gameName, token } = this.state;
    const info = getGameInfoByGameId(gameId) || getGameInfoByGameName(gameName);

    this.socket.login({ jwt: token });

    return info ? this.renderGameMonitoringComponent(info) : this.renderLoaderOrGameMenu();
  }
}

export default withRouter(Home);
