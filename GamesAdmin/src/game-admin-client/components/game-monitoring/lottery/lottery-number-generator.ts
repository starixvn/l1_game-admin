import Chance from "chance";
import {  LotteryGameResult } from "../../common/model/LotteryModel";

const definition = {
  prizes: {
    special: { length: 5, max: 99999, total: 1 },
    first: { length: 5, max: 99999, total: 1 },
    second: { length: 5, max: 99999, total: 2 },
    third: { length: 5, max: 99999, total: 6 },
    fourth: { length: 4, max: 9999, total: 4 },
    fifth: { length: 4, max: 9999, total: 6 },
    sixth: { length: 3, max: 999, total: 3 },
    seventh: { length: 2, max: 99, total: 4 },
  },
};

function randomNumber(max) {
  const chance = new Chance();

  return chance.integer({ min: 0, max: max });
}

export default function generateBoard() {
  const { prizes } = definition;
  let generatedBoard = new LotteryGameResult();

  Object.keys(prizes).forEach((name) => {
    const { max, total, length } = prizes[name];

    for (let index = 0; index < total; index++) {
      const number = randomNumber(max);
      generatedBoard[name][index] = number.toString().padStart(length, "0");
    }
  });

  return generatedBoard;
}
