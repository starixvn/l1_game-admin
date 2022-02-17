import React from "react";
import styles from "./fish-prawn-crab.module.scss";
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
  FishPrawnCrabModel,
  FishPrawnCrabBetList,
} from "../../common/model/FishPrawnCrabModel";
import { Notice } from "../../shared/notice/notice";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";

const CHOICES = ["stag", "gourd", "rooster", "fish", "crab", "prawn"];

enum FishPrawnCrabDice {
  stag = 0,
  gourd,
  rooster,
  fish,
  crab,
  prawn,
}

export default class FishPrawnCrab extends BaseMonitoring<FishPrawnCrabModel> {
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = { ...new FishPrawnCrabModel(), betList: this.defaultBetList };
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  get defaultBetList(): {} {
    return new FishPrawnCrabBetList();
  }

  get defaultResult(): any {
    return [
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
    ];
  }

  get defaultSettlementResult(): any {
    return [false, false, false, false, false, false];
  }

  get totalTimeInRound(): number {
    return 30;
  }

  get waitingTime(): number {
    return 10;
  }

  getResult(result) {
    const dices = new Array<DiceModel>();

    dices.push(new DiceModel(result.dice1, DiceType.BauCua));
    dices.push(new DiceModel(result.dice2, DiceType.BauCua));
    dices.push(new DiceModel(result.dice3, DiceType.BauCua));

    return dices;
  }

  getSettlementResult(settlementResult) {
    const winners = [];

    CHOICES.forEach((choice) => {
      winners.push(settlementResult[choice]);
    });

    return winners;
  }

  handleChangeResult(id, value) {
    const { currentDiceNumber } = this.state;
    const gameResult = this.state.nextRoundResult;
    const newResult = this.state.result;

    gameResult[`dice${+currentDiceNumber + 1}`] = value;

    newResult[currentDiceNumber] = new DiceModel(value, DiceType.BauCua);
    this.setState({
      result: newResult,
      isOpenModal: false,
    });

    if (this.state.gameState == GameState.waiting) {
      this.setState({
        isHandleNextRoundResult: true,
        nextRoundResult: gameResult,
      });
    }

    this.props.sendMessage({
      set_result: {
        result: gameResult,
        roundId:
          this.state.gameState == GameState.running
            ? this.state.roundNumber
            : this.state.roundNumber + 1,
      },
    });
  }

  openModal(id) {
    this.setState({
      isOpenModal: true,
      currentDiceNumber: id.split("-")[1],
    });
  }

  toggleModal() {
    this.setState({
      isOpenModal: !this.state.isOpenModal,
    });
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, settlementResult, gameState } =
      this.state;

    return (
      <Container>
        <Row className={styles["align-middle"]}>
          <Col align="center" className="col-12 col-sm-6">
            <Row className={styles["align-middle"]}>
              <Col className={styles.timecounter}>
                <div className={styles.title}>Fish Prawn Crab</div>
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
          <Col align="center" className={styles.timecounter}>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.stag.total}
                type="nai"
                isWin={settlementResult[FishPrawnCrabDice.stag]}
              ></TotalBet>
              <TotalBet
                total={betList.gourd.total}
                type="bau"
                isWin={settlementResult[FishPrawnCrabDice.gourd]}
              ></TotalBet>
              <TotalBet
                total={betList.rooster.total}
                type="ga"
                isWin={settlementResult[FishPrawnCrabDice.rooster]}
              ></TotalBet>
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.fish.total}
                type="ca"
                isWin={settlementResult[FishPrawnCrabDice.fish]}
              ></TotalBet>
              <TotalBet
                total={betList.crab.total}
                type="cua"
                isWin={settlementResult[FishPrawnCrabDice.crab]}
              ></TotalBet>
              <TotalBet
                total={betList.prawn.total}
                type="tom"
                isWin={settlementResult[FishPrawnCrabDice.prawn]}
              ></TotalBet>
            </Row>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList, settlementResult } = this.state;

    const renderBetList = CHOICES.map((choice) => (
      <Col className="col-4 col-lg-2">
        <BetList
          type="simple"
          dataList={betList[choice].dataList}
          titles={[this.renderBetListBau(FishPrawnCrabDice[choice])]}
          isWin={settlementResult[FishPrawnCrabDice[choice]]}
        />
      </Col>
    ));

    return <React.Fragment>{renderBetList}</React.Fragment>;
  }

  renderBetListBau(diceValue) {
    const dice = new DiceModel(diceValue, DiceType.BauCua);

    return <Dice diceModel={dice} isSmall={true}></Dice>;
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
            diceModel={new DiceModel(0, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(1, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(2, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(3, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(4, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(5, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
        </Row>
      </Modal>
    );
  }

  render() {
    return (
      <div className={styles["fish-prawn-crab"]}>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangeSystemFilter={this.onChangeSystemFilter}
          onChangePartnerFilter={this.onChangePartnerFilter}
          disabled={this.state.gameState == GameState.disabled}
          enableFilter={true}
          partners={this.props.partners}
        />
        {this.state.gameState == GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
