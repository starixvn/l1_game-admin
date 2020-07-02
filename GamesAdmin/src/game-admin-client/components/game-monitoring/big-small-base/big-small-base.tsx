import React from "react";
import styles from "./big-small-base.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import DiceResult from "../../shared/dice-result/dice-result";
import TotalBet from "../../shared/total-bet/total-bet";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import { Container, Row, Col } from "reactstrap";
import TotalUser from "../../shared/total-user/total-user";
import RoundNum from "../../shared/round-num/round-num";
import DiceModel from "../../common/model/DiceModel";
import BigSmallBaseModel from '../../common/model/BigSmallBaseModel';
import DiceType from '../../common/model/DiceType';
import BetItem from '../../common/model/BetItem';
import { types, gameMapping, gameFlagMapping } from '../game-monitoring-config';
import { GameState } from '../../shared/common/GameState'
import { Notice } from "../../shared/notice/notice";
import { time } from "console";

type BigSmalllProps = {
  turbo: boolean,
  type: string,
  onMessage: Function
}

const betListMapping = (gameType: string, flag: boolean) => {
  const betListLabels = ['firstBetType', 'secondBetType'];
  const reducer = (result: number, gameName: string) => {
    if (gameType === gameName) {
      const mapping: Array<string> = gameMapping[gameName];
      const choice = gameFlagMapping[gameName][flag ? 1 : 0];
      result = mapping.indexOf(choice);
    }

    return result;
  }

  const index = Object.keys(gameMapping).reduce(reducer, 0);

  return betListLabels[index];
}

export default class BigSmallBase extends React.Component<BigSmalllProps, BigSmallBaseModel> {
  constructor(props) {
    super(props);

    this.state = new BigSmallBaseModel();
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    const { turbo, type } = this.props;
    const totalRemainingTime = this.props.turbo ? 30 : 50;
    const totalWaitingTime = this.props.turbo ? 5 : 10;
    const gameType = turbo && type == 'chanle' ? 'clturbo' : type;

    if (data.taixiu != null || data.chanle != null || data.clturbo != null) {
      this.setState({
        betList: this.getTotalBetList(data[gameType], this.state.betList, gameType),
        firstBetTypeUsers: data[gameType][`red_player_${gameMapping[gameType][0]}`],
        secondBetTypeUsers: data[gameType][`red_player_${gameMapping[gameType][1]}`]
      })

    }

    if (!!data.list) {
      this.setState({
        betList: this.getUserBetList(data.list, this.state.betList)
      })
    }

    if (!!data.finish) {
      setTimeout(() => {
        this.setState({
          result: [
            new DiceModel(-1, DiceType.TaiXiu),
            new DiceModel(-1, DiceType.TaiXiu),
            new DiceModel(-1, DiceType.TaiXiu)
          ],
          isHavingResult: false,
          currentState: GameState.running,
          time: {
            remainingTime: totalRemainingTime
          }
        })
      }, (totalWaitingTime + 1) * 1000);

      this.setState({
        result: this.getResult(data.finish),
        isHavingResult: true,
        currentState: GameState.waiting,
        time: {
          remainingTime: totalWaitingTime
        }
      });
    }

    if (!!data.um) {
      this.setState({
        disabled: data.um,
        time: { remainingTime: 0 },
        currentState: GameState.none
      });
    }

    if (!!data.time_remain) {
      this.setState({ disabled: false });
      const { time_remain, phien } = data;
      if ((this.props.turbo && time_remain > totalRemainingTime + 6) || time_remain > totalRemainingTime) {
        this.setState({
          time: {
            remainingTime: time_remain - totalRemainingTime - 1
          },
          roundNumber: phien - 1,
          currentState: GameState.waiting
        })

        setTimeout(() => {
          this.setState({
            time: {
              remainingTime: totalRemainingTime,
            },
            roundNumber: phien,
            currentState: GameState.running,
            isHavingResult: false,
            result: [
              new DiceModel(-1, DiceType.TaiXiu),
              new DiceModel(-1, DiceType.TaiXiu),
              new DiceModel(-1, DiceType.TaiXiu)
            ]
          })
        }, (time_remain - totalRemainingTime - 1) * 1000)
      } else {
        this.setState({
          currentState: GameState.running,
          time: {
            remainingTime: time_remain - 1
          },
          roundNumber: phien,
          isHavingResult: false,
          result: [
            new DiceModel(-1, DiceType.TaiXiu),
            new DiceModel(-1, DiceType.TaiXiu),
            new DiceModel(-1, DiceType.TaiXiu)
          ]
        })
      }
    }
  }

  getTotalBetList(data, currentData, gameType) {
    currentData.firstBetType.total = data[`red_${gameMapping[gameType][0]}`];
    currentData.secondBetType.total = data[`red_${gameMapping[gameType][1]}`];

    return currentData;
  }

  getUserBetList(data, currentData) {
    const { type } = this.props;

    Object.keys(currentData).forEach(key => {
      currentData[key].dataList = new Array<BetItem>();
    });

    data.forEach(user => {
      currentData[betListMapping(type, user.select)].dataList.push(new BetItem(user.name, user.bet, user.time));
    });

    return currentData;
  }

  getResult(data) {
    const result = new Array<DiceModel>();

    result.push(new DiceModel(data.dices[0], DiceType.TaiXiu));
    result.push(new DiceModel(data.dices[1], DiceType.TaiXiu));
    result.push(new DiceModel(data.dices[2], DiceType.TaiXiu));

    return result;
  }

  getWiner() {
    const { type } = this.props;
    const { result } = this.state;

    const sumDicesValue = result.reduce((sum, dice) => sum + dice.value, 0);
    const winner = type == types.taixiu
      ? sumDicesValue >= 11 ? 'tai' : 'xiu'
      : sumDicesValue % 2 == 1 ? 'le' : 'chan';

    return winner;
  }

  renderMonitor() {
    const { turbo, type } = this.props;
    const { betList, time, roundNumber, result, firstBetTypeUsers, secondBetTypeUsers, isHavingResult, currentState } = this.state;

    const gameName = `${turbo ? "Turbo " : ""}${type == types.taixiu ? "Big Small" : "Odd Even"}`;
    const type1 = type == types.taixiu ? gameMapping.taixiu[0] : gameMapping.chanle[0];
    const type2 = type == types.taixiu ? gameMapping.taixiu[1] : gameMapping.chanle[1];
    let winner = '';

    if (isHavingResult) {
      winner = this.getWiner();
    }

    return (
      <Container>
        <Row>
          <span className={styles.turbo}>
            <div>{gameName}</div>
          </span>
          <Col align="center" className={styles["flex-col"]}>
            <TotalBet type={type1} total={betList.firstBetType.total} isWin={gameMapping[type][0] == winner} />
            <TotalUser totalUser={firstBetTypeUsers} />
          </Col>
          <Col align="center" className={styles["shared-result"]}>
            <div>
              <TimeCounter remainingTime={time.remainingTime} gameState={currentState} key={currentState} />
              <RoundNum roundNumber={roundNumber} />
            </div>
            <DiceResult dices={result} />
          </Col>
          <Col align="center" className={styles["flex-col"]}>
            <TotalBet type={type2} total={betList.secondBetType.total} isWin={gameMapping[type][1] == winner} />
            <TotalUser totalUser={secondBetTypeUsers} />
          </Col>
        </Row>
      </Container>
    );
  };

  renderBetList() {
    const { type } = this.props;
    const { betList, isHavingResult } = this.state;
    let winner = '';

    if (isHavingResult) {
      winner = this.getWiner();
    }

    return (
      <Container>
        <Row className={styles["wrap-result"]}>
          <Col className="col-12 col-sm-6">
            <BetList
              dataList={betList.firstBetType.dataList}
              type="withtime"
              titles={this.getTitles()}
              isWin={gameMapping[type][0] == winner}
            ></BetList>
          </Col>
          <Col className="col-12 col-sm-6">
            <BetList
              dataList={betList.secondBetType.dataList}
              type="withtime"
              titles={this.getTitles()}
              isWin={gameMapping[type][1] == winner}
            ></BetList>
          </Col>
        </Row>
      </Container>
    );
  };

  getTitles() {
    return [
      'Time',
      'Nickname',
      'Stake'
    ]
  };

  render() {
    return (
      <div className={styles.bigsmallturbo}>
        <GameTemplate renderBetList={this.renderBetList()} renderMonitor={this.renderMonitor()} disabled={this.state.disabled} />
      </div>
    );
  }
}
