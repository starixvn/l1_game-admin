import React from "react";
import { Col, Container, Modal, Row } from "reactstrap";

import DiceModel from "../../common/model/DiceModel";
import DiceType from "../../common/model/DiceType";
import {
  VipFishPrawnCrabBetList,
  VipFishPrawnCrabModel,
  VipFishPrawnCrabSettlementResult,
} from "../../common/model/VipFishPrawnCrabModel";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";
import BetList from "../../shared/bet-list/bet-list";
import { GameState } from "../../shared/common/GameState";
import DiceResult from "../../shared/dice-result/dice-result";
import Dice from "../../shared/dice/dice";
import { Notice } from "../../shared/notice/notice";
import RoundNum from "../../shared/round-num/round-num";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import GameTemplate from "../game-template/game-template";
import styles from "./vip-fish-prawn-crab.module.scss";

type VipFishPrawnCrabProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

enum FishPrawnCrabDice {
  nai = 0,
  bau,
  ga,
  ca,
  cua,
  tom,
}

export default class VipFishPrawnCrab extends BaseMonitoring<VipFishPrawnCrabModel> {
  constructor(props) {
    super(props);

    this.state = new VipFishPrawnCrabModel();
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  get totalTimeInRound(): number {
    return 30;
  }

  get waitingTime(): number {
    return 13;
  }

  get defaultBetList(): {} {
    return new VipFishPrawnCrabBetList();
  }

  get defaultResult(): any {
    return [
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
    ];
  }

  get defaultSettlementResult(): any {
    return new VipFishPrawnCrabSettlementResult();
  }

  getResult(dices) {
    const result = new Array<DiceModel>();

    result.push(new DiceModel(dices.dice1, DiceType.BauCua));
    result.push(new DiceModel(dices.dice2, DiceType.BauCua));
    result.push(new DiceModel(dices.dice3, DiceType.BauCua));

    return result;
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
    const { betList, roundNumber, time, result, gameState, settlementResult } =
      this.state;

    return (
      <Container>
        <Row className={styles["align-middle"]}>
          <Col align="center" className="col-12 col-md-4">
            <Row className={styles["align-middle"]}>
              <Col className={styles.timecounter}>
                <div className={styles.title}>VIP Fish Prawn Crab</div>
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
                total={betList.stag.total}
                type="nai"
                isWin={settlementResult.stag}
              ></TotalBet>
              <TotalBet
                total={betList.gourd.total}
                type="bau"
                isWin={settlementResult.gourd}
              ></TotalBet>
              <TotalBet
                total={betList.rooster.total}
                type="ga"
                isWin={settlementResult.rooster}
              ></TotalBet>
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.fish.total}
                type="ca"
                isWin={settlementResult.fish}
              ></TotalBet>
              <TotalBet
                total={betList.crab.total}
                type="cua"
                isWin={settlementResult.crab}
              ></TotalBet>
              <TotalBet
                total={betList.prawn.total}
                type="tom"
                isWin={settlementResult.prawn}
              ></TotalBet>
            </Row>
          </Col>
          <Col align="center" className="col-2">
            <Row className="justify-content-center">
              <TotalBet
                total={betList.mountain.total}
                type="mountain"
                isWin={settlementResult.mountain}
              />
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.ocean.total}
                type="ocean"
                isWin={settlementResult.ocean}
              />
            </Row>
          </Col>
          <Col align="center" className="col-2">
            <TotalBet
              total={betList.anyTriple.total}
              type="anyTriple"
              isWin={settlementResult.anyTriple}
            ></TotalBet>
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
            type="simple"
            dataList={betList.stag.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.nai)]}
            isWin={settlementResult.stag}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.gourd.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.bau)]}
            isWin={settlementResult.gourd}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.rooster.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.ga)]}
            isWin={settlementResult.rooster}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.fish.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.ca)]}
            isWin={settlementResult.fish}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.crab.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.cua)]}
            isWin={settlementResult.crab}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.prawn.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.tom)]}
            isWin={settlementResult.prawn}
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
            type="simple"
            dataList={betList.mountain.dataList}
            titles={[<div className="text-center">Mountain</div>]}
            isWin={settlementResult.mountain}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.ocean.dataList}
            titles={[<div className="text-center">Ocean</div>]}
            isWin={settlementResult.ocean}
          />
        </Col>
      </React.Fragment>
    );
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
          enableFilter={true}
          onChangeSystemFilter={this.onChangeSystemFilter}
          onChangePartnerFilter={this.onChangePartnerFilter}
          disabled={this.state.gameState == GameState.disabled}
          partners={this.props.partners}
        />
        {this.state.gameState == GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
