import DiceType from './DiceType';

export default class DiceModel {
    value: number;
    type: DiceType;

    constructor(value: number = -1, type: DiceType = DiceType.BauCua) {
        this.value = value;
        this.type = type;
    }
}