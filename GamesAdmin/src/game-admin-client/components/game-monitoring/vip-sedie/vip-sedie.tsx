import React from "react";
import { Col, Container, Modal, Row } from "reactstrap";

import DiceModel from "../../common/model/DiceModel";
import DiceType from "../../common/model/DiceType";
import ShakeThePlateModel, {
  SeDieSettlementResult,
  ShakeThePlateBetList,
} from "../../common/model/ShakeThePlateModel";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";
import BetList from "../../shared/bet-list/bet-list";
import { GameState } from "../../shared/common/GameState";
import Dice from "../../shared/dice/dice";
import RoundNum from "../../shared/round-num/round-num";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import styles from "./vip-sedie.module.scss";

export default class VipSeDie extends BaseMonitoring<ShakeThePlateModel> {
  constructor(props) {
    super(props);

    this.state = new ShakeThePlateModel();
  }

  get defaultResult() {
    return [
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
    ];
  }

  get defaultSettlementResult() {
    return new SeDieSettlementResult();
  }

  get totalTimeInRound(): number {
    return 30;
  }

  get waitingTime(): number {
    return 10;
  }

  get defaultBetList(): {} {
    return new ShakeThePlateBetList();
  }

  getResult(data) {
    const result = new Array<DiceModel>();

    Object.keys(data).forEach((key) => {
      result.push(new DiceModel(data[key] ? 1 : 0, DiceType.XocXoc));
    });

    return result;
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
    const { betList, roundNumber, time, result, gameState, settlementResult } =
      this.state;

    return (
      <Container className={styles["shakeThePlate-container"]}>
        <Row>
          <Col align="center" className={styles["shakeThePlate-timmer"]}>
            <TimeCounter
              remainingTime={time.remainingTime}
              gameState={gameState}
              key={gameState}
            />
            <div>
              <div className={styles.title}>VIP Se Die</div>
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
          <Col align="center" className={styles["shakeThePlate-betList"]}>
            <Col className={styles["col-plateeven"]}>
              <TotalBet
                type="plateeven"
                total={betList.even.total}
                isWin={settlementResult.even}
              />
            </Col>
            <Col className={styles["col-plateodd"]}>
              <TotalBet
                type="plateodd"
                total={betList.odd.total}
                isWin={settlementResult.odd}
              />
            </Col>
            <Col>
              <TotalBet
                type="bondo"
                total={betList.red4.total}
                isWin={settlementResult.red4}
              />
            </Col>
            <Col>
              <TotalBet
                type="mottrangbado"
                total={betList.red3.total}
                isWin={settlementResult.red3}
              />
            </Col>
            <Col>
              <TotalBet
                type="haido"
                total={betList.red2.total}
                isWin={settlementResult.red2}
              />
            </Col>
            <Col>
              <TotalBet
                type="motdobatrang"
                total={betList.white3.total}
                isWin={settlementResult.white3}
              />
            </Col>
            <Col>
              <TotalBet
                type="bontrang"
                total={betList.white4.total}
                isWin={settlementResult.white4}
              />
            </Col>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList, settlementResult } = this.state;

    return (
      <React.Fragment>
        <Col className="col">
          <BetList
            titles={[<div className={styles.bondo}></div>]}
            type="simple"
            dataList={betList.red4.dataList}
            isWin={settlementResult.red4}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles.mottrangbado}></div>]}
            type="simple"
            dataList={betList.red3.dataList}
            isWin={settlementResult.red3}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles.haido}></div>]}
            type="simple"
            dataList={betList.red2.dataList}
            isWin={settlementResult.red2}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles["title-chan"]}>Even</div>]}
            type="simple"
            dataList={betList.even.dataList}
            isWin={settlementResult.even}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles["title-le"]}>Odd</div>]}
            type="simple"
            dataList={betList.odd.dataList}
            isWin={settlementResult.odd}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles.motdobatrang}></div>]}
            type="simple"
            dataList={betList.white3.dataList}
            isWin={settlementResult.white3}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles.bontrang}></div>]}
            type="simple"
            dataList={betList.white4.dataList}
            isWin={settlementResult.white4}
          />
        </Col>
      </React.Fragment>
    );
  }
}
