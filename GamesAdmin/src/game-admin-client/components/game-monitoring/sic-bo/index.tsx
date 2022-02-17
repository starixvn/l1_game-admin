import React from "react";
import styles from "./styles.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import DiceResult from "../../shared/dice-result/dice-result";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Row, Modal } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import Dice from "../../shared/dice/dice";
import RoundNum from "../../shared/round-num/round-num";
import DiceType from "../../common/model/DiceType";
import DiceModel from "../../common/model/DiceModel";
import { GameState } from "../../shared/common/GameState";
import {
  SicBoBetList,
  SicBoModel,
  SicBoSettlementResult,
} from "../../common/model/SicBoModel";
import { Notice } from "../../shared/notice/notice";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";
import BetItem from "../../common/model/BetItem";

export default class SicBo extends BaseMonitoring<SicBoModel> {
  constructor(props) {
    super(props);

    this.state = new SicBoModel();
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  get totalTimeInRound(): number {
    return 45;
  }

  get waitingTime(): number {
    return 9;
  }

  get defaultResult(): any {
    return [
      new DiceModel(-1, DiceType.TaiXiu),
      new DiceModel(-1, DiceType.TaiXiu),
      new DiceModel(-1, DiceType.TaiXiu),
    ];
  }

  get defaultBetList(): {} {
    return new SicBoBetList();
  }

  get defaultSettlementResult(): any {
    return new SicBoSettlementResult();
  }

  getUserBetList(data, currentData: SicBoBetList) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      const { betTracks, system, currency, partnerId } = data[user];
      const userRate = this.currencyExchangeRate[currency];
      Object.keys(betTracks).forEach((key) => {
        if (betTracks[key]) {
          const baseBetAmount = parseFloat(
            (betTracks[key] * userRate).toFixed(2)
          );

          if (key.includes("specific")) {
            const choice = key.split("specific")[1];
            currentData.specificNumber.dataList.unshift(
              new BetItem(
                data[user].name,
                baseBetAmount,
                "",
                choice,
                system,
                currency,
                partnerId
              )
            );
            currentData.specificNumber.total += betTracks[key];
            return;
          }
          if (key.includes("total")) {
            const choice = key.split("total")[1];
            currentData.totalPoints.dataList.unshift(
              new BetItem(
                data[user].name,
                baseBetAmount,
                "",
                choice,
                system,
                currency,
                partnerId
              )
            );
            currentData.totalPoints.total += betTracks[key];
            return;
          }

          currentData[key].total += betTracks[key];
          currentData[key].dataList.unshift(
            new BetItem(
              data[user].name,
              baseBetAmount,
              "",
              "",
              system,
              currency,
              partnerId
            )
          );
        }
      });
    }

    return { ...currentData };
  }

  getResult(diceResult) {
    const result = new Array<DiceModel>();

    result.push(new DiceModel(diceResult.dice1, DiceType.TaiXiu));
    result.push(new DiceModel(diceResult.dice2, DiceType.TaiXiu));
    result.push(new DiceModel(diceResult.dice3, DiceType.TaiXiu));

    return result;
  }

  handleChangeResult(id, value) {
    const { currentDiceNumber } = this.state;
    const { result } = this.state;
    result[currentDiceNumber] = new DiceModel(value, DiceType.TaiXiu);
    const newResult = {};
    result.forEach(
      (dice, index) =>
        (newResult[`dice${index + 1}`] = dice.value === -1 ? 1 : dice.value)
    );

    this.props.sendMessage({
      set_result: {
        roundId: this.state.roundNumber,
        result: newResult,
      },
    });

    this.setState({
      result,
      isOpenModal: false,
    });

    if (this.state.gameState == GameState.waiting) {
      const gameResult = this.state.nextRoudResult;
      gameResult[currentDiceNumber] = new DiceModel(value, DiceType.TaiXiu);

      this.setState({
        isHandleNextRoundResult: true,
        nextRoudResult: gameResult,
      });
    }
  }

  openModal(id) {
    this.setState({
      isOpenModal: true,
      currentDiceNumber: parseInt(id.split("-")[1]),
    });
  }

  toggleModal() {
    this.setState({
      isOpenModal: !this.state.isOpenModal,
    });
  }

  isSpecificNumberWon() {
    const { settlementResult } = this.state;
    return Object.keys(settlementResult)
      .filter((key) => key.includes("specific"))
      .some((key) => settlementResult[key]);
  }

  isTotalPointsWon() {
    const { settlementResult } = this.state;
    return Object.keys(settlementResult)
      .filter((key) => key.includes("total"))
      .some((key) => settlementResult[key]);
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState, settlementResult } =
      this.state;

    return (
      <Container>
        <Row className={styles["align-middle"]}>
          <Col align="center" className="col-12 col-md-4">
            <Row className={styles["align-middle"]}>
              <Col className={styles.timecounter}>
                <div className={styles.title}> Sic Bo</div>
                <TimeCounter
                  remainingTime={time.remainingTime}
                  gameState={gameState}
                ></TimeCounter>
                <RoundNum roundNumber={roundNumber} />
              </Col>
              <Col className={styles.result}>
                <DiceResult dices={result} diceClickHandle={this.openModal} />
              </Col>
            </Row>
          </Col>
          <Col align="center" className="col-4">
            <Row className="justify-content-center">
              <TotalBet
                total={betList.specificNumber.total}
                type="specificNumber"
                isWin={this.isSpecificNumberWon()}
              ></TotalBet>
              <TotalBet
                total={betList.big.total}
                type="sicboBig"
                isWin={settlementResult.big}
              ></TotalBet>
              <TotalBet
                total={betList.small.total}
                type="sicboSmall"
                isWin={settlementResult.small}
              ></TotalBet>
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.anyTriple.total}
                type="anyTriple"
                isWin={settlementResult.anyTriple}
              ></TotalBet>
              <TotalBet
                total={betList.totalPoints.total}
                type="totalPoints"
                isWin={this.isTotalPointsWon()}
              ></TotalBet>
            </Row>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList, settlementResult } = this.state;

    return (
      <React.Fragment>
        <Col className="col-4 col-lg">
          <BetList
            type="withchoice"
            dataList={betList.specificNumber.dataList}
            titles={[<div className="text-center">Specific Number</div>]}
            isWin={this.isSpecificNumberWon()}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.big.dataList}
            titles={[<div className="text-center">Big</div>]}
            isWin={settlementResult.big}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.small.dataList}
            titles={[<div className="text-center">Small</div>]}
            isWin={settlementResult.small}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.anyTriple.dataList}
            titles={[<div className="text-center">Any Triple</div>]}
            isWin={settlementResult.anyTriple}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="withchoice"
            dataList={betList.totalPoints.dataList}
            titles={[<div className="text-center">Total Points</div>]}
            isWin={this.isTotalPointsWon()}
          />
        </Col>
      </React.Fragment>
    );
  }

  renderGameControl() {
    const { isOpenModal } = this.state;

    return (
      <Modal
        isOpen={isOpenModal}
        toggle={this.toggleModal}
        className={styles["game-control-panel"]}
      >
        <Row>
          <Dice
            diceModel={new DiceModel(1, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(2, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(3, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(4, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(5, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(6, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
        </Row>
      </Modal>
    );
  }

  render() {
    return (
      <div className={styles["-sic-bo"]}>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          enableFilter={true}
          disabled={this.state.gameState === GameState.disabled}
          partners={this.props.partners}
        />
        {this.state.gameState === GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
