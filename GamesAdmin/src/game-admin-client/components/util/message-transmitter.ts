export default function messageTransmitter() {
  let listeners: [Function?] = [];

  return {
    addListener: (listener: Function) => listeners.push(listener),
    clearAllListener: () => listeners = [],
    broadcast: (message: any) => {
      listeners.forEach(listener => listener(message));
    }
  }
}

export interface MessageTransmitterType {
  addListener(listener: Function);
  clearAllListener();
  broadcast(message: any);
}
