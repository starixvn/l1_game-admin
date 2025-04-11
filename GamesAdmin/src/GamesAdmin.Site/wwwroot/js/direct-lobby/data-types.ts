export type Game = {
  gameId: number;
  name: string;
  displayName: string;
  newTag: boolean;
};

export type DirectLobbyConfig = {
  id?: string;
  name: string;
  currencyGroupIds: string[];
  gameIds: number[];
  newGames: number[];
};

export type CurrencyGroup = {
  id: string;
  name: string;
  partnerId: string;
};

export type Operator = {
  id: string;
  name: string;
  partnerId: string;
};

export type CreateOrUpdateViewModel = {
  currencyGroupModels: CurrencyGroup[];
  lobbyConfig: DirectLobbyConfig;
  gameSettings: Game[];
  operators: Operator[];
  isSuperAdmin: boolean;
};

export type SelectItem = {
  value: string;
  text: string;
};

export type DirectLobbyManagementViewModel = {
  lobbyConfigs: DirectLobbyConfig[];
  currencyGroupItems: SelectItem[];
  isSuperAdmin: boolean;
  displayDeleteButton: boolean;
};
