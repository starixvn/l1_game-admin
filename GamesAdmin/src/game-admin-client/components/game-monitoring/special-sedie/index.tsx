import React from "react";
import { Col, Container, Modal, Row } from "reactstrap";

import DiceModel from "../../common/model/DiceModel";
import DiceType from "../../common/model/DiceType";
import SpecialSeDieModel, {
  SpecialSeDieBetList,
} from "../../common/model/SpecialSeDieModel";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";
import BetList from "../../shared/bet-list/bet-list";
import { GameState } from "../../shared/common/GameState";
import Dice from "../../shared/dice/dice";
import RoundNum from "../../shared/round-num/round-num";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import styles from "./styles.module.scss";

export default class SpecialSeDie extends BaseMonitoring<SpecialSeDieModel> {
  constructor(props) {
    super(props);

    this.state = new SpecialSeDieModel();
    this.handleChangeResult = this.handleChangeResult.bind(this);
  }

  get totalTimeInRound(): number {
    return 31;
  }

  get waitingTime(): number {
    return 8;
  }

  get defaultResult(): any {
    return [
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
    ];
  }

  get defaultBetList(): {} {
    return new SpecialSeDieBetList();
  }

  get isExcludeUUS(): boolean {
    return true;
  }

  getResult(data) {
    const result = new Array<DiceModel>();

    Object.keys(data).forEach((key) => {
      result.push(new DiceModel(data[key], DiceType.XocXoc));
    });

    return result;
  }

  getWinner() {
    const { result } = this.state;
    const sumDicesValue = result.reduce((sum, dice) => sum + dice.value, 0);

    let winner = "";
    switch (sumDicesValue) {
      case 0:
        winner = "white4";
        break;
      case 1:
        winner = "white3";
        break;
      case 2:
        winner = "draw";
        break;
      case 3:
        winner = "red3";
        break;
      case 4:
        winner = "red4";
        break;
    }

    return winner;
  }

  renderBetList() {
    const { betList, gameState } = this.state;
    const winner = gameState == GameState.waiting ? this.getWinner() : "";

    return (
      <React.Fragment>
        <Col className="col-12 col-sm-6">
          <BetList
            titles={[<div className={styles["title-chan"]}>Even</div>]}
            type="simple"
            dataList={betList.even.dataList}
            isWin={winner != "" && winner != "white3" && winner != "red3"}
          />
        </Col>
        <Col className="col-12 col-sm-6">
          <BetList
            titles={[<div className={styles["title-le"]}>Odd</div>]}
            type="simple"
            dataList={betList.odd.dataList}
            isWin={winner == "white3" || winner == "red3"}
          />
        </Col>
      </React.Fragment>
    );
  }

  toggleModal() {
    this.setState({
      isOpenControlResultModal: !this.state.isOpenControlResultModal,
    });
  }

  openModal(diceIndex) {
    this.setState({
      isOpenControlResultModal: true,
      currentDiceNumber: diceIndex,
    });
  }

  handleChangeResult(id, value) {
    const { currentDiceNumber, result } = this.state;
    const gameResult = this.state.nextRoundResult;

    const newResult = result.map((diceInfo, index) => {
      if (index === currentDiceNumber || diceInfo.value === -1) {
        diceInfo.value = value;
        gameResult[`red${index + 1}`] = !!value;
      }

      return diceInfo;
    });

    this.setState({
      result: newResult,
      isOpenControlResultModal: false,
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

  renderGameControl() {
    const { isOpenControlResultModal } = this.state;

    return (
      <Modal
        isOpen={isOpenControlResultModal}
        toggle={() => this.toggleModal()}
        className={styles["game-control-panel"]}
      >
        <Row>
          <Dice
            key={`dice-select-${0}`}
            diceModel={new DiceModel(0, DiceType.XocXoc)}
            isSmall={false}
            onClickHandle={(id: string, value: number) =>
              this.handleChangeResult(id, value)
            }
          />
          <Dice
            key={`dice-select-${1}`}
            diceModel={new DiceModel(1, DiceType.XocXoc)}
            isSmall={false}
            onClickHandle={(id: string, value: number) =>
              this.handleChangeResult(id, value)
            }
          />
        </Row>
      </Modal>
    );
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState } = this.state;
    const winer = gameState == GameState.waiting ? this.getWinner() : "";

    return (
      <Container className={styles["shakeThePlate-container"]}>
        <Row>
          <Col>
            <TotalBet
              type="plateeven"
              total={betList.even.total}
              isWin={winer != "" && winer != "white3" && winer != "red3"}
            />
          </Col>
          <Col align="center" className={styles["shakeThePlate-timmer"]}>
            <div>
              <div className={styles.title}>Special Se Die</div>
              <TimeCounter
                remainingTime={time.remainingTime}
                gameState={gameState}
              />
              <Row className={styles.result}>
                <Dice
                  diceModel={result[0]}
                  isSmall={false}
                  onClickHandle={() => this.openModal(0)}
                />
                <Dice
                  diceModel={result[1]}
                  isSmall={false}
                  onClickHandle={() => this.openModal(1)}
                />
                <Dice
                  diceModel={result[2]}
                  isSmall={false}
                  onClickHandle={() => this.openModal(2)}
                />
                <Dice
                  diceModel={result[3]}
                  isSmall={false}
                  onClickHandle={() => this.openModal(3)}
                />
              </Row>
              <RoundNum roundNumber={roundNumber} />
            </div>
          </Col>
          <Col>
            <TotalBet
              type="plateodd"
              total={betList.odd.total}
              isWin={winer == "white3" || winer == "red3"}
            />
          </Col>
        </Row>
      </Container>
    );
  }
}
