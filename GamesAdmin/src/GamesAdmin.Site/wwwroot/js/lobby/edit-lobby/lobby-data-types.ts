export type Game = {
  value: string;
  text: string;
  isMain: boolean;
  enableRoundTimer: boolean;
};

export type Banner = {
  gameId: number;
  displayTime: number;
};

export type GameSetting = {
  gameId: number;
  bigSize: boolean;
  newTag: boolean;
};

export type ReorderConfig = {
  sourceIndex: number;
  destinationIndex: number;
};

export type CategoryName = {
  eN_US: string;
  vI_VN: string;
  tH_TH: string;
  iD_ID: string;
  zH_CN: string;
  zH_TW: string;
};

export type Category = {
  name: CategoryName;
  gameSettings: GameSetting[];
};

export type LobbyConfig = {
  categories: Category[];
};

export type RoundTimerGame  = {
  gameIds: number[];
  main: number;
};