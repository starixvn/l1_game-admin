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
import BigSmallBaseModel, {
  BigSmallBaseBetList,
} from "../../common/model/BigSmallBaseModel";
import DiceType from "../../common/model/DiceType";
import BetItem from "../../common/model/BetItem";
import { types, gameMapping } from "../game-monitoring-config";
import { GameState } from "../../shared/common/GameState";
import Dice from "../../shared/dice/dice";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";

const optionChoice = ["big", "even"];
const DiceValue = [1, 2, 3, 4, 5, 6];

const betListMapping = (flag: boolean) => {
  const betListLabels = ["firstBetType", "secondBetType"];
  const choice = flag ? 0 : 1;

  return betListLabels[choice];
};

export default class BigSmallBase extends BaseMonitoring<BigSmallBaseModel> {
  constructor(props) {
    super(props);

    this.state = {
      ...new BigSmallBaseModel(),
      result: this.defaultResult,
      betList: this.defaultBetList,
    };
  }

  get isExcludeUUS(): boolean {
    return true;
  }

  get type() {
    return "";
  }

  get turbo() {
    return false;
  }

  get defaultResult(): any {
    return [
      new DiceModel(-1, DiceType.TaiXiu),
      new DiceModel(-1, DiceType.TaiXiu),
      new DiceModel(-1, DiceType.TaiXiu),
    ];
  }

  get defaultBetList(): {} {
    return new BigSmallBaseBetList();
  }

  getUserBetList(data, currentData) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      const currency = data[user].currency;
      const userRate = this.currencyExchangeRate[data[user].currency];

      let betTracks = data[user].betTracks;

      Object.keys(betTracks).forEach((key) => {
        if (betTracks[key]) {
          const baseBetAmount = parseFloat(
            (betTracks[key] * userRate).toFixed(2)
          );
          let select = false;
          if (optionChoice.indexOf(key) !== -1) {
            select = true;
          }

          if (!currency || currency.toUpperCase() !== "UUS") {
            currentData[betListMapping(select)].total += baseBetAmount;
          }

          currentData[betListMapping(select)].dataList.unshift(
            new BetItem(
              data[user].name,
              baseBetAmount,
              null,
              null,
              data[user].system,
              data[user].currency,
              data[user].partnerId
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
    const { result } = this.state;

    const sumDicesValue = result.reduce((sum, dice) => sum + dice.value, 0);
    const winner =
      this.type == types.taixiu
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

  renderMonitor() {
    const {
      betList,
      time,
      roundNumber,
      result,
      firstBetTypeUsers,
      secondBetTypeUsers,
      isHavingResult,
      gameState,
    } = this.state;

    const gameName = `${this.turbo ? "Turbo " : ""}${
      this.type == types.taixiu ? "Big Small" : "Odd Even"
    }`;
    const type1 =
      this.type == types.taixiu ? gameMapping.taixiu[0] : gameMapping.chanle[0];
    const type2 =
      this.type == types.taixiu ? gameMapping.taixiu[1] : gameMapping.chanle[1];
    let winner = "";

    if (isHavingResult) {
      winner = this.getWiner();
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
              isWin={gameMapping[this.type][0] == winner}
            />
            <TotalUser totalUser={firstBetTypeUsers} />
          </Col>
          <Col align="center" className={styles["shared-result"]}>
            <div>
              <TimeCounter
                remainingTime={time.remainingTime}
                gameState={gameState}
                key={gameState}
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
              isWin={gameMapping[this.type][1] == winner}
            />
            <TotalUser totalUser={secondBetTypeUsers} />
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
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
              isWin={gameMapping[this.type][0] == winner}
            ></BetList>
          </Col>
          <Col className="col-12 col-sm-6">
            <BetList
              dataList={betList.secondBetType.dataList}
              type="simple"
              titles={this.getTitles()}
              isWin={gameMapping[this.type][1] == winner}
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
      renderBetList: this.renderBetList,
      renderMonitor: this.renderMonitor,
      onChangePartnerFilter: this.onChangePartnerFilter,
      onChangeSystemFilter: this.onChangeSystemFilter,
      enableFilter: true,
      disabled: this.state.currentState == GameState.disabled,
      partners: this.props.partners,
    };

    return (
      <div className={styles["bigsmallturbo"]}>
        {this.props.isProduction && this.renderGameControl()}
        <GameTemplate {...gameTemplateProps} />
      </div>
    );
  }
}
