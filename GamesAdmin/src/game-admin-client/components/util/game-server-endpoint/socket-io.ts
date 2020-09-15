import io from "socket.io-client";

import logger from "../logger";
import Websocket from "./websocket";

export default class SocketIO extends Websocket {
  private socketIO: SocketIOClient.Socket;

  connect() {
    return new Promise((resolve) => {
      console.log(this.isConnected);
      if (!this.isConnected) {
        console.log(this.endpoint);
        this.socketIO = io(this.endpoint, { transports: ["websocket"], upgrade: false });
        this.socketIO.on("connect", () => {
          console.log(this.socketIO);
          this.isConnected = true;
          this.socketIO.on("message", this.onReceiveMessage.bind(this));
          this.socketIO.on("error", (event) => logger.error(event));
          this.socketIO.on("disconnect", this.disconnect.bind(this));
          resolve();
        });
      } else {
        resolve();
      }
    });
  }

  send(message: Object) {
    try {
      if (this.socketIO && this.socketIO.connected) {
        this.socketIO.emit("message", message);
      }
    } catch (e) {
      logger.error(e);
    }
  }

  protected onReceiveMessage(event: MessageEvent) {
    try {
      //   console.log(event);
      //   const data = event.data;
      //   const message = JSON.parse(data);

      this.executeObservers(event);
    } catch (e) {
      logger.error(e);
    }
  }

  disconnect() {
    const message = {
      [this.onDisconnectedMessage]: this.onDisconnectedMessage,
    };

    this.socketIO.close();
    this.executeObservers(message);
  }
}
