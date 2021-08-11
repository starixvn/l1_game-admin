import React from "react";
import { Col, Container, Row } from "reactstrap";

import BetItem from "../../common/model/BetItem";
import DiceModel from "../../common/model/DiceModel";
import DiceType from "../../common/model/DiceType";
import SpecialSeDieModel from "../../common/model/SpecialSeDieModel";
import BetList from "../../shared/bet-list/bet-list";
import { GameState } from "../../shared/common/GameState";
import Dice from "../../shared/dice/dice";
import RoundNum from "../../shared/round-num/round-num";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { updateTotalBetOfBetList } from "../../util/util";
import GameTemplate from "../game-template/game-template";
import styles from "./styles.module.scss";

type Props = {
  onMessage: Function;
};

export default class SpecialSeDie extends React.Component<
  Props,
  SpecialSeDieModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;

  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new SpecialSeDieModel();
    this.renderMonitor = this.renderMonitor.bind(this);
    this.renderBetList = this.renderBetList.bind(this);
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 31) {
        clearTimeout(this.timeout);
        this.setState({
          time: { remainingTime: 0 },
          gameState: GameState.disabled,
          roundNumber: this.isFirstLoad
            ? this.state.roundNumber
            : this.state.roundNumber - 1,
          result: new SpecialSeDieModel().result,
        });

        this.isFirstLoad = false;
      }
    }

    if (!!data.inGame) {
      this.setState({
        betList: this.getUserBetList(data.inGame, this.state.betList),
      });
    }

    if (!!data.remainingTime) {
      if (data.remainingTime > 31) {
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result: data.result ? this.getResult(data.result) : this.state.result,
          time: {
            remainingTime: data.remainingTime - 31,
          },
        });

        this.timeout = setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 31,
            },
          });
        }, (data.remainingTime - 31) * 1000);
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

    if (!!data.finish) {
      this.isFirstLoad = false;
      const waitingTime = 8;
      this.setState({
        result: this.getResult(data.finish.result),
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        this.setState({
          result: new SpecialSeDieModel().result,
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 31,
          },
        });
      }, (waitingTime + 1) * 1000);
    }
  }

  getUserBetList(data, currentData) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      const { currency, betTracks } = data[user];
      Object.keys(betTracks).forEach((key) => {
        if (betTracks[key]) {
          if (!currency || currency.toUpperCase() !== "UUS") {
            currentData[key].total += betTracks[key];
          }

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

  renderBetList(systemFilter) {
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
            systemFilter={systemFilter}
          />
        </Col>
        <Col className="col-12 col-sm-6">
          <BetList
            titles={[<div className={styles["title-le"]}>Odd</div>]}
            type="simple"
            dataList={betList.odd.dataList}
            isWin={winner == "white3" || winner == "red3"}
            systemFilter={systemFilter}
          />
        </Col>
      </React.Fragment>
    );
  }

  renderMonitor(systemFilter) {
    const { betList, roundNumber, time, result, gameState } = this.state;
    const winer = gameState == GameState.waiting ? this.getWinner() : "";

    updateTotalBetOfBetList(betList, systemFilter, true);

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
                <Dice diceModel={result[0]} isSmall={false} />
                <Dice diceModel={result[1]} isSmall={false} />
                <Dice diceModel={result[2]} isSmall={false} />
                <Dice diceModel={result[3]} isSmall={false} />
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

  render() {
    return (
      <div>
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          disabled={this.state.gameState == GameState.disabled}
          enableFilter={true}
        ></GameTemplate>
      </div>
    );
  }
}
