export default class CardModel {
  cardName?: string;
  value: Number;
  suit: Number;
  onClickHandle?: Function;

  constructor() {
    this.value = -1;
    this.suit = -1;
    this.key = "";
  }
}
