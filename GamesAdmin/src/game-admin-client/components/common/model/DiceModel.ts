import DiceType from './DiceType';

export default class DiceModel {
    value: number | boolean;
    type: DiceType;

    constructor(value: number | boolean = -1, type: DiceType = DiceType.BauCua) {
        this.value = value;
        this.type = type;
    }
}