import React from "react";
import { Col, Container, Row } from "reactstrap";

import {
  BaccaratModel,
  BaccaratBetList,
} from "../../common/model/BaccaratModel";
import CardResult from "../../common/model/CardResult";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";
import BetList from "../../shared/bet-list/bet-list";
import { GameState } from "../../shared/common/GameState";
import RoundNum from "../../shared/round-num/round-num";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import GameTemplate from "../game-template/game-template";
import BaccaratCardResult from "./baccarat-card-result";
import BaccaratResultUpdater from "./baccarat-result-updater";
import DeskOfCards from "./desk-of-cards";
import styles from "./styles.module.scss";

const TOTAL_TIME = 30;
const WAITING_TIME = 15;
const DefaultBetList = new BaccaratBetList();
const DefaultResult = {
  bankerCards: [new CardResult(), new CardResult(), new CardResult()],
  playerCards: [new CardResult(), new CardResult(), new CardResult()],
};

const CHOICES = [
  { name: "player", displayName: "Player" },
  { name: "banker", displayName: "Banker" },
  { name: "tie", displayName: "Tie" },
  { name: "playerPair", displayName: "Player Pair" },
  { name: "bankerPair", displayName: "Banker Pair" },
  { name: "big", displayName: "Big" },
  { name: "small", displayName: "Small" },
];

export default class Baccarat extends BaseMonitoring<BaccaratModel> {
  constructor(props) {
    super(props);

    this.state = { ...new BaccaratModel(), betList: this.defaultBetList };
    this.showUpdater = this.showUpdater.bind(this);
    this.updaterToggle = this.updaterToggle.bind(this);
    this.updaterSubmit = this.updaterSubmit.bind(this);
    this.showDeskOfCards = this.showDeskOfCards.bind(this);
    this.updaterDeskOfCardsToggle = this.updaterDeskOfCardsToggle.bind(this);
    this.onPickCard = this.onPickCard.bind(this);
  }

  get gameName() {
    return "Baccarat";
  }

  get setResultObjectKey() {
    return "set_result";
  }

  get defaultBetList(): {} {
    return DefaultBetList;
  }

  get defaultResult(): any {
    return { ...DefaultResult };
  }

  get totalTimeInRound(): number {
    return TOTAL_TIME;
  }

  get waitingTime(): number {
    return WAITING_TIME;
  }

  getResult(result) {
    const bankerResult: CardResult[] = Object.values(result.banker);
    const playerResult: CardResult[] = Object.values(result.player);
    return {
      bankerCards: bankerResult,
      playerCards: playerResult,
    };
  }

  showUpdater() {
    this.setState({
      isShowResultUpdater: true,
    });
  }

  checkWinner(choice) {
    const { settlementResult } = this.state;

    return settlementResult && settlementResult[choice];
  }

  showDeskOfCards(selectedCard: string, cardIndex: number) {
    this.setState({
      isShowDeskOfCards: true,
      selectedCard: selectedCard,
      cardIndex: cardIndex,
    });
  }

  renderMonitor() {
    const { time, roundNumber, betList, result } = this.state;

    return (
      <Container fluid>
        <Row>
          <Col className="col-12 col-lg-4">
            <Row className="justify-content-center">
              <Col align="center" className={styles.timecounter}>
                <TimeCounter
                  remainingTime={time.remainingTime}
                  gameState={this.state.gameState}
                  key={this.state.gameState}
                ></TimeCounter>
                <RoundNum roundNumber={roundNumber} />
              </Col>
            </Row>
            <Row>
              <Col align="center" className="col-12 col-xl-6">
                <div className={styles.title}>Player</div>
                <BaccaratCardResult
                  cardResult={result.playerCards}
                  onCardClick={this.showDeskOfCards}
                  selectedCard="playerCards"
                />
              </Col>
              <Col align="center" className="col-12 col-xl-6">
                <div className={styles.title}>Banker</div>
                <BaccaratCardResult
                  cardResult={result.bankerCards}
                  onCardClick={this.showDeskOfCards}
                  selectedCard="bankerCards"
                />
              </Col>
            </Row>
          </Col>
          <Col align="center" className="col-12 col-lg-1">
            <div className={styles.title}>{this.gameName}</div>
            {!this.props.isProduction && (
              <input
                type="button"
                value="Set Result"
                className={styles["set-button"]}
                onClick={this.showUpdater}
              ></input>
            )}
          </Col>
          <Col className="p-0 col-12 col-lg-7">
            <Row className={styles["row-style"]}>
              <TotalBet
                type="player"
                total={betList.player.total}
                isWin={this.checkWinner("player")}
              />
              <TotalBet
                type="banker"
                total={betList.banker.total}
                isWin={this.checkWinner("banker")}
              />
              <TotalBet
                type="baccaratBig"
                total={betList.big.total}
                isWin={this.checkWinner("big")}
              />
              <TotalBet
                type="baccaratSmall"
                total={betList.small.total}
                isWin={this.checkWinner("small")}
              />
            </Row>
            <Row className={styles["row-style"]}>
              <TotalBet
                type="tie"
                total={betList.tie.total}
                isWin={this.checkWinner("tie")}
              />
              <TotalBet
                type="playerPair"
                total={betList.playerPair.total}
                isWin={this.checkWinner("playerPair")}
              />
              <TotalBet
                type="bankerPair"
                total={betList.bankerPair.total}
                isWin={this.checkWinner("bankerPair")}
              />
            </Row>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const result = [];
    const _this = this;
    CHOICES.forEach(function (choice, i) {
      result.push(
        <Col className={styles["baccarat-betlist"]} key={i}>
          <BetList
            type="onecolumn"
            isWin={_this.checkWinner(choice.name)}
            titles={[<div>{choice.displayName}</div>]}
            dataList={_this.state.betList[choice.name].dataList}
          />
        </Col>
      );
    });

    return <React.Fragment>{result}</React.Fragment>;
  }

  renderTitle(value) {
    return <span className={styles["title"]}>{value}</span>;
  }

  updaterToggle() {
    this.setState({
      isShowResultUpdater: !this.state.isShowResultUpdater,
    });
  }

  updaterDeskOfCardsToggle() {
    this.setState({
      isShowDeskOfCards: !this.state.isShowDeskOfCards,
    });
  }

  updaterSubmit(result) {
    const obj = {
      [this.setResultObjectKey]: {
        result: {
          banker: result.banker,
          player: result.player,
        },
        roundId:
          this.state.gameState == GameState.running
            ? this.state.roundNumber
            : this.state.roundNumber + 1,
      },
    };

    this.setState({
      result: {
        playerCards: result.player,
        bankerCards: result.banker,
      },
      isShowResultUpdater: false,
    });
    this.props.sendMessage(obj);
  }

  onPickCard(card: number, type: number) {
    const { result, selectedCard, cardIndex } = this.state;
    let cardResult = new CardResult();
    cardResult.card = card;
    cardResult.type = type;
    result[selectedCard][cardIndex] = cardResult;
    this.setState({
      result: result,
      isShowDeskOfCards: false,
    });

    const message = {
      [this.setResultObjectKey]: {
        result: {
          banker: result.bankerCards,
          player: result.playerCards,
        },
        roundId:
          this.state.gameState == GameState.running
            ? this.state.roundNumber
            : this.state.roundNumber + 1,
      },
    };

    this.props.sendMessage(message);
  }

  render() {
    return (
      <div>
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          disabled={this.state.disabled}
          enableFilter={true}
          partners={this.props.partners}
        ></GameTemplate>
        $
        {!this.props.isProduction && (
          <BaccaratResultUpdater
            modal={this.state.isShowResultUpdater}
            toggle={this.updaterToggle}
            submit={this.updaterSubmit}
          />
        )}
        $
        {!this.props.isProduction && (
          <DeskOfCards
            isOpen={this.state.isShowDeskOfCards}
            toggle={this.updaterDeskOfCardsToggle}
            onPickCard={this.onPickCard}
          />
        )}
      </div>
    );
  }
}
