import React, { useState, createContext, useEffect, } from "react";
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
    const [categories, setCategories] = useState([{ id: "all games", content: "All Games" }, { id: "se die", content: "Se Die" }]);

    const onChangeCategory = (categories) => {
        console.log("on changed ", categories);
    }

    return (
        <div>

            <LobbyHeader onChange={onChangeCategory} categories={categories} />
        </div>
    );
};

export default LobbyLayout;
