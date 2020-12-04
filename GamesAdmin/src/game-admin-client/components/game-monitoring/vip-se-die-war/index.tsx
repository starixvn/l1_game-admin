import React from "react";
import styles from "./vip-sedie-war.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Modal, Row } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import Dice from "../../shared/dice/dice";
import DiceModel from "../../common/model/DiceModel";
import DiceType from "../../common/model/DiceType";
import BetItem from "../../common/model/BetItem";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import VipSeDieWarModel, {
  VipSeDieWarResult,
} from "../../common/model/VipSeDieWarModel";
import { Notice } from "../../shared/notice/notice";
import { calendarFormat } from "moment";

type Props = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

export default class VipSeDieWar extends React.Component<
  Props,
  VipSeDieWarModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);
    this.isFirstLoad = true;
    this.state = new VipSeDieWarModel();
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 30) {
        clearTimeout(this.timeout);
        this.setState({
          time: { remainingTime: 0 },
          gameState: GameState.disabled,
          roundNumber: this.isFirstLoad
            ? this.state.roundNumber
            : this.state.roundNumber - 1,
          result: new VipSeDieWarModel().result,
        });

        this.isFirstLoad = false;
      }
    }

    if (!!data.ingame) {
      this.setState({
        betList: this.getUserBetList(data.ingame, this.state.betList),
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
      const waitingTime = 9;
      this.setState({
        result: this.getResult(data.finish.result),
        settlementResult: data.finish.settlementResult,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        this.setState({
          ...new VipSeDieWarModel(),
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 30,
          },
        });
      }, (waitingTime + 4) * 1000);
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
            new BetItem(data[user].name, betTracks[key])
          );
        }
      });
    }

    return currentData;
  }

  getResult(data) {
    const result = new VipSeDieWarResult();
    result.banker = [];
    result.player = [];

    Object.keys(data.player).forEach((key) => {
      result.player.push(new DiceModel(data.player[key] ? 1 : 0, DiceType.XocXoc));
    });

    Object.keys(data.banker).forEach((key) => {
      result.banker.push(new DiceModel(data.banker[key] ? 1 : 0, DiceType.XocXoc));
    });

    return result;
  }

  renderMonitor() {
    const {
      betList,
      roundNumber,
      time,
      result,
      gameState,
      settlementResult,
    } = this.state;

    return (
      <Container className={styles["sedie-container"]}>
        <Row>
          <Col align="center" className={styles["sedie-timmer"]}>
            <TimeCounter
              remainingTime={time.remainingTime}
              gameState={gameState}
            />
            <div>
              <div className={styles.title}>VIP Se Die War</div>
              <div className={styles.title}>Player</div>
              <Row className={styles.result}>
                <Dice
                  id="player-0"
                  diceModel={result.player[0]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="player-1"
                  diceModel={result.player[1]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="player-2"
                  diceModel={result.player[2]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="player-3"
                  diceModel={result.player[3]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
              </Row>
              <div className={styles.title}>Banker</div>
              <Row className={styles.result}>
                <Dice
                  id="banker-0"
                  diceModel={result.banker[0]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="banker-1"
                  diceModel={result.banker[1]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="banker-2"
                  diceModel={result.banker[2]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="banker-3"
                  diceModel={result.banker[3]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
              </Row>
              <RoundNum roundNumber={roundNumber} />
            </div>
          </Col>
          <Col align="center" className={styles["sedie-betList"]}>
            <TotalBet
              total={betList.even.total}
              type="even"
              isWin={settlementResult.even}
            ></TotalBet>
            <TotalBet
              total={betList.odd.total}
              type="odd"
              isWin={settlementResult.odd}
            ></TotalBet>
            <TotalBet
              total={betList.player.total}
              type="player"
              isWin={settlementResult.player}
            />
            <TotalBet
              total={betList.banker.total}
              type="banker"
              isWin={settlementResult.banker}
            />
            <TotalBet
              total={betList.tie.total}
              type="tie"
              isWin={settlementResult.tie}
            />
            <TotalBet
              total={betList.playerOdd.total}
              type="playerOdd"
              isWin={settlementResult.playerOdd}
            />
            <TotalBet
              total={betList.playerEven.total}
              type="playerEven"
              isWin={settlementResult.playerEven}
            />
            <TotalBet
              total={betList.bankerOdd.total}
              type="bankerOdd"
              isWin={settlementResult.bankerOdd}
            />
            <TotalBet
              total={betList.bankerEven.total}
              type="bankerEven"
              isWin={settlementResult.bankerEven}
            />
            <TotalBet
              total={betList.playerRed4.total}
              type="playerRed4"
              isWin={settlementResult.playerRed4}
            />
            <TotalBet
              total={betList.playerRed3.total}
              type="playerRed3"
              isWin={settlementResult.playerRed3}
            />
            <TotalBet
              total={betList.playerWhite3.total}
              type="playerWhite3"
              isWin={settlementResult.playerWhite3}
            />
            <TotalBet
              total={betList.playerWhite4.total}
              type="playerWhite4"
              isWin={settlementResult.playerWhite4}
            />
            <TotalBet
              total={betList.bankerRed4.total}
              type="bankerRed4"
              isWin={settlementResult.bankerRed4}
            />
            <TotalBet
              total={betList.bankerRed3.total}
              type="bankerRed3"
              isWin={settlementResult.bankerRed3}
            />
            <TotalBet
              total={betList.bankerWhite3.total}
              type="bankerWhite3"
              isWin={settlementResult.bankerWhite3}
            />
            <TotalBet
              total={betList.bankerWhite4.total}
              type="bankerWhite4"
              isWin={settlementResult.bankerWhite4}
            />
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList, settlementResult } = this.state;

    return (
      <React.Fragment>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Even</div>]}
            type="simple"
            dataList={betList.even.dataList}
            isWin={settlementResult.even}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Odd</div>]}
            type="simple"
            dataList={betList.odd.dataList}
            isWin={settlementResult.odd}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Player</div>]}
            type="simple"
            dataList={betList.player.dataList}
            isWin={settlementResult.player}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Banker</div>]}
            type="simple"
            dataList={betList.banker.dataList}
            isWin={settlementResult.banker}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Tie</div>]}
            type="simple"
            dataList={betList.tie.dataList}
            isWin={settlementResult.tie}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Player Odd</div>]}
            type="simple"
            dataList={betList.playerOdd.dataList}
            isWin={settlementResult.playerOdd}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Player Even</div>]}
            type="simple"
            dataList={betList.playerEven.dataList}
            isWin={settlementResult.playerEven}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Banker Odd</div>]}
            type="simple"
            dataList={betList.bankerOdd.dataList}
            isWin={settlementResult.bankerOdd}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Banker Even</div>]}
            type="simple"
            dataList={betList.bankerEven.dataList}
            isWin={settlementResult.bankerEven}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Player - 4 Red</div>]}
            type="simple"
            dataList={betList.playerRed4.dataList}
            isWin={settlementResult.playerRed4}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Player - 3 Red</div>]}
            type="simple"
            dataList={betList.playerRed3.dataList}
            isWin={settlementResult.playerRed3}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Player - 1 Red</div>]}
            type="simple"
            dataList={betList.playerWhite3.dataList}
            isWin={settlementResult.playerWhite3}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Player - 4 White</div>]}
            type="simple"
            dataList={betList.playerWhite4.dataList}
            isWin={settlementResult.playerWhite4}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Banker - 4 Red</div>]}
            type="simple"
            dataList={betList.bankerRed4.dataList}
            isWin={settlementResult.bankerRed4}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Banker - 3 Red</div>]}
            type="simple"
            dataList={betList.bankerRed3.dataList}
            isWin={settlementResult.bankerRed3}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Banker - 1 Red</div>]}
            type="simple"
            dataList={betList.bankerWhite3.dataList}
            isWin={settlementResult.bankerWhite3}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            titles={[<div className="text-center">Banker - 4 White</div>]}
            type="simple"
            dataList={betList.bankerWhite4.dataList}
            isWin={settlementResult.bankerWhite4}
          />
        </Col>
      </React.Fragment>
    );
  }

  handleChangeResult(id, value) {
    const { selectedDice, result } = this.state;
    const splits = selectedDice.split("-");
    const side = splits[0];
    const position = parseInt(splits[1]);
    result[side][position] = new DiceModel(value, DiceType.XocXoc);
    const newResult = { player: {}, banker: {} };

    result.player.forEach(
      (dice, index) => (newResult.player[`red${index + 1}`] = dice.value === 1)
    );
    result.banker.forEach(
      (dice, index) => (newResult.banker[`red${index + 1}`] = dice.value === 1)
    );

    this.props.sendMessage({
      set_result: {
        roundId: this.state.roundNumber,
        result: newResult,
      },
    });

    this.setState({
      result,
      isOpenModal: false,
    });
  }

  openModal(id) {
    this.setState({
      isOpenModal: true,
      selectedDice: id,
    });
  }
  toggleModal() {
    this.setState((prevState) => ({
      isOpenModal: !prevState.isOpenModal,
    }));
  }

  renderGameControl() {
    const { isOpenModal } = this.state;

    return (
      <Modal
        isOpen={isOpenModal}
        toggle={this.toggleModal}
        className={styles["game-control-panel"]}
      >
        <Row>
          <Dice
            diceModel={new DiceModel(1, DiceType.XocXoc)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(0, DiceType.XocXoc)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
        </Row>
      </Modal>
    );
  }

  render() {
    return (
      <div>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate
          renderBetList={this.renderBetList()}
          renderMonitor={this.renderMonitor()}
          disabled={this.state.gameState === GameState.disabled}
        ></GameTemplate>
        {this.state.gameState == GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
