import BetItem from './BetItem';

export default class BetList {
    dataList: Array<BetItem>
    total: number

    constructor() {
        this.dataList = [];
        this.total = 0;
    }
}
