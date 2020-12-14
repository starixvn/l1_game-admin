import React from "react";

import BaccaratModel from "../../common/model/BaccaratModel";
import BetItem from "../../common/model/BetItem";
import Baccarat from "../baccarat";

const CHOICES = [
    { name: "player", displayName: "Player" },
    { name: "banker", displayName: "Banker" },
    { name: "tie", displayName: "Tie" },
    { name: "playerPair", displayName: "Player Pair" },
    { name: "bankerPair", displayName: "Banker Pair" },
    { name: "big", displayName: "Big" },
    { name: "small", displayName: "Small" }
];

export default class VipBaccarat extends Baccarat {

    get gameName(){
        return "Vip Baccarat";
    }

    get setResultObjectKey(){
        return "set_result";
    }

    handleMessage(message) {
        super.handleMessage(message);
        
        if (!!message.inGame) {
            this.handleBet(message.inGame);
        }
    }

    handleBet(data) {
        let betList = new BaccaratModel().betList;

        Object.keys(data).forEach(key => {
            const userObj = data[key];
            const betTracks =userObj.betTracks;
            CHOICES.forEach(choice => {
                if (betTracks && betTracks[choice.name] > 0) {
                    betList[choice.name].total += betTracks[choice.name];
                    betList[choice.name].dataList.unshift(new BetItem(userObj.name, betTracks[choice.name]));
                }
            });
        });

        this.setState({ betList: betList });
    }

    mapResult(result){
        return { bankerCards: Object.values(result.banker), playerCards: Object.values(result.player) };
    }
}