import React, { useState, createContext, useEffect, } from "react";
import LobbyContent from "./lobby-content";
import LobbyHeader from "./lobby-header";


type Props = {};

type GameCategory = {
    id: string,
    content: string
}

type GameSetting = {
    id: number;
    bigSize: boolean;
    newTag: boolean;
}

const getGameCategorySetting = (categoryId): GameSetting[] => {
    const count = 4;
    return Array.from({ length: count }, (v, k) => k).map((k, index) => ({
        id: index + 1,
        bigSize: true,
        newTag: false
    }));
}

// fake data generator
const getItems = (count): GameCategory[] =>
    Array.from({ length: count }, (v, k) => k).map(k => ({
        id: `item-${k}`,
        content: `item ${k}`,
    }));


const LobbyLayout: React.FC<Props> = () => {
    const [items, setItems] = useState(getItems(6));
    const [categories, setCategories] = useState([{ id: null, EN_US: "All Games" }, { id: null, EN_US: "Se Die" }]);
    const [gameSettings, setGameSettings] = useState([{ gameId: 1 }, { gameId: 3 }]);
    const [lobbyCategories, setLobbyCategories] = useState([
        {
            name: { id: null, EN_US: "All Games" }, gameSettings: [{ gameId: 1 }, { gameId: 2 }]
        },
        {
            name: { id: null, EN_US: "Se Die" }, gameSettings: [{ gameId: 3 }, { gameId: 4 }]
        }
    ]);
    const [editingIndex, setEditingIndex] = useState(0);
    const [editingLobbyCategory, setEditingLobbyCategory] = useState(lobbyCategories[editingIndex]);

    const onChangeLobbyCategories = (lobbyCategories) => {
        console.log("on changed lobbyCategories", lobbyCategories);
        setLobbyCategories(lobbyCategories);
    }

    const onChangeLobbyCategory = (lobbyCategory) => {
        lobbyCategories[editingIndex] = lobbyCategory;
        console.log("change lobbyCategory ", lobbyCategory);
        setLobbyCategories(lobbyCategories);
    }

    const switchCategory = (index) => {


        setEditingIndex(index);
        setEditingLobbyCategory(lobbyCategories[index]);

        console.log("switch tab", index, editingLobbyCategory);
    }

    return (
        <div>

            <LobbyHeader onChange={onChangeLobbyCategories} lobbyCategories={lobbyCategories} switchCategory={switchCategory} editingIndex={editingIndex} />
            <LobbyContent onChange={onChangeLobbyCategory} lobbyCategory={lobbyCategories[editingIndex]} />
        </div>
    );
};

export default LobbyLayout;
