import logger from "../logger";

type ObserverSelector = (message?: object) => boolean;
type ObserverClient = {
  selector: ObserverSelector;
  handler: Function;
};

export default class {
  private socket: WebSocket;
  protected isConnected: boolean;
  private isLogged: boolean;
  private disconnectListenterCounter: number;
  protected readonly endpoint: string;
  protected readonly endpointRoute: string;
  private readonly observers: {
    [key: string]: ObserverClient;
  };
  protected readonly onDisconnectedMessage: string;

  constructor(endpoint: string, route: string = "") {
    this.isConnected = false;
    this.isLogged = false;
    this.endpoint = endpoint;
    this.endpointRoute = route;
    this.observers = {};
    this.disconnectListenterCounter = 0;
    this.onDisconnectedMessage = "onDisconnected";
  }

  public connect() {
    return new Promise((resolve) => {
      if (!this.isConnected) {
        this.socket = new WebSocket(this.endpoint);
        this.socket.onmessage = this.onReceiveMessage.bind(this);
        this.socket.onerror = (event) => {
          logger.error(event);
          console.log(event);
        };
        this.socket.onclose = this.disconnect.bind(this);
        this.socket.onopen = () => {
          this.isConnected = true;
          resolve();
        };
      } else {
        resolve();
      }
    });
  }

  public addObserver(
    key: string | number,
    handler: Function,
    selector: ObserverSelector = () => true
  ) {
    if (!this.observers[key]) {
      this.observers[key] = { handler, selector };
    } else {
      logger.warn(`Observer '${key}' have already been registered`);
    }
    logger.info(this.observers);
  }

  public removeObserver(key) {
    if (this.observers[key]) {
      delete this.observers[key];
    }
  }

  public addOrReplaceObserver(
    key: string | number,
    handler: Function,
    selector: ObserverSelector = () => true
  ) {
    this.observers[key] = { handler, selector };
  }

  public login(data: { username?: string; password?: string; jwt?: string }) {
    return new Promise((resolve) => {
      if (this.isConnected && !this.isLogged) {
        const loginResultKey = "login-result";
        const loginResultSelector = (message) => message.Authorized != null;
        const loginHandler = (message) => {
          this.isLogged = message.Authorized;
          this.removeObserver(loginResultKey);
          resolve();
        };

        this.addOrReplaceObserver(
          loginResultKey,
          loginHandler,
          loginResultSelector
        );

        this.send({ authentication: { ...data } });
      } else {
        resolve();
      }
    });
  }

  public send(message: Object) {
    try {
      if (this.socket && this.socket.readyState === 1) {
        this.socket.send(JSON.stringify(message));
      }
    } catch (e) {
      logger.error(e);
    }
  }

  public onDisconnected(handler: () => void) {
    const key = `${this.onDisconnectedMessage}:${this
      .disconnectListenterCounter++}`;
    this.addObserver(
      key,
      handler,
      (message: Object) => message[this.onDisconnectedMessage]
    );
  }

  protected disconnect() {
    const message = {
      [this.onDisconnectedMessage]: this.onDisconnectedMessage,
    };

    this.isConnected = false;
    this.socket.close();
    this.executeObservers(message);
  }

  protected onReceiveMessage(event: MessageEvent) {
    try {
      const data = event.data;
      const message = JSON.parse(data);

      this.executeObservers(message);
    } catch (e) {
      logger.error(e);
    }
  }

  protected executeObservers(message: Object) {
    Object.values(this.observers).forEach((client) => {
      if (client.selector && client.selector(message)) {
        client.handler && client.handler(message);
      }
    });
  }
}
