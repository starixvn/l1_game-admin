import React, { createContext } from "react";
import { Category, LobbyConfig } from "./lobby-data-types";

const LobbyConfigContext = createContext<{
  lobbyConfig: LobbyConfig;
  onChangeCategories: (categories: Category[]) => void;
}>({
  lobbyConfig: { categories: [] },
  onChangeCategories: (categories: Category[]) => {},
});

export const LobbyConfigProvider = LobbyConfigContext.Provider;
export const LobbyConfigConsumer = LobbyConfigContext.Consumer;
export default LobbyConfigContext;
