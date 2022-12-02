import "regenerator-runtime/runtime.js";
import ReactDOM from "react-dom";
import React, { useState, createContext } from "react";
import { FormControl } from "react-bootstrap";
import LobbyLayout from "./lobby-layout";
import MiniPanel from "./minipanel";
const e = React.createElement;

export const LobbyContext = createContext({});
const lobbyId = (document.getElementById("lobbyId") as HTMLInputElement)?.value;

const isCreatePage = lobbyId === "";

const Lobby: React.FC = () => {
    const [name, setName] = useState("INH Lobby");

    return (
        <LobbyContext.Provider value={{}}>
            <div className="container-fluid form">
                <div className="row">
                    <div className="col-1 bold pt-3">Name: </div>
                    <div className="col-2">
                        <FormControl
                            type="text"
                            defaultValue={name}
                            onChange={(e) => isCreatePage && setName(e.target.value)}
                            disabled={!isCreatePage}
                        />
                    </div>
                </div>
                <LobbyLayout />
                <MiniPanel />
            </div>
        </LobbyContext.Provider>
    );
};

const domContainer = document.querySelector("#root");
ReactDOM.render(e(Lobby), domContainer);
