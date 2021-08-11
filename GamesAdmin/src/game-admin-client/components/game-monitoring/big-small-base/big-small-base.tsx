import React from "react";
import styles from "./big-small-base.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import DiceResult from "../../shared/dice-result/dice-result";
import TotalBet from "../../shared/total-bet/total-bet";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import { Container, Row, Col, Modal } from "reactstrap";
import TotalUser from "../../shared/total-user/total-user";
import RoundNum from "../../shared/round-num/round-num";
import DiceModel from "../../common/model/DiceModel";
import BigSmallBaseModel from "../../common/model/BigSmallBaseModel";
import DiceType from "../../common/model/DiceType";
import BetItem, { SystemId } from "../../common/model/BetItem";
import { types, gameMapping } from "../game-monitoring-config";
import { GameState } from "../../shared/common/GameState";
import { getTotalBetOnSystem } from "../../util/util";
import Dice from "../../shared/dice/dice";

type BigSmallProps = {
  turbo: boolean;
  type: string;
  onMessage: Function;
  enableFilter?: boolean;
  sendMessage: Function;
  isProduction: boolean;
};

const optionChoice = ["big", "even"];
const DiceValue = [1, 2, 3, 4, 5, 6];

const betListMapping = (flag: boolean) => {
  const betListLabels = ["firstBetType", "secondBetType"];
  const choice = flag ? 0 : 1;

  return betListLabels[choice];
};

export default class BigSmallBase extends React.Component<
  BigSmallProps,
  BigSmallBaseModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;

  constructor(props) {
    super(props);

    this.state = new BigSmallBaseModel();
    this.isFirstLoad = true;
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    const { turbo } = this.props;
    const totalRemainingTime = turbo ? 30 : 50;
    const totalWaitingTime = turbo ? 5 : 10;

    if (!!data.inGame) {
      this.setState({
        betList: this.getUserBetList(data.inGame, this.state.betList),
      });
    }

    if (!!data.finish) {
      this.isFirstLoad = false;
      setTimeout(() => {
        this.setState({
          result: new BigSmallBaseModel().result,
          isHavingResult: false,
          currentState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: totalRemainingTime,
          },
        });
      }, (totalWaitingTime + 1) * 1000);

      this.setState({
        result: this.getResult(data.finish.result),
        isHavingResult: true,
        currentState: GameState.waiting,
        time: {
          remainingTime: totalWaitingTime,
        },
      });
    }

    if (!!data.um) {
      if (
        this.isFirstLoad ||
        this.state.time.remainingTime == totalRemainingTime
      ) {
        clearTimeout(this.timeout);
        this.setState({
          time: { remainingTime: 0 },
          result: new BigSmallBaseModel().result,
          isHavingResult: false,
          currentState: GameState.disabled,
        });
        this.isFirstLoad = false;
      }
    }

    if (!!data.remainingTime) {
      const { remainingTime, roundId, result } = data;
      if (
        (this.props.turbo && remainingTime > totalRemainingTime + 6) ||
        remainingTime > totalRemainingTime
      ) {
        this.setState({
          time: {
            remainingTime: remainingTime - totalRemainingTime - 1,
          },
          result: result ? this.getResult(result) : this.state.result,
          isHavingResult: result ? true : this.state.isHavingResult,
          roundNumber: roundId - 1,
          currentState: GameState.waiting,
        });

        this.timeout = setTimeout(() => {
          this.setState({
            time: {
              remainingTime: totalRemainingTime,
            },
            roundNumber: roundId,
            currentState: GameState.running,
            isHavingResult: false,
            result: new BigSmallBaseModel().result,
          });
        }, (remainingTime - totalRemainingTime - 1) * 1000);
      } else {
        this.setState({
          currentState: GameState.running,
          time: {
            remainingTime: remainingTime - 1,
          },
          roundNumber: roundId,
          isHavingResult: false,
          result: new BigSmallBaseModel().result,
        });
      }
    }
  }

  getUserBetList(data, currentData) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      const currency = data[user].currency;

      let betTracks = data[user].betTracks;

      Object.keys(betTracks).forEach((key) => {
        if (betTracks[key]) {
          let select = false;
          if (optionChoice.indexOf(key) !== -1) {
            select = true;
          }

          if (!currency || currency.toUpperCase() !== "UUS") {
            currentData[betListMapping(select)].total += betTracks[key];
          }

          currentData[betListMapping(select)].dataList.unshift(
            new BetItem(
              data[user].name,
              betTracks[key],
              null,
              null,
              data[user].system,
              currency
            )
          );
        }
      });
    }

    return currentData;
  }

  getResult(result) {
    const dices = new Array<DiceModel>();

    dices.push(new DiceModel(result.dice1, DiceType.TaiXiu));
    dices.push(new DiceModel(result.dice2, DiceType.TaiXiu));
    dices.push(new DiceModel(result.dice3, DiceType.TaiXiu));

    return dices;
  }

  getWiner() {
    const { type } = this.props;
    const { result } = this.state;

    const sumDicesValue = result.reduce((sum, dice) => sum + dice.value, 0);
    const winner =
      type == types.taixiu
        ? sumDicesValue >= 11
          ? "tai"
          : "xiu"
        : sumDicesValue % 2 == 1
        ? "le"
        : "chan";

    return winner;
  }

  toggleModal() {
    this.setState({
      isOpenControlResultModal: !this.state.isOpenControlResultModal,
    });
  }

  openModal(id) {
    this.setState({
      isOpenControlResultModal: true,
      currentDiceNumber: +id.split("-")[1],
    });
  }

  handleChangeResult(id, value) {
    const { currentDiceNumber, result } = this.state;
    const gameResult = this.state.nextRoundResult;

    const newResult = result.map((diceInfo, index) => {
      if (index === currentDiceNumber || diceInfo.value === -1) {
        diceInfo.value = value;
        gameResult[`dice${index + 1}`] = value;
      }

      return diceInfo;
    });

    this.setState({
      result: newResult,
      isOpenControlResultModal: false,
    });

    if (this.state.currentState == GameState.waiting) {
      this.setState({
        isHandleNextRoundResult: true,
        nextRoundResult: gameResult,
      });
    }

    this.props.sendMessage({
      set_result: {
        result: gameResult,
        roundId:
          this.state.currentState == GameState.running
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
          {DiceValue.map((diceValue) => (
            <Dice
              key={`dice-select-${diceValue}`}
              diceModel={new DiceModel(diceValue, DiceType.TaiXiu)}
              isSmall={false}
              onClickHandle={(id: string, value: number) =>
                this.handleChangeResult(id, value)
              }
            />
          ))}
        </Row>
      </Modal>
    );
  }

  renderMonitor(systemFilter) {
    const { turbo, type } = this.props;
    const {
      betList,
      time,
      roundNumber,
      result,
      firstBetTypeUsers,
      secondBetTypeUsers,
      isHavingResult,
      currentState,
    } = this.state;

    const gameName = `${turbo ? "Turbo " : ""}${
      type == types.taixiu ? "Big Small" : "Odd Even"
    }`;
    const type1 =
      type == types.taixiu ? gameMapping.taixiu[0] : gameMapping.chanle[0];
    const type2 =
      type == types.taixiu ? gameMapping.taixiu[1] : gameMapping.chanle[1];
    let winner = "";

    if (isHavingResult) {
      winner = this.getWiner();
    }

    if (systemFilter) {
      betList.firstBetType.total = getTotalBetOnSystem(
        betList.firstBetType.dataList,
        systemFilter,
        true
      );

      betList.secondBetType.total = getTotalBetOnSystem(
        betList.secondBetType.dataList,
        systemFilter,
        true
      );
    }

    return (
      <Container>
        <Row>
          <span className={styles["turbo"]}>
            <div>{gameName}</div>
          </span>
          <Col align="center" className={styles["flex-col"]}>
            <TotalBet
              type={type1}
              total={betList.firstBetType.total}
              isWin={gameMapping[type][0] == winner}
            />
            <TotalUser totalUser={firstBetTypeUsers} />
          </Col>
          <Col align="center" className={styles["shared-result"]}>
            <div>
              <TimeCounter
                remainingTime={time.remainingTime}
                gameState={currentState}
                key={currentState}
              />
              <RoundNum roundNumber={roundNumber} />
            </div>
            <DiceResult
              dices={result}
              diceClickHandle={(id: string) => this.openModal(id)}
            />
          </Col>
          <Col align="center" className={styles["flex-col"]}>
            <TotalBet
              type={type2}
              total={betList.secondBetType.total}
              isWin={gameMapping[type][1] == winner}
            />
            <TotalUser totalUser={secondBetTypeUsers} />
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList(systemFilter) {
    const { type } = this.props;
    const { betList, isHavingResult } = this.state;
    let winner = "";

    if (isHavingResult) {
      winner = this.getWiner();
    }

    return (
      <Container>
        <Row className={styles["wrap-result"]}>
          <Col className="col-12 col-sm-6">
            <BetList
              dataList={betList.firstBetType.dataList}
              type="simple"
              titles={this.getTitles()}
              isWin={gameMapping[type][0] == winner}
              systemFilter={systemFilter}
            ></BetList>
          </Col>
          <Col className="col-12 col-sm-6">
            <BetList
              dataList={betList.secondBetType.dataList}
              type="simple"
              titles={this.getTitles()}
              isWin={gameMapping[type][1] == winner}
              systemFilter={systemFilter}
            ></BetList>
          </Col>
        </Row>
      </Container>
    );
  }

  getTitles() {
    return ["Nickname", "Stake"];
  }

  render() {
    const gameTemplateProps = {
      renderBetList: this.renderBetList.bind(this),
      renderMonitor: this.renderMonitor.bind(this),
      disabled: this.state.currentState == GameState.disabled,
      enableFilter: this.props.enableFilter,
    };

    return (
      <div className={styles["bigsmallturbo"]}>
        {this.props.isProduction && this.renderGameControl()}
        <GameTemplate {...gameTemplateProps} />
      </div>
    );
  }
}
