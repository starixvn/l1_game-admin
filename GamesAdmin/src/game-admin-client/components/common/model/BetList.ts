import BetItem from './BetItem';

export default class BetList {
    dataList: Array<BetItem>
    total: number

    constructor() {
        this.dataList = new Array();
        this.total = 0;
    }
}