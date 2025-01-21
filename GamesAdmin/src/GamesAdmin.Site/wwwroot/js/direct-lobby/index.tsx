import "regenerator-runtime/runtime.js";
import ReactDOM from "react-dom";
import React, { createContext } from "react";
import { BrowserRouter, Route } from "react-router-dom";
import DirectLobbyManagement from "./direct-lobby-management";
import EditDirectLobby from "./edit-direct-lobby/index";

const e = React.createElement;

export const LobbyContext = createContext({});
const Lobby: React.FC = () => {
  return (
    <div className="container-fluid form">
      <BrowserRouter>
        <Route path="/direct-lobby" exact component={DirectLobbyManagement} />
        <Route path="/direct-lobby/edit/:id" component={EditDirectLobby} />
        <Route path="/direct-lobby/add" component={EditDirectLobby} />
      </BrowserRouter>
    </div>
  );
};

const domContainer = document.querySelector("#root");
ReactDOM.render(e(Lobby), domContainer);
