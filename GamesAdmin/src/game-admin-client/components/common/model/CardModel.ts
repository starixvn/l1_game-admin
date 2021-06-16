export default class CardModel {
  cardName?: string;
  value: Number | String;
  suit: Number;
  onClickHandle?: Function;

  constructor() {
    this.value = -1;
    this.suit = -1;
  }
}
