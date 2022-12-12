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


const LobbyLayout: React.FC<Props> = ({ gameIds, lobbyCategories, onChange }) => {
    const [editingIndex, setEditingIndex] = useState(0);
    console.log("lobbyCategories xx", lobbyCategories, editingIndex);
    const [editingLobbyCategory, setEditingLobbyCategory] = useState(lobbyCategories[editingIndex]);

    useEffect(() => {
        console.log("update editingLobbyCategory ", lobbyCategories);
        setEditingLobbyCategory(lobbyCategories[editingIndex]);
    }, [lobbyCategories])

    const onChangeLobbyCategories = (lobbyCategories) => {
        console.log("on changed lobbyCategories", lobbyCategories);
        onChange(lobbyCategories);
    }

    const onChangeLobbyCategory = (lobbyCategory) => {
        lobbyCategories[editingIndex] = lobbyCategory;
        console.log("change lobbyCategory ", lobbyCategory);
        onChange(lobbyCategories);
        setEditingLobbyCategory(lobbyCategory);
    }

    const switchCategory = (index) => {


        setEditingIndex(index);
        setEditingLobbyCategory(lobbyCategories[index]);

        console.log("switch tab", index, editingLobbyCategory);
    }

    return (
        <div>
            <div className="bold">Lobby Layout</div>
            <br />
            <LobbyHeader onChange={onChangeLobbyCategories} lobbyCategories={lobbyCategories} switchCategory={switchCategory} editingIndex={editingIndex} />
            <LobbyContent onChange={onChangeLobbyCategory} lobbyCategory={editingLobbyCategory} gameIds={gameIds} />
        </div>
    );
};

export default LobbyLayout;
