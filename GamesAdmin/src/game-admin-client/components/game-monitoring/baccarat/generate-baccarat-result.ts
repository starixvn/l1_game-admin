class Card {
    card: number;
    type: number;
    value: number;

    constructor(number) {
        this.card = Math.floor(number / 4);
        this.type = number % 4;
        this.value = (this.card >= 9 ? -1 : this.card) + 1;
    }

    equals(other: Card) {
        return other.card === this.card;
    }
}

class Game {
    banker: Card[];
    player: Card[];
    cards: Card[];

    constructor() {
        this.banker = [];
        this.player = [];
        const cards = [...Array(52).keys()].map(number => new Card(number));
        this.cards = cards;
    }

    pick() {
        const getRandomNumber = (min: number, max: number) => Math.floor(Math.random() * (max - min + 1)) + min;
        const index = getRandomNumber(0, this.cards.length - 1);
        return this.cards.splice(index, 1)[0];
    }

    _hasPair(key) {
        return this[key].length >= 2 && this[key][0].equals(this[key][1]);
    }

    _value(key) {
        return this[key].reduce((total, card) => total + card.value, 0) % 10;
    }

    _reset(key) {
        this.cards.push(...this[key]);
        this[key] = [];
    }

    _init(key) {
        while (this[key].length < 2) {
            const card = this.pick();
            this[key].push(card);
        }
    }

    _pickThirdCard(key) {
        const card = this.pick();
        this[key].push(card);
    }

    bankerHasPair() {
        return this._hasPair('banker');
    }

    playerHasPair() {
        return this._hasPair('player');
    }

    bankerValue() {
        return this._value('banker');
    }

    playerValue() {
        return this._value('player');
    }

    reset() {
        this._reset('banker');
        this._reset('player');
    }

    init() {
        this._init('banker');
        this._init('player');
    }

    naturalVictory() {
        return this.totalCards() === 4 && (this.bankerValue() > 7 || this.playerValue() > 7);
    }

    totalCards() {
        return this.player.length + this.banker.length;
    }

    playerPickThirdCard() {
        if (!this.naturalVictory()) {
            const condition = this.playerValue() <= 5;

            if (condition) {
                this._pickThirdCard('player');
            }
        }
    }

    bankerPickThirdCard() {
        if (!this.naturalVictory()) {
            const bankerValue = this.bankerValue();

            if (bankerValue <= 2) {
                this._pickThirdCard('banker');
            } else if (this.player.length == 3) {
                const thirdCardOfPlayer = this.player[2].value;
                const condition = (
                    (bankerValue === 3 && thirdCardOfPlayer !== 8) ||
                    (bankerValue === 4 && thirdCardOfPlayer >= 2 && thirdCardOfPlayer <= 7) ||
                    (bankerValue === 5 && thirdCardOfPlayer >= 4 && thirdCardOfPlayer <= 7) ||
                    (bankerValue === 6 && thirdCardOfPlayer >= 6 && thirdCardOfPlayer <= 7)
                );

                if (condition) {
                    this._pickThirdCard('banker');
                }
            } else if (this.player.length === 2) {
                if (bankerValue < 6) {
                    this._pickThirdCard('banker');
                }
            }
        }
    }
}

const generateResultWithCondition = (condition) => {
    const game = new Game();

    while (!condition(game)) {
        game.reset();
        [game.init, game.playerPickThirdCard, game.bankerPickThirdCard].forEach((action) => {
            action.call(game);
        });
    }

    return game;
};

export const generateResult = function (options) {
    const condition = function (game) {
        const conditionMapper = {
            player: game.playerValue() > game.bankerValue(),
            banker: game.playerValue() < game.bankerValue(),
            playerPair: game.playerHasPair(),
            bankerPair: game.bankerHasPair(),
            big: game.totalCards() > 4
        };

        return Object.keys(conditionMapper).reduce((result, key) => {
            return result && options[key] == conditionMapper[key];
        }, true);
    }

    return generateResultWithCondition(condition);
}