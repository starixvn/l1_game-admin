import React from "react";
import styles from "./styles.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import DiceResult from "../../shared/dice-result/dice-result";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Row, Modal } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import Dice from "../../shared/dice/dice";
import BetItem from "../../common/model/BetItem";
import RoundNum from "../../shared/round-num/round-num";
import DiceType from "../../common/model/DiceType";
import DiceModel from "../../common/model/DiceModel";
import { GameState } from "../../shared/common/GameState";
import {
  VipSicBoBetList,
  VipSicBoModel,
} from "../../common/model/VipSicBoModel";
import { Notice } from "../../shared/notice/notice";

type VipSicBoProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

export default class VipSicBo extends React.Component<
  VipSicBoProps,
  VipSicBoModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new VipSicBoModel();
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
        const model = new VipSicBoModel();
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
        const newState = new VipSicBoModel();

        this.setState({
          ...newState,
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 30,
          },
        });
      }, (waitingTime + 4) * 1000);
    }

    if (!!data.remainingTime) {
      if (data.remainingTime > 31) {
        this.setState(prevState => ({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result: data.result
            ? this.getResult(data.result)
            : prevState.result,
          time: {
            remainingTime: data.remainingTime - 31,
          },
        }));

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
  }

  getResult(diceResult) {
    const result = new Array<DiceModel>();

    result.push(new DiceModel(diceResult.dice1, DiceType.TaiXiu));
    result.push(new DiceModel(diceResult.dice2, DiceType.TaiXiu));
    result.push(new DiceModel(diceResult.dice3, DiceType.TaiXiu));

    return result;
  }

  getUserBetList(data, currentData: VipSicBoBetList) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      const { betTracks } = data[user];
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

  handleChangeResult(id, value) {
    const { currentDiceNumber } = this.state;

    this.props.sendMessage({
      set_result: {
        roundId: this.state.roundNumber,
        result: {
          [`dice${currentDiceNumber + 1}`]: value,
        },
      },
    });

    const newResult = this.state.result;
    newResult[currentDiceNumber] = new DiceModel(value, DiceType.TaiXiu);
    this.setState({
      result: newResult,
      isOpenModal: false,
    });

    if (this.state.gameState == GameState.waiting) {
      const gameResult = this.state.nextRoudResult;
      gameResult[currentDiceNumber] = new DiceModel(value, DiceType.TaiXiu);

      this.setState({
        isHandleNextRoundResult: true,
        nextRoudResult: gameResult,
      });
    }
  }

  openModal(id) {
    this.setState({
      isOpenModal: true,
      currentDiceNumber: parseInt(id.split("-")[1]),
    });
  }

  toggleModal() {
    this.setState({
      isOpenModal: !this.state.isOpenModal,
    });
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
      <Container>
        <Row className={styles["align-middle"]}>
          <Col align="center" className="col-12 col-md-4">
            <Row className={styles["align-middle"]}>
              <Col className={styles.timecounter}>
                <div className={styles.title}>VIP Sic Bo</div>
                <TimeCounter
                  remainingTime={time.remainingTime}
                  gameState={gameState}
                ></TimeCounter>
                <RoundNum roundNumber={roundNumber} />
              </Col>
              <Col className={styles.result}>
                <DiceResult dices={result} diceClickHandle={this.openModal} />
              </Col>
            </Row>
          </Col>
          <Col align="center" className="col-4">
            <Row className="justify-content-center">
              <TotalBet
                total={betList["1point"].total}
                type="1point"
                isWin={settlementResult["1point"]}
              ></TotalBet>
              <TotalBet
                total={betList["2points"].total}
                type="2points"
                isWin={settlementResult["2points"]}
              ></TotalBet>
              <TotalBet
                total={betList["3points"].total}
                type="3points"
                isWin={settlementResult["3points"]}
              ></TotalBet>
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList["4points"].total}
                type="4points"
                isWin={settlementResult["4points"]}
              ></TotalBet>
              <TotalBet
                total={betList["5points"].total}
                type="5points"
                isWin={settlementResult["5points"]}
              ></TotalBet>
              <TotalBet
                total={betList["6points"].total}
                type="6points"
                isWin={settlementResult["6points"]}
              ></TotalBet>
            </Row>
          </Col>
          <Col align="center" className="col-2">
            <Row className="justify-content-center">
              <TotalBet
                total={betList.big.total}
                type="vipsicbobig"
                isWin={settlementResult.big}
              />
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.small.total}
                type="vipsicbosmall"
                isWin={settlementResult.small}
              />
            </Row>
          </Col>
          <Col align="center" className="col-2">
            <TotalBet
              total={betList.odd.total}
              type="vipsicboodd"
              isWin={settlementResult.odd}
            ></TotalBet>
            <TotalBet
              total={betList.even.total}
              type="vipsicboeven"
              isWin={settlementResult.even}
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
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList["1point"].dataList}
            titles={[<div className="text-center">1 Point</div>]}
            isWin={settlementResult["1point"]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList["2points"].dataList}
            titles={[<div className="text-center">2 Points</div>]}
            isWin={settlementResult["2points"]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList["3points"].dataList}
            titles={[<div className="text-center">3 Points</div>]}
            isWin={settlementResult["3points"]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList["4points"].dataList}
            titles={[<div className="text-center">4 Points</div>]}
            isWin={settlementResult["4points"]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList["5points"].dataList}
            titles={[<div className="text-center">5 Points</div>]}
            isWin={settlementResult["5points"]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList["6points"].dataList}
            titles={[<div className="text-center">6 Points</div>]}
            isWin={settlementResult["6points"]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.big.dataList}
            titles={[<div className="text-center">Big</div>]}
            isWin={settlementResult.big}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.small.dataList}
            titles={[<div className="text-center">Small</div>]}
            isWin={settlementResult.small}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.odd.dataList}
            titles={[<div className="text-center">Odd</div>]}
            isWin={settlementResult.odd}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.even.dataList}
            titles={[<div className="text-center">Even</div>]}
            isWin={settlementResult.even}
          />
        </Col>
      </React.Fragment>
    );
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
            diceModel={new DiceModel(1, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(2, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(3, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(4, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(5, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(6, DiceType.TaiXiu)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
        </Row>
      </Modal>
    );
  }

  render() {
    return (
      <div className={styles["vip-sic-bo"]}>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate
          renderBetList={this.renderBetList()}
          renderMonitor={this.renderMonitor()}
          disabled={this.state.gameState == GameState.disabled}
        />
        {this.state.gameState == GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
