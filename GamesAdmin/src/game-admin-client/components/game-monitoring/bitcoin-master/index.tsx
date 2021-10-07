import React from "react";
import styles from "./styles.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Row, Modal, ModalHeader, ModalBody } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import BetItem from "../../common/model/BetItem";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import {
  BitcoinMasterResultModel,
  BitcoinMasterBetListModel,
  BitcoinMasterModel,
} from "../../common/model/BitcoinMasterModel";
import { Notice } from "../../shared/notice/notice";
import { updateTotalBetOfBetList } from "../../util/util";
import gameDurations from "./game-durations";
import EditableText from "../../shared/editable-text";

type BitcoinMasterProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

export default class BitcoinMaster extends React.Component<
  BitcoinMasterProps,
  BitcoinMasterModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new BitcoinMasterModel();
    this.renderBetList = this.renderBetList.bind(this);
    this.renderMonitor = this.renderMonitor.bind(this);
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleUM(data) {
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

  handleInGame(data) {
    if (!data.inGame || Object.keys(data.inGame).length == 0) {
      return;
    }

    this.setState({
      betList: this.getUserBetList(data.inGame, this.state.betList),
    });
  }

  handleFinish(data) {
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
        time: {
          remainingTime: gameDurations.wholeRound,
        },
      });
    }, (waitingTime + 4) * 1000);
  }

  handleRemainingTime(data) {
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

  handleData(data) {
    this.handleUM(data);
    this.handleInGame(data);
    this.handleFinish(data);
    this.handleRemainingTime(data);
  }

  getUserBetList(data, currentData: BitcoinMasterBetListModel) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      let betTracks = data[user].betTracks;
      Object.keys(betTracks).forEach((key) => {
        if (betTracks[key]) {
          currentData[key].total += betTracks[key];
          currentData[key].dataList.unshift(
            new BetItem(
              data[user].name,
              betTracks[key],
              null,
              null,
              data[user].system
            )
          );
        }
      });
    }

    return currentData;
  }

  setBettingPrice(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({
      result: {
        bettingPrice: parseFloat(event.target.value),
        resultPrice: this.state.result.resultPrice,
      },
    });
  }
  setResultPrice(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({
      result: {
        resultPrice: parseFloat(event.target.value),
        bettingPrice: this.state.result.bettingPrice,
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

  renderMonitor(systemFilter) {
    const { betList, roundNumber, time, result, settlementResult, gameState } =
      this.state;

    updateTotalBetOfBetList(betList, systemFilter);

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
    const bettingPrice = result.bettingPrice
      ? Math.floor(result.bettingPrice * 100) / 100
      : 0;

    const resultPrice = result.resultPrice
      ? Math.floor(result.resultPrice * 100) / 100
      : 0;

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

  renderBetList(systemFilter) {
    const { betList, settlementResult } = this.state;

    return (
      <React.Fragment>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.up.dataList}
            titles={[<div className={styles["text-green"]}>Up</div>]}
            isWin={settlementResult.up}
            systemFilter={systemFilter}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.down.dataList}
            titles={[<div className={styles["text-red"]}>Down</div>]}
            isWin={settlementResult.down}
            systemFilter={systemFilter}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.odd.dataList}
            titles={[<div className={styles["text-grey"]}>Odd</div>]}
            isWin={settlementResult.odd}
            systemFilter={systemFilter}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.even.dataList}
            titles={[<div className={styles["text-black"]}>Even</div>]}
            isWin={settlementResult.even}
            systemFilter={systemFilter}
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
          disabled={this.state.gameState === GameState.disabled}
          enableFilter={true}
        />
        {this.state.gameState === GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
