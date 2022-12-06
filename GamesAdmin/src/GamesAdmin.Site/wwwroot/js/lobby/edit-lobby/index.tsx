import React, { useState, createContext, useEffect, } from "react";

import { FormControl } from "react-bootstrap";
import LobbyLayout from "./lobby-layout";
import MiniPanel from "./minipanel";
import axios from 'axios';

declare global {
    var jQuery: any;
}

export const LobbyContext = createContext({});
const lobbyId = (document.getElementById("lobbyId") as HTMLInputElement)?.value;

const isCreatePage = lobbyId === "";

const EditLobby: React.FC = () => {
    const [name, setName] = useState("INH Lobby");
    const [isCreatePage, setIsCreatePage] = useState(true);

    useEffect(() => {
        initLobbyConfig();
    }, []);

    const initLobbyConfig = async () => {

        const filterGroups = (partnerId, currencyGroupModels) =>
            currencyGroupModels.filter((group) => group.partnerId === partnerId);

        const convertPartnerToItem = (defaultValue, currencyGroupModels) => (partner) => ({
            id: partner.partnerId,
            value: partner.partnerId,
            text: partner.name,
            checked: defaultValue,
            enable: true,
            cssClass: "partner-item",
            items: filterGroups(partner.partnerId, currencyGroupModels).map(
                (group) => {
                    return {
                        id: group.id,
                        value: group.id,
                        text: group.name,
                        checked: true,
                        enable: true,
                        cssClass: "group-item",
                    }
                }),
        });

        const res = await axios('/lobby/GetLobbyConfig');
        setIsCreatePage(!res.data.lobbyConfig);
        const data = res.data.partners
            .map(convertPartnerToItem(true, res.data.currencyGroupModels))
            .filter((partner) => partner.items.length);

        window.jQuery("#currency-groups").fanexMultiSelect({
            listData: data,
            showFilter: true,
            onLeave: function () {
                var value = window.jQuery("#currency-groups").fanexMultiSelect('getItems', true);
                console.log("select value", value);
            },
        });
    }

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
                <div className="row">
                    <div className="col-1 bold pt-3">Groups</div>
                    <div className="col-2">
                        <select id="currency-groups"></select>
                    </div>
                </div>
                <LobbyLayout />
                <MiniPanel />
            </div>
        </LobbyContext.Provider>
    );
};

export default EditLobby;
