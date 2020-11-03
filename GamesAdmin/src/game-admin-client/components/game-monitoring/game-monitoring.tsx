import _ from 'lodash';

import Baccarat from './baccarat';
import BigSmallTurbo from './big-small-turbo/big-small-turbo';
import BigSmall from './big-small/big-small';
import DragonTiger from './dragon-tiger/dragon-tiger';
import FishPrawnCrab from './fish-prawn-crab/fish-prawn-crab';
import gameNameMapper from './game-name-mapper';
import KenoProMax from './keno-pro-max/keno-pro-max';
import MiniKeno from './mini-keno';
import MiniSedie from './mini-sedie';
import OddEvenTurbo from './odd-even-turbo/odd-even-turbo';
import OddEven from './odd-even/odd-even';
import Roulette from './roulette';
import ShakeThePlate from './shake-the-plate/shake-the-plate';
import SpecialSeDie from './special-sedie';
import VipFishPrawnCrab from './vip-fish-prawn-crab/vip-fish-prawn-crab';
import VipSeDie from './vip-sedie/vip-sedie';

const getObjectOfGame = (path: string) => (onMessage: Function) => (message: any) => {
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

    info.initialMessage[key] = { get_time: true, view: true, get_new: true, sync: true };
    info.endMessage[key] = { view: false };

    return info;
};

const gameMapper = {
    1: {
        component: BigSmall,
        ...getInfoByGameKey('taixiu'),
    },
    2: {
        component: ShakeThePlate,
        ...getInfoByGameKey('XocXoc'),
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
    10: {
        component: Baccarat,
        ...getInfoByGameKey('baccarat'),
    },
    12: {
        component: MiniKeno('kenoMax', 20),
        ...getInfoByGameKey('kenoMax'),
    },
    13: {
        component: MiniKeno('kenoMax2', 20),
        ...getInfoByGameKey('kenoMax2'),
    },
    14: {
        component: MiniKeno('kenoMini', 10),
        ...getInfoByGameKey('kenoMini'),
    },
    15: {
        component: MiniKeno('kenoMini2', 10),
        ...getInfoByGameKey('kenoMini2'),
    },
    16: {
        component: MiniKeno('kenoEast', 20),
        ...getInfoByGameKey('kenoEast'),
    },
    17: {
        component: MiniKeno('kenoWest', 20),
        ...getInfoByGameKey('kenoWest'),
    },
    18: {
        component: MiniKeno('kenoSouth', 20),
        ...getInfoByGameKey('kenoSouth'),
    },
    19: {
        component: MiniKeno('kenoNorth', 20),
        ...getInfoByGameKey('kenoNorth'),
    },
    21: {
        component: Roulette,
        ...getInfoByGameKey('roulette'),
    },
    22: {
        component: VipSeDie,
        ...getInfoByGameKey('VipSeDie'),
    },
    24: {
        component: SpecialSeDie,
        ...getInfoByGameKey('SpecialSeDie'),
    },
    26: {
        component: VipFishPrawnCrab,
        ...getInfoByGameKey('vipfishprawncrab'),
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
