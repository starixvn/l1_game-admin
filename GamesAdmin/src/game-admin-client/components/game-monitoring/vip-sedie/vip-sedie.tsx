import React from "react";
import { Col, Container, Row } from "reactstrap";

import BetItem, { SystemId } from "../../common/model/BetItem";
import DiceModel from "../../common/model/DiceModel";
import DiceType from "../../common/model/DiceType";
import ShakeThePlateModel from "../../common/model/ShakeThePlateModel";
import BetList from "../../shared/bet-list/bet-list";
import { GameState } from "../../shared/common/GameState";
import Dice from "../../shared/dice/dice";
import RoundNum from "../../shared/round-num/round-num";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { getTotalBetOnSystem } from "../../util/util";
import GameTemplate from "../game-template/game-template";
import styles from "./vip-sedie.module.scss";

type VipSeDieProps = {
  onMessage: Function;
};

const CHOICES = ["stag", "gourd", "rooster", "fish", "crab", "prawn"];

export default class VipSeDie extends React.Component<
  VipSeDieProps,
  ShakeThePlateModel
> {
  private timeout: NodeJS.Timeout;
  private systemFilter: SystemId;
  constructor(props) {
    super(props);

    this.state = new ShakeThePlateModel();
    this.systemFilter = SystemId.None;
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    if (!!data.um) {
      clearTimeout(this.timeout);
      this.setState({
        disabled: data.um,
        time: { remainingTime: 0 },
        gameState: GameState.none,
      });
    }

    if (!!data.inGame) {
      this.setState({
        betList: this.getUserBetList(data.inGame, this.state.betList),
      });
    }

    if (!!data.remainingTime) {
      this.setState({ disabled: false });
      if (data.remainingTime > 30) {
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          time: {
            remainingTime: data.remainingTime - 31,
          },
          result: data.result ? this.getResult(data.result) : this.state.result,
        });

        setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: this.state.roundNumber + 1,
            time: {
              remainingTime: 30,
            },
            result: [
              new DiceModel(-1, DiceType.XocXoc),
              new DiceModel(-1, DiceType.XocXoc),
              new DiceModel(-1, DiceType.XocXoc),
              new DiceModel(-1, DiceType.XocXoc),
            ],
          });
        }, (data.remainingTime - 30) * 1000);
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
      const waitingTime = 10;
      this.setState({
        result: this.getResult(data.finish.result),
        settlementResult: data.finish.settlementResult,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      this.timeout = setTimeout(() => {
        this.setState({
          ...new ShakeThePlateModel(),
          result: [
            new DiceModel(-1, DiceType.XocXoc),
            new DiceModel(-1, DiceType.XocXoc),
            new DiceModel(-1, DiceType.XocXoc),
            new DiceModel(-1, DiceType.XocXoc),
          ],
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 30,
          },
        });
      }, waitingTime * 1000);
    }
  }

  getUserBetList(data, currentData) {
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

  getResult(data) {
    const result = new Array<DiceModel>();

    Object.keys(data).forEach((key) => {
      result.push(new DiceModel(data[key] ? 1 : 0, DiceType.XocXoc));
    });

    return result;
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState, settlementResult } =
      this.state;

    if (!this.systemFilter) {
      CHOICES.forEach((choice) => {
        betList[choice].total = getTotalBetOnSystem(
          betList[choice].dataList,
          this.systemFilter
        );
      });
    }

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
                <Dice diceModel={result[0]} isSmall={false} />
                <Dice diceModel={result[1]} isSmall={false} />
                <Dice diceModel={result[2]} isSmall={false} />
                <Dice diceModel={result[3]} isSmall={false} />
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
            systemFilter={this.systemFilter}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles.mottrangbado}></div>]}
            type="simple"
            dataList={betList.red3.dataList}
            isWin={settlementResult.red3}
            systemFilter={this.systemFilter}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles.haido}></div>]}
            type="simple"
            dataList={betList.red2.dataList}
            isWin={settlementResult.red2}
            systemFilter={this.systemFilter}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles["title-chan"]}>Even</div>]}
            type="simple"
            dataList={betList.even.dataList}
            isWin={settlementResult.even}
            systemFilter={this.systemFilter}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles["title-le"]}>Odd</div>]}
            type="simple"
            dataList={betList.odd.dataList}
            isWin={settlementResult.odd}
            systemFilter={this.systemFilter}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles.motdobatrang}></div>]}
            type="simple"
            dataList={betList.white3.dataList}
            isWin={settlementResult.white3}
            systemFilter={this.systemFilter}
          />
        </Col>
        <Col className="col">
          <BetList
            titles={[<div className={styles.bontrang}></div>]}
            type="simple"
            dataList={betList.white4.dataList}
            isWin={settlementResult.white4}
            systemFilter={this.systemFilter}
          />
        </Col>
      </React.Fragment>
    );
  }

  render() {
    return (
      <div>
        <GameTemplate
          renderBetList={this.renderBetList()}
          renderMonitor={this.renderMonitor()}
          disabled={this.state.disabled}
          onUpdateFilter={(value: SystemId) => (this.systemFilter = value)}
        ></GameTemplate>
      </div>
    );
  }
}
