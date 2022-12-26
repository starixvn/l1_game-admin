import "regenerator-runtime/runtime.js";
import ReactDOM from "react-dom";
import React, { createContext } from "react";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import LobbyManagement from "./lobby-management";
import EditLobby from "./edit-lobby/index";

const e = React.createElement;

export const LobbyContext = createContext({});
const Lobby: React.FC = () => {
  return (
    <div className="container-fluid form">
      <Router>
        <Route path="/lobby" exact component={LobbyManagement} />
        <Route path="/lobby/edit/:id" component={EditLobby} />
        <Route path="/lobby/add" component={EditLobby} />
      </Router>
    </div>
  );
};

const domContainer = document.querySelector("#root");
ReactDOM.render(e(Lobby), domContainer);
