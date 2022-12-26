import { GameSetting } from "../lobby-data-types";

export type LobbyMenuColumn = {
  isBig: boolean;
  ids: number[];
};

export default class LobbyMenu {
  gameSettings: GameSetting[];

  constructor(gameSettings: GameSetting[]) {
    this.gameSettings = gameSettings;
  }

  private splitWhenever<T>(filter: (element: T) => boolean, list: T[]) {
    const result: T[][] = [];
    let current: T[] = [];

    for (let i = 0; i < list.length; i = i + 1) {
      if (!filter(list[i])) {
        current.push(list[i]);
      } else {
        result.push([list[i]]);
      }

      if (
        ((i < list.length - 1 && filter(list[i + 1])) ||
          i === list.length - 1) &&
        current.length > 0
      ) {
        result.push(current);
        current = [];
      }
    }

    return result;
  }

  private getNextArea(
    index: number,
    list: number[][],
    needNext: (games: number[]) => boolean
  ) {
    if (list.length - 1 <= index) {
      return [];
    } else if (needNext(list[index + 1])) {
      return this.getNextArea(index + 1, list, needNext);
    } else {
      return list[index + 1];
    }
  }

  private arrangeGames(games: number[][]) {
    const hasBigSize = (list: number[]) => {
      if (list) {
        return list.length === 1 && this.isBigSize(list[0]);
      } else {
        return false;
      }
    };

    const result: number[] = [];

    for (let index = 0; index < games.length; index++) {
      const gameIds = games[index];
      const passed =
        hasBigSize(gameIds) ||
        gameIds.length % 2 === 0 ||
        index === games.length - 1;

      if (!passed) {
        const nextGames = this.getNextArea(index, games, hasBigSize);

        if (nextGames.length !== 0) {
          const ids = nextGames.splice(0, 1);

          gameIds.push(...ids);
        } else {
          const ids = gameIds.splice(gameIds.length - 1, 1);

          games[games.length - 1].push(...ids);
        }
      }

      result.push(...gameIds);
    }

    return result;
  }

  private isBigSize(id: number) {
    const selectedGameSetting = this.gameSettings.find(
      (gameSetting) => gameSetting.gameId === id
    );
    return selectedGameSetting ? selectedGameSetting.bigSize : false;
  }

  getListToRender() {
    const games = this.gameSettings.map((gameSetting) => gameSetting.gameId);
    const splittedGames = this.splitWhenever<number>(
      this.isBigSize.bind(this),
      games
    );
    const list = this.arrangeGames(splittedGames);

    const result: LobbyMenuColumn[] = [];
    let tmp: number[] = [];
    for (let i = 0; i < list.length; i++) {
      if (this.isBigSize(list[i])) {
        result.push({ isBig: true, ids: [list[i]] });
      } else if (tmp.length === 1) {
        result.push({ isBig: false, ids: [tmp[0], list[i]] });
        tmp = [];
      } else if (i === list.length - 1) {
        result.push({ isBig: false, ids: [list[i]] });
      } else {
        tmp.push(list[i]);
      }
    }

    return result;
  }
}
