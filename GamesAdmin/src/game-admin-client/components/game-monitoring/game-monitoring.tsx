import _ from "lodash";

import Baccarat from "./baccarat";
import BigSmallTurbo from "./big-small-turbo/big-small-turbo";
import BigSmall from "./big-small/big-small";
import DragonTiger from "./dragon-tiger/dragon-tiger";
import FishPrawnCrab from "./fish-prawn-crab/fish-prawn-crab";
import gameNameMapper from "./game-name-mapper";
import KenoProMax from "./keno-pro-max/keno-pro-max";
import MiniKeno from "./mini-keno";
import MiniSedie from "./mini-sedie";
import OddEvenTurbo from "./odd-even-turbo/odd-even-turbo";
import OddEven from "./odd-even/odd-even";
import Roulette from "./roulette";
import ShakeThePlate from "./shake-the-plate/shake-the-plate";
import SpecialSeDie from "./special-sedie";
import VipBaccarat from "./vip-baccarat";
import VipFishPrawnCrab from "./vip-fish-prawn-crab/vip-fish-prawn-crab";
import VipSeDie from "./vip-sedie/vip-sedie";
import VipSeDieWar from "./vip-se-die-war";
import SicBo from "./sic-bo";
import Lottery from "./lottery";
import ThreeCards from "./three-cards";
import BitcoinMaster from "./bitcoin-master";
import { KenoMatch1, KenoMatch2 } from "./keno-match";
import { KenoPower1, KenoPower2 } from "./keno-power";

const getObjectOfGame =
  (path: string) => (onMessage: Function) => (message: any) => {
    const object = _.get(message, path);
    onMessage && onMessage(object);
  };

const getInfoByGameKey = (key: string) => {
  const info = {
    selector: (message) => message[key] != null,
    onMessage: getObjectOfGame(key),
    initialMessage: {},
    endMessage: {},
    generateMessage: (data: any) => {
      const message = {};
      message[key] = data;

      return message;
    },
  };

  info.initialMessage[key] = {
    get_time: true,
    view: true,
    get_new: true,
    sync: true,
  };
  info.endMessage[key] = { view: false };

  return info;
};

const gameMapper = {
  1: {
    component: BigSmall,
    ...getInfoByGameKey("taixiu"),
  },
  2: {
    component: ShakeThePlate,
    ...getInfoByGameKey("XocXoc"),
  },
  3: {
    component: FishPrawnCrab,
    ...getInfoByGameKey("BauCua"),
  },
  4: {
    component: BigSmallTurbo,
    ...getInfoByGameKey("TXTurbo"),
  },
  5: {
    component: OddEven,
    ...getInfoByGameKey("chanle"),
  },
  6: {
    component: OddEvenTurbo,
    ...getInfoByGameKey("OETurbo"),
  },
  7: {
    component: DragonTiger,
    ...getInfoByGameKey("DragonTiger"),
  },
  8: {
    component: KenoProMax,
    ...getInfoByGameKey("KenoProMax"),
  },
  9: {
    component: MiniSedie,
    ...getInfoByGameKey("MiniSeDie"),
  },
  10: {
    component: Baccarat,
    ...getInfoByGameKey("Baccarat"),
  },
  12: {
    component: MiniKeno("kenoMax", 20),
    ...getInfoByGameKey("KenoMax"),
  },
  13: {
    component: MiniKeno("kenoMax2", 20),
    ...getInfoByGameKey("KenoMax2"),
  },
  14: {
    component: MiniKeno("kenoMini", 10),
    ...getInfoByGameKey("KenoMini"),
  },
  15: {
    component: MiniKeno("kenoMini2", 10),
    ...getInfoByGameKey("KenoMini2"),
  },
  16: {
    component: MiniKeno("kenoEast", 20),
    ...getInfoByGameKey("KenoEast"),
  },
  17: {
    component: MiniKeno("kenoWest", 20),
    ...getInfoByGameKey("KenoWest"),
  },
  18: {
    component: MiniKeno("kenoSouth", 20),
    ...getInfoByGameKey("KenoSouth"),
  },
  19: {
    component: MiniKeno("kenoNorth", 20),
    ...getInfoByGameKey("KenoNorth"),
  },
  21: {
    component: Roulette,
    ...getInfoByGameKey("Roulette"),
  },
  22: {
    component: VipSeDie,
    ...getInfoByGameKey("VipSeDie"),
  },
  24: {
    component: SpecialSeDie,
    ...getInfoByGameKey("SpecialSeDie"),
  },
  25: {
    component: SicBo,
    ...getInfoByGameKey("SicBo"),
  },
  26: {
    component: VipFishPrawnCrab,
    ...getInfoByGameKey("VipFishPrawnCrab"),
  },
  27: {
    component: VipBaccarat,
    ...getInfoByGameKey("VipBaccarat"),
  },
  28: {
    component: VipSeDieWar,
    ...getInfoByGameKey("VipSedieWar"),
  },
  31: {
    component: Lottery,
    ...getInfoByGameKey("Lottery"),
  },
  35: {
    component: ThreeCards,
    ...getInfoByGameKey("ThreeCards"),
  },
  43: {
    component: BitcoinMaster,
    ...getInfoByGameKey("BitcoinMaster"),
  },
  36: {
    component: KenoMatch1,
    ...getInfoByGameKey("KenoMatch1"),
  },
  37: {
    component: KenoMatch2,
    ...getInfoByGameKey("KenoMatch2"),
  },
  40: {
    component: KenoPower1,
    ...getInfoByGameKey("KenoPower1"),
  },
  41: {
    component: KenoPower2,
    ...getInfoByGameKey("KenoPower2"),
  },
};

export function getGameInfoByGameId(gameId: number | string) {
  return gameMapper[gameId];
}

export function getGameInfoByGameName(gameName: string) {
  return gameMapper[gameNameMapper[gameName]];
}

export type GameMonitoringInfo = {
  component: any;
  selector: (message: any) => boolean;
  onMessage: (callback: (message: any) => any) => Function;
  initialMessage: object;
  endMessage: object;
  generateMessage: (data: any) => object;
};
