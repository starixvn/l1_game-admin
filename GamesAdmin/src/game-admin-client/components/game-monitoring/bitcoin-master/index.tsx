import React from "react";
import styles from "./styles.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import { Col, Container, Row } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import {
  BitcoinMasterResultModel,
  BitcoinMasterBetListModel,
  BitcoinMasterModel,
  SettlementResultModel,
} from "../../common/model/BitcoinMasterModel";
import { Notice } from "../../shared/notice/notice";
import gameDurations from "./game-durations";
import EditableText from "../../shared/editable-text";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";

export default class BitcoinMaster extends BaseMonitoring<BitcoinMasterModel> {
  constructor(props) {
    super(props);

    this.state = {
      ...new BitcoinMasterModel(),
      betList: this.defaultBetList,
      result: this.defaultResult,
      settlementResult: this.defaultSettlementResult,
    };
  }

  get defaultBetList(): {} {
    return new BitcoinMasterBetListModel();
  }

  get defaultResult(): any {
    return new BitcoinMasterResultModel();
  }

  get defaultSettlementResult(): any {
    return new SettlementResultModel();
  }

  umHandler(data) {
    if (!data.um) {
      return;
    }

    if (
      this.isFirstLoad ||
      this.state.time.remainingTime == gameDurations.wholeRound
    ) {
      clearTimeout(this.timeout);
      const model = new BitcoinMasterModel();
      this.setState({
        time: { remainingTime: 0 },
        gameState: GameState.disabled,
        roundNumber: this.isFirstLoad
          ? this.state.roundNumber
          : this.state.roundNumber - 1,
        result: model.result,
      });
      this.isFirstLoad = false;
    }
  }

  finishHandler(data) {
    if (!data.finish) {
      return;
    }

    this.isFirstLoad = false;
    const waitingTime = gameDurations.waitingTime;
    this.setState({
      result: data.finish.result,
      settlementResult: data.finish.settlementResult,
      gameState: GameState.waiting,
      time: {
        remainingTime: waitingTime,
      },
    });

    setTimeout(() => {
      const newState = new BitcoinMasterModel();

      this.setState({
        ...newState,
        gameState: GameState.running,
        roundNumber: this.state.roundNumber + 1,
        betList: this.defaultBetList,
        result: this.defaultResult,
        settlementResult: this.defaultSettlementResult,
        time: {
          remainingTime: gameDurations.wholeRound,
        },
      });
    }, (waitingTime + 4) * 1000);
  }

  remainingTimeHandler(data) {
    if (!data.remainingTime) {
      return;
    }

    if (data.remainingTime > gameDurations.overTime) {
      this.setState((prevState) => ({
        gameState: GameState.waiting,
        roundNumber: data.roundId - 1,
        result: data.result ? data.result : prevState.result,
        time: {
          remainingTime: data.remainingTime - gameDurations.overTime,
        },
      }));

      this.timeout = setTimeout(() => {
        this.setState({
          gameState: GameState.running,
          roundNumber: data.roundId,
          time: {
            remainingTime: gameDurations.overTime,
          },
        });
      }, (data.remainingTime - gameDurations.overTime) * 1000);
    } else {
      this.setState({
        gameState: GameState.running,
        roundNumber: data.roundId,
        time: {
          remainingTime: data.remainingTime,
        },
      });
    }
  }

  setBettingPrice(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({
      result: {
        open: parseFloat(event.target.value),
        close: this.state.result.close,
      },
    });
  }
  setResultPrice(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({
      result: {
        close: parseFloat(event.target.value),
        open: this.state.result.open,
      },
    });
  }

  sendAdminResult() {
    this.props.sendMessage({
      set_result: {
        roundId: this.state.roundNumber,
        result: this.state.result,
      },
    });
  }

  renderMonitor() {
    const { roundNumber, time, result, gameState } = this.state;

    const priceDiffStyle = this.getPriceDiffStyle(result);

    return (
      <Container className={styles["bitcoin-master-container"]}>
        <Row>
          <Col align="center" className="my-auto col-2">
            <TimeCounter
              remainingTime={time.remainingTime}
              gameState={gameState}
              key={gameState}
            />
          </Col>
          <Col align="center" className="my-auto">
            <div className={styles.title}>Bitcoin Master</div>
            <RoundNum roundNumber={roundNumber} />
          </Col>
          <Col align="center" className="col-2">
            <div className={`${styles.title} ${styles["result-price-title"]}`}>
              Betting Price
            </div>
            <EditableText
              disabled={this.props.isProduction}
              name="betting-price"
              labelStyle={styles["result-price"]}
              inputStyle={styles["txt-result-price"]}
              onChange={(e) => this.setBettingPrice(e)}
              onBlur={() => this.sendAdminResult()}
              value={priceDiffStyle.bettingPrice.toFixed(2)}
            />
          </Col>
          <Col align="center" className={styles["result-price-line"]}></Col>
          <Col align="center" className="col-2">
            <div className={`${styles.title} ${styles["result-price-title"]}`}>
              Result Price
            </div>
            <EditableText
              disabled={this.props.isProduction}
              name="result-price"
              labelStyle={styles["result-price"]}
              inputStyle={styles["txt-result-price"]}
              onChange={(e) => this.setResultPrice(e)}
              onBlur={() => this.sendAdminResult()}
              value={priceDiffStyle.resultPrice.toFixed(2)}
            />
          </Col>
          <Col align="center" className="col-2">
            <div className={priceDiffStyle.className}>
              {priceDiffStyle.icon} {priceDiffStyle.priceDiff.toFixed(2)}
            </div>
          </Col>
        </Row>
      </Container>
    );
  }

  getPriceDiffStyle(result) {
    const bettingPrice = result.open ? Math.floor(result.open * 100) / 100 : 0;
    const resultPrice = result.close ? Math.floor(result.close * 100) / 100 : 0;

    const priceDiff = result.priceDiff
      ? Math.floor(result.priceDiff * 100) / 100
      : resultPrice - bettingPrice;

    if (priceDiff > 0) {
      return {
        bettingPrice,
        resultPrice,
        priceDiff,
        className: styles["price-up"],
        icon: "▲",
      };
    }

    if (priceDiff < 0) {
      return {
        bettingPrice,
        resultPrice,
        priceDiff: Math.abs(priceDiff),
        className: styles["price-down"],
        icon: "▼",
      };
    }

    return {
      bettingPrice,
      resultPrice,
      priceDiff,
      className: styles["price-unchanged"],
      icon: "",
    };
  }

  renderBetList() {
    const { betList, settlementResult } = this.state;

    return (
      <React.Fragment>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.up.dataList}
            titles={[<div className={styles["text-green"]}>Up</div>]}
            isWin={settlementResult.up}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.down.dataList}
            titles={[<div className={styles["text-red"]}>Down</div>]}
            isWin={settlementResult.down}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.odd.dataList}
            titles={[<div className={styles["text-white"]}>Odd</div>]}
            isWin={settlementResult.odd}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.even.dataList}
            titles={[<div className={styles["text-black"]}>Even</div>]}
            isWin={settlementResult.even}
          />
        </Col>
      </React.Fragment>
    );
  }

  render() {
    return (
      <div className={styles["bitcoin-master"]}>
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          disabled={this.state.gameState === GameState.disabled}
          enableFilter={true}
          partners={this.props.partners}
        />
        {this.state.gameState === GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
