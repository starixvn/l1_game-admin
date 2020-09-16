import Cookies from "js-cookie";
import { NextPageContext } from "next";
import { NextRouter } from "next/router";
import React from "react";

import GameName from "../components/shared/common/GameName";
import GameWsEndpoint from "../components/util/game-server-endpoint/websocket";
import SocketIO from "../components/util/game-server-endpoint/socket-io";
import MessageTransmitter, {
  MessageTransmitterType,
} from "../components/util/message-transmitter";
import {
  GameMonitoringInfo,
  getGameInfoByGameId,
  getGameInfoByGameName,
} from "../components/game-monitoring/game-monitoring";
import { Modal, Row } from "reactstrap";
import Loading from "../components/game-monitoring/game-template/game-template-loading";
import GameMenu from "../components/game-monitoring/menu/menu";

type Props = {
  router: NextRouter;
  game: string;
  gameId: string;
};

type State = {
  showLoading: boolean;
  token: string;
  showDisconnectedNotice: boolean;
};

class Monitor extends React.Component<Props, State> {
  private socket: GameWsEndpoint;
  private messageTransmitter: MessageTransmitterType;
  private isProduction: boolean;
  protected protocol: string;

  constructor(props: Props) {
    super(props);
    const token =
      process.env.NEXT_PUBLIC_ENV === "local"
        ? process.env.NEXT_PUBLIC_TOKEN
        : Cookies.get("token");

    this.state = {
      showLoading: true,
      token,
      showDisconnectedNotice: false,
    };
  }

  static async getInitialProps(ctx: NextPageContext) {
    return { game: ctx.query.game, gameId: ctx.query.gameId };
  }

  initSocket() {
    let serverEndpoint = "";
    switch (this.props.game) {
      case GameName.VipSeDie:
        serverEndpoint =
          process.env.NEXT_PUBLIC_ENV === "local"
            ? process.env.NEXT_PUBLIC_VIP_SEDIE_SERVER_ENDPOINT
            : Cookies.get("vip-sedie-server-endpoint");
        break;
      case GameName.SeDie:
        serverEndpoint =
          process.env.NEXT_PUBLIC_ENV === "local"
            ? process.env.NEXT_PUBLIC_SEDIE_SERVER_ENDPOINT
            : Cookies.get("sedie-server-endpoint");
        break;
    }

    return new SocketIO(`${this.protocol}://${serverEndpoint}`);
  }

  changeLoadingState(showLoading: boolean) {
    this.setState({ showLoading });
  }

  async componentDidMount() {
    this.protocol = (globalThis.location || { protocol: "" }).protocol.endsWith(
      "s:"
    )
      ? "wss"
      : "ws";
    this.isProduction =
      Cookies.get("env") && Cookies.get("env").toLocaleUpperCase() === "PRO";
    this.socket = this.initSocket();
    this.socket.onDisconnected(this.showWebSocketDisconnected.bind(this));
    this.socket.addOrReplaceObserver(
      "sessionTimeout",
      this.showWebSocketDisconnected.bind(this),
      (message: { unauth: Object }) => message && message.unauth != null
    );

    this.messageTransmitter = MessageTransmitter();

    await this.socket.connect();
    this.changeLoadingState(false);
  }

  renderGameMonitoringComponent(info: GameMonitoringInfo) {
    const { gameId } = this.props;
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
    if (this.socket) {
      const { gameId, game } = this.props;
      const { token } = this.state;
      const info =
        getGameInfoByGameId(gameId ? gameId.toString() : "") ||
        getGameInfoByGameName(game ? game.toString() : "");

      this.socket.login({ jwt: token });

      return info
        ? this.renderGameMonitoringComponent(info)
        : this.renderLoaderOrGameMenu();
    }

    return null;
  }
}

export default Monitor;
