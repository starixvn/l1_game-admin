export function pickNumbers(isEven, isBig, element, isMini = false) {
    if (isMini) {
      return generateForMiniKeno(isEven, isBig, element);
    } else {
      return generateForNormalKeno(isEven, isBig, element);
    }
}

const generateForNormalKeno = (isEven, isBig, element) => {
  const evenCondition = total => isEven ? total % 2 == 0 : total % 2 == 1;
  const bigCondition = total => isBig ? total > 810 : total <= 810;
  const elementCondition = {
    6: total => total >= 210 && total <= 695, // Gold
    8: total => total >= 696 && total <= 763, // Wood
    7: total => total >= 764 && total <= 855, // Water
    5: total => total >= 856 && total <= 923, // Fire
    4: total => total >= 924 && total <= 1410 // Earth
  };
  const condition = total =>
    evenCondition(total) &&
    bigCondition(total) &&
    elementCondition[element](total);

  return pick20NumberWithCondition(condition);
}

const generateForMiniKeno = (isEven, isBig, element) => {
  const evenCondition = total => isEven ? total % 2 == 0 : total % 2 == 1;
  const bigCondition = total => isBig ? total > 205 : total <= 205;
  const elementCondition = {
    6: total => total >= 55 && total <= 163, // Gold
    8: total => total >= 164 && total <= 187, // Wood
    7: total => total >= 188 && total <= 222, // Water
    5: total => total >= 223 && total <= 246, // Fire
    4: total => total >= 247 && total <= 355 // Earth
  };
  const condition = total =>
    evenCondition(total) &&
    bigCondition(total) &&
    elementCondition[element](total);

  return pick10NumberWithCondition(condition);
}

const pick20NumberWithCondition = (condition) => {
    return pickNumbersWithCondition(20, condition);
}

const pick10NumberWithCondition = (condition) => {
  return pickNumbersWithCondition(10, condition, 40);
}

const pickNumbersWithCondition = (length, condition, seedLength = 80) => {
    const picker = new Picker(seedLength);
    const onPicked1Number = function (picker) {
        const rollback = (length / 2) >> 0;
        const total = picker.total();

        if (!condition(total) && picker.result.length == length) {
            picker.rollback(rollback);
        }
    }

    while (picker.result.length < length) {
        picker.pick(onPicked1Number)
    }

    return picker.getResult();
}

class Picker {
    result: Array<number>
    seeds: Array<number>;

    constructor(length = 80) {
        this.seeds = [...Array(length + 1).keys()].slice(1);
        this.result = [];
    }

    getRandomNumber(min, max) {
        return Math.floor(Math.random() * (max - min)) + min;
    }

    add(a, b) { return a + b }

    subtract(a, b) { return a - b }

    total() { return this.result.reduce(this.add) }

    rollback(number = 1) {
        const picked = this.result.splice(this.result.length - number);
        this.seeds.push(...picked);
    }

    pick(onPicked) {
        const random = this.getRandomNumber(1, this.seeds.length);
        const number = this.seeds.splice(random, 1);

        this.result.push(number[0]);
        onPicked && onPicked(this);
        return number;
    }

    getResult() {
        return this.result.sort(this.subtract)
    }
}
