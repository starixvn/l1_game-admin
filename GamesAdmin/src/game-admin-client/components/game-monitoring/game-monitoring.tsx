import _ from 'lodash';
import BigSmall from './big-small/big-small';
import BigSmallTurbo from './big-small-turbo/big-small-turbo';
import DragonTiger from './dragon-tiger/dragon-tiger';
import FishPrawnCrab from './fish-prawn-crab/fish-prawn-crab';
import KenoProMax from './keno-pro-max/keno-pro-max';
import OddEven from './odd-even/odd-even';
import OddEvenTurbo from './odd-even-turbo/odd-even-turbo';
import ShakeThePlate from './shake-the-plate/shake-the-plate';
import MiniSedie from './mini-sedie';
import MiniKeno from './mini-keno';

const getObjectOfGame = (path: string) => (onMessage: Function) => (message: any) => {
  const object = _.get(message, path);
  onMessage && onMessage(object);
}

const getInfoByGameKey = (key: string) => {
  const data = {
    selector: message => message[key] != null,
    onMessage: getObjectOfGame(key),
    initialMessage: {},
    endMessage: {},
    generateMessage: (data: any) => {
      const message = {};
      message[key] = data;

      return message;
    },
  };

  data.initialMessage[key] = { get_time: true, view: true, get_new: true, sync: true };
  data.endMessage[key] = { view: false };

  return data;
};

const gameNameMapper = {
  taixiu: 1,
  xocxoc: 2,
  baucua: 3,
  txturbo: 4,
  oddeven: 5,
  oeturbo: 6,
  dragontiger: 7,
  kenopromax: 8,
  minisedie: 9,
  baccarat: 10,
  kenomini: 14,
  kenomini2: 15,
  kenoeast: 16,
  kenowest: 17,
  kenosouth: 18,
  kenonorth: 19,
};

const gameMapper = {
  1: {
    component: BigSmall,
    ...getInfoByGameKey('taixiu'),
  },
  2: {
    component: ShakeThePlate,
    ...getInfoByGameKey('xocxoc'),
  },
  3: {
    component: FishPrawnCrab,
    ...getInfoByGameKey('baucua'),
  },
  4: {
    component: BigSmallTurbo,
    ...getInfoByGameKey('taixiu_turbo'),
  },
  5: {
    component: OddEven,
    ...getInfoByGameKey('chanle'),
  },
  6: {
    component: OddEvenTurbo,
    ...getInfoByGameKey('clturbo'),
  },
  7: {
    component: DragonTiger,
    ...getInfoByGameKey('mini_dragon_tiger'),
  },
  8: {
    component: KenoProMax,
    ...getInfoByGameKey('kenoProMax'),
  },
  9: {
    component: MiniSedie,
    ...getInfoByGameKey('miniSedie'),
  },
  12: {
    component: MiniKeno('kenoMax'),
    ...getInfoByGameKey('kenoMax')
  },
  13: {
    component: MiniKeno('kenoMax2'),
    ...getInfoByGameKey('kenoMax2')
  },
  14: {
    component: MiniKeno('kenoMini'),
    ...getInfoByGameKey('kenoMini')
  },
  15: {
    component: MiniKeno('kenoMini2'),
    ...getInfoByGameKey('kenoMini2')
  },
  16: {
    component: MiniKeno('kenoEast'),
    ...getInfoByGameKey('kenoEast')
  },
  17: {
    component: MiniKeno('kenoWest'),
    ...getInfoByGameKey('kenoWest')
  },
  18: {
    component: MiniKeno('kenoSouth'),
    ...getInfoByGameKey('kenoSouth')
  },
  19: {
    component: MiniKeno('kenoNorth'),
    ...getInfoByGameKey('kenoNorth')
  },
};

export function getGameInfoByGameId(gameId: number | string) {
  return gameMapper[gameId];
}

export function getGameInfoByGameName(gameName: string) {
  return gameMapper[gameNameMapper[gameName]];
}

export type GameMonitoringInfo = {
  component: any,
  selector: (message: any) => boolean,
  onMessage: (callback: (message: any) => any) => Function,
  initialMessage: object,
  endMessage: object,
  generateMessage: (data: any) => object,
}
