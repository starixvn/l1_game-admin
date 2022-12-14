import React, { createContext } from "react";

const LobbyConfigContext = createContext({
    lobbyConfig: { categories: [] },
    onChangeCategories: (categories) => { }
})

export const LobbyConfigProvider = LobbyConfigContext.Provider;
export const LobbyConfigConsumer = LobbyConfigContext.Consumer;
export default LobbyConfigContext;