import React from "react";
import styles from "./styles.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Row } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import BetItem from "../../common/model/BetItem";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import {
  LotteryBetList,
  LotteryGameResult,
  LotteryModel,
} from "../../common/model/LotteryModel";
import { Notice } from "../../shared/notice/notice";
import LotteryRoundResult from "../../shared/lottery-round-result";
import generateBoard from "./lottery-number-generator";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";

export default class Lottery extends BaseMonitoring<LotteryModel> {
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new LotteryModel();
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.handlePrizeNumberChange = this.handlePrizeNumberChange.bind(this);
    this.handleGenerateResult = this.handleGenerateResult.bind(this);
    this.renderMonitor = this.renderMonitor.bind(this);
    this.renderBetList = this.renderBetList.bind(this);
  }

  get defaultResult() {
    return new LotteryGameResult();
  }

  get defaultSettlementResult() {
    return null;
  }

  get totalTimeInRound() {
    return 46;
  }

  get waitingTime() {
    return 7;
  }

  get defaultBetList() {
    return new LotteryBetList();
  }

  getUserBetList(data, currentData: LotteryBetList) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      const { betTracks, system, currency, partnerId, name } = data[user];
      const userRate = this.currencyExchangeRate[currency];

      Object.keys(betTracks)
        .filter((x) => x !== "totalAmount")
        .forEach((betTypeKey) => {
          Object.entries(betTracks[betTypeKey]).forEach(
            ([betChoice, amountOnChoice]) => {
              const baseBetAmount = parseFloat(
                (+amountOnChoice * userRate).toFixed(2)
              );
              currentData[betTypeKey].dataList.unshift(
                new BetItem(
                  name,
                  baseBetAmount,
                  "",
                  betChoice.replaceAll("-", ","),
                  system,
                  currency,
                  partnerId
                )
              );
              currentData[betTypeKey].total += amountOnChoice;
            }
          );
        });
    }

    return currentData;
  }

  handleGenerateResult() {
    this.setState({ result: generateBoard() });
  }

  handleChangeResult() {
    const { result } = this.state;

    this.props.sendMessage({
      set_result: {
        roundId: this.state.roundNumber,
        result,
      },
    });

    this.setState({
      result,
      isOpenModal: true,
    });

    if (this.state.gameState == GameState.waiting) {
      const gameResult = this.state.nextRoudResult;

      this.setState({
        isHandleNextRoundResult: true,
        nextRoudResult: gameResult,
      });
    }

    setTimeout(() => this.setState({ isOpenModal: false }), 1000);
  }

  handlePrizeNumberChange(event: React.ChangeEvent<HTMLInputElement>) {
    const [prize, index] = event.target.name.split(".");
    const { result } = this.state;
    result[prize][index] = event.target.value;

    this.setState({ result });
  }

  calculateTotalBet() {
    const { betList } = this.state;
    return (
      betList["4Sets"].total +
      betList.special.total +
      betList["27Sets"].total +
      betList.head.total +
      betList.tail.total +
      betList.others.total
    );
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState } = this.state;

    return (
      <Container>
        <Row className={styles["align-middle"]}>
          <Col className="col-12 col-md-5">
            <LotteryRoundResult
              isProduction={this.props.isProduction}
              result={result}
              onChange={this.handlePrizeNumberChange}
            />
            <button
              className="btn btn-success mr-1"
              onClick={this.handleGenerateResult}
            >
              Generate
            </button>
            <button
              className="btn btn-primary"
              onClick={this.handleChangeResult}
            >
              Save
            </button>
          </Col>
          <Col align="center" className="col-12 col-md-3">
            <Row className={styles["align-middle"]}>
              <Col className={styles.timecounter}>
                <div className={styles.title}>RNG Lottery</div>
                <TimeCounter
                  remainingTime={time.remainingTime}
                  gameState={gameState}
                ></TimeCounter>
                <RoundNum roundNumber={roundNumber} />
              </Col>
            </Row>
          </Col>
          <Col align="center" className="col-4">
            <Row className="justify-content-center">
              <TotalBet total={betList["4Sets"].total} type="4Sets" />
              <TotalBet total={betList.special.total} type="special" />
              <TotalBet total={betList.head.total} type="head" />
              <TotalBet total={betList.tail.total} type="tail" />
            </Row>
            <Row className="justify-content-center">
              <TotalBet total={betList["27Sets"].total} type="27Sets" />
              <TotalBet total={betList.others.total} type="others" />
            </Row>
            <Row className="justify-content-center">
              <TotalBet total={this.calculateTotalBet()} type="turnover" />
            </Row>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList } = this.state;

    return (
      <React.Fragment>
        <Col className="col-4 col-lg">
          <BetList
            type="withchoice"
            dataList={betList["4Sets"].dataList}
            titles={[<div className="text-center">4 Sets</div>]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="withchoice"
            dataList={betList.special.dataList}
            titles={[<div className="text-center">Special</div>]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="withchoice"
            dataList={betList.head.dataList}
            titles={[<div className="text-center">Head</div>]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="withchoice"
            dataList={betList.tail.dataList}
            titles={[<div className="text-center">Tail</div>]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="withchoice"
            dataList={betList["27Sets"].dataList}
            titles={[<div className="text-center">27 Sets</div>]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="withchoice"
            dataList={betList.others.dataList}
            titles={[<div className="text-center">Others</div>]}
          />
        </Col>
      </React.Fragment>
    );
  }

  render() {
    return (
      <div className={styles["-sic-bo"]}>
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
        <Notice
          isOpen={this.state.isOpenModal}
          message="Generated result has been saved!"
        />
      </div>
    );
  }
}
