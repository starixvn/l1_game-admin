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
import { LotteryBetList, LotteryModel } from "../../common/model/LotteryModel";
import { Notice } from "../../shared/notice/notice";
import LotteryRoundResult from "../../shared/lottery-round-result";
import generateBoard from "./lottery-number-generator";

type LotteryProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

export default class Lottery extends React.Component<
  LotteryProps,
  LotteryModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new LotteryModel();
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.handlePrizeNumberChange = this.handlePrizeNumberChange.bind(this);
    this.handleGenerateResult = this.handleGenerateResult.bind(this);
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 40) {
        clearTimeout(this.timeout);
        const model = new LotteryModel();
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

    if (!!data.inGame && Object.keys(data.inGame).length > 0) {
      this.setState({
        betList: this.getUserBetList(data.inGame, this.state.betList),
      });
    }

    if (!!data.finish) {
      this.isFirstLoad = false;
      const waitingTime = 7;
      this.setState({
        result: data.finish.result,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        const newState = new LotteryModel();

        this.setState({
          ...newState,
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 45,
          },
        });
      }, (waitingTime + 1) * 1000);
    }

    if (!!data.remainingTime) {
      if (data.remainingTime > 46) {
        this.setState((prevState) => ({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result: data.result ? data.result : prevState.result,
          time: {
            remainingTime: data.remainingTime - 46,
          },
        }));

        this.timeout = setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 46,
            },
          });
        }, (data.remainingTime - 46) * 1000);
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
  }

  getUserBetList(data, currentData: LotteryBetList) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      const { betTracks } = data[user];
      Object.keys(betTracks)
        .filter((x) => x !== "totalAmount")
        .forEach((betTypeKey) => {
          Object.entries(betTracks[betTypeKey]).forEach(
            ([betChoice, amountOnChoice]) => {
              currentData[betTypeKey].dataList.unshift(
                new BetItem(
                  data[user].name,
                  amountOnChoice as number,
                  "",
                  betChoice.replaceAll("-", ",")
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
      betList["Sets4"].total +
      betList.special.total +
      betList["Sets27"].total +
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
              <TotalBet total={betList["Sets4"].total} type="Sets4" />
              <TotalBet total={betList.special.total} type="special" />
              <TotalBet total={betList.head.total} type="head" />
              <TotalBet total={betList.tail.total} type="tail" />
            </Row>
            <Row className="justify-content-center">
              <TotalBet total={betList["Sets27"].total} type="Sets27" />
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
            dataList={betList["Sets4"].dataList}
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
            dataList={betList["Sets27"].dataList}
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
          renderBetList={this.renderBetList()}
          renderMonitor={this.renderMonitor()}
          disabled={this.state.gameState === GameState.disabled}
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
