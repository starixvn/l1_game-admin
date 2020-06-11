import logger from "../logger";

type ObserverSelector = (message?: object) => boolean;
type ObserverClient = {
  selector: ObserverSelector,
  handler: Function
};

export default class {
  private socket: WebSocket;
  private isConnected: boolean;
  private isLogged: boolean;
  private readonly endpoint: string;
  private readonly observers: {
    [key: string]: ObserverClient
  };

  constructor(endpoint: string) {
    this.isConnected = false;
    this.isLogged = false;
    this.endpoint = endpoint;
    this.observers = {};
  }

  public connect() {
    return new Promise((resolve => {
      if (!this.isConnected) {
        this.socket = new WebSocket(this.endpoint);
        this.socket.onmessage = this.onReceiveMessage.bind(this);
        this.socket.onerror = event => logger.error(event);
        this.socket.onclose = this.disconnect.bind(this);
        this.socket.onopen = () => {
          this.isConnected = true;
          resolve();
        };
      } else {
        resolve();
      }
    }));
  }

  public addObserver(key, handler, selector: ObserverSelector = () => true) {
    if (!this.observers[key]) {
      this.observers[key] = { handler, selector };
    } else {
      logger.warn(`Observer '${key}' have already been registered`)
    }
    logger.info(this.observers);
  }

  public removeObserver(key) {
    if (this.observers[key]) {
      delete this.observers[key];
    }
  }

  public addOrReplaceObserver(key, handler, selector: ObserverSelector = () => true) {
    this.observers[key] = { handler, selector };
  }

  public login(data: { username?: string, password?: string, jwt?: string }) {
    return new Promise(resolve => {
      if (this.isConnected && !this.isLogged) {
        const loginResultKey = 'login-result';
        const loginResultSelector = message => message.Authorized != null;
        const loginHandler = message => {
          this.isLogged = message.Authorized;
          this.removeObserver(loginResultKey);
          resolve();
        }

        this.addOrReplaceObserver(loginResultKey, loginHandler, loginResultSelector);

        this.send({ authentication: { ...data } });
      } else {
        resolve();
      }
    });
  }

  public send(message) {
    try {
      if (this.socket && this.socket.readyState === 1) {
        this.socket.send(JSON.stringify(message));
      }
    } catch (e) {
      logger.error(e);
    }
  }

  private disconnect() {
    this.isConnected = false;
    this.socket.close();
  }

  private onReceiveMessage(event) {
    try {
      const data = event.data;
      const message = JSON.parse(data);

      //console.log(message);
      //console.log(message.mini_dragon_tiger.finish);
      Object.values(this.observers).forEach((client) => {
        if (client.selector && client.selector(message)) {
          client.handler && client.handler(message);
        }
      })
    } catch (e) {
      logger.error(e);
    }
  }
}
