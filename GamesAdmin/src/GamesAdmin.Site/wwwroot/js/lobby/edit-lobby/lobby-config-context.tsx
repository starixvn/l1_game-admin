import React, { createContext } from "react";
import { Category, LobbyConfig } from "./lobby-data-types";

const LobbyConfigContext = createContext<{
  lobbyConfig: LobbyConfig;
  onChangeCategories: (categories: Category[]) => void;
  isSuperAdmin: boolean;
}>({
  lobbyConfig: { categories: [] },
  onChangeCategories: (categories: Category[]) => {},
  isSuperAdmin: false,
});

export const LobbyConfigProvider = LobbyConfigContext.Provider;
export const LobbyConfigConsumer = LobbyConfigContext.Consumer;
export default LobbyConfigContext;
