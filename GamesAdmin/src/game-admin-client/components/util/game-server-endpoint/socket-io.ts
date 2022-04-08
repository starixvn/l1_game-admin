import { io, Socket } from "socket.io-client";
import logger from "../logger";
import Websocket from "./websocket";

export default class SocketIO extends Websocket {
  private socketIO: Socket;

  connect() {
    return new Promise((resolve) => {
      if (!this.isConnected) {
        this.socketIO = io(this.endpoint, {
          transports: ["websocket"],
          upgrade: false,
          path: this.endpointRoute,
        });
        this.socketIO.on("connect", () => {
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
