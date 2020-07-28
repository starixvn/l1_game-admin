import React from "react";
import {
  getGameInfoByGameId,
  getGameInfoByGameName,
  GameMonitoringInfo,
} from "../components/game-monitoring/game-monitoring";
import Loading from "../components/game-monitoring/game-template/game-template-loading";
import GameMenu from "../components/game-monitoring/menu/menu";
import { NextRouter, withRouter } from "next/router";
import GameWsEndpoint from "../components/util/game-server-endpoint/websocket";
import MessageTransmitter, {
  MessageTransmitterType,
} from "../components/util/message-transmitter";
import Cookies from "js-cookie";
import { Modal, Row } from "reactstrap";
import SocketIO from "../components/util/game-server-endpoint/socket-io";

type Props = {
  router: NextRouter;
};

type State = {
  gameId: string | number;
  showLoading: boolean;
  gameName: string;
  token: string;
  showDisconnectedNotice: boolean;
};

export class Home extends React.Component<Props, State> {
  public state: State;
  private socket: GameWsEndpoint;
  private messageTransmitter: MessageTransmitterType;
  private isProduction: boolean;

  constructor(props: Props, isUseSocketIO = false) {
    super(props);

    const gameServerEndpoint =
      process.env.NEXT_PUBLIC_ENV === "local"
        ? process.env.NEXT_PUBLIC_GAME_SERVER_ENDPOINT
        : Cookies.get("game-server-endpoint");
    const token =
      process.env.NEXT_PUBLIC_ENV === "local"
        ? process.env.NEXT_PUBLIC_TOKEN
        : Cookies.get("token");
    const protocol = (
      globalThis.location || { protocol: "" }
    ).protocol.endsWith("s:")
      ? "wss"
      : "ws";
    const sedieServerEndpoint =
      process.env.NEXT_PUBLIC_ENV === "local"
        ? process.env.NEXT_PUBLIC_SEDIE_SERVER_ENDPOINT
        : Cookies.get("sedie-server-endpoint");

    this.isProduction =
      Cookies.get("env") && Cookies.get("env").toLocaleUpperCase() === "PRO";
    this.socket = isUseSocketIO
      ? new SocketIO(`${protocol}://${sedieServerEndpoint}`)
      : new GameWsEndpoint(`${protocol}://${gameServerEndpoint}/admin`);
    this.socket.onDisconnected(this.showWebSocketDisconnected.bind(this));
    this.socket.addOrReplaceObserver(
      "sessionTimeout",
      this.showWebSocketDisconnected.bind(this),
      (message: { unauth: Object }) => message && message.unauth != null
    );

    this.messageTransmitter = MessageTransmitter();
    this.state = {
      gameId: 0,
      showLoading: true,
      gameName: "",
      token,
      showDisconnectedNotice: false,
    };
  }

  static getDerivedStateFromProps(props: Props, state: State) {
    state.gameId = (props.router.query.gameId || 0).toString();
    state.gameName = (props.router.query.game || "").toString();

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
    const {
      onMessage,
      selector,
      initialMessage,
      endMessage,
      generateMessage,
    } = info;
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
    this.socket.addOrReplaceObserver(
      key,
      onMessage(this.messageTransmitter.broadcast),
      selector
    );

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
          <Row>
            Please close this popup and open new popup to monitor again.
          </Row>
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
    const { gameId, gameName, token } = this.state;
    const info = getGameInfoByGameId(gameId) || getGameInfoByGameName(gameName);

    this.socket.login({ jwt: token });

    return info
      ? this.renderGameMonitoringComponent(info)
      : this.renderLoaderOrGameMenu();
  }
}

export default withRouter(Home);
