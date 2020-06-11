import DiceType from './DiceType';

export default class DiceModel {
    value: Number;
    type: DiceType;

    constructor(value: Number = -1, type: DiceType = DiceType.BauCua) {
        this.value = value;
        this.type = type;
    }
}