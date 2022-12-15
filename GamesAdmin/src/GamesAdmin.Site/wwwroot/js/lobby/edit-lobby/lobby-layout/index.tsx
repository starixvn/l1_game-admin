import React, { useState, useContext } from "react";
import LobbyContent from "./lobby-content";
import LobbyHeader from "./lobby-header";
import LobbyConfigContext from '../lobby-config-context';


type Props = {};

const LobbyLayout: React.FC<Props> = ({ gameIds }) => {
    const lobbyConfigContext = useContext(LobbyConfigContext);
    const lobbyCategories = lobbyConfigContext.lobbyConfig.categories;
    const [editingIndex, setEditingIndex] = useState(0);

    const onChangeLobbyCategory = (lobbyCategory) => {
        lobbyCategories[editingIndex] = lobbyCategory;
        lobbyConfigContext.onChangeCategories(lobbyCategories);
    }

    const switchCategory = (index) => {
        setEditingIndex(index);
    }

    return (
        <div className="lobby-panel">
            <div className="title">Lobby Layout</div>         
            <LobbyHeader switchCategory={switchCategory} editingIndex={editingIndex} />
            <LobbyContent onChange={onChangeLobbyCategory} lobbyCategory={lobbyCategories[editingIndex]} gameIds={gameIds} />
        </div>
    );
};

export default LobbyLayout;
