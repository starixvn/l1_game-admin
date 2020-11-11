import React from "react";
import styles from "./vip-fish-prawn-crab.module.scss";
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
  VipFishPrawnCrabBetList,
  VipFishPrawnCrabModel,
} from "../../common/model/VipFishPrawnCrabModel";
import { Notice } from "../../shared/notice/notice";

type VipFishPrawnCrabProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

enum FishPrawnCrabDice {
  nai = 0,
  bau,
  ga,
  ca,
  cua,
  tom,
}

export default class VipFishPrawnCrab extends React.Component<
  VipFishPrawnCrabProps,
  VipFishPrawnCrabModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new VipFishPrawnCrabModel();
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
        const model = new VipFishPrawnCrabModel();
        this.setState({
          time: { remainingTime: 0 },
          gameState: GameState.disabled,
          roundNumber: this.isFirstLoad
            ? this.state.roundNumber
            : this.state.roundNumber - 1,
          result: model.result,
          winners: model.winners,
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
      const waitingTime = 8;
      const result = this.getResult(data.finish.result);
      this.setState({
        result,
        winners: this.getWinners(result),
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        const newState = new VipFishPrawnCrabModel();

        this.setState({
          ...newState,
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 31,
          },
        });
      }, (waitingTime + 1) * 1000);
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
  }

  getUserBetList(data, currentData: VipFishPrawnCrabBetList) {
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
            new BetItem(data[user].name, betTracks[key])
          );
        }
      });
    }

    return currentData;
  }

  getResult(dices) {
    const result = new Array<DiceModel>();

    result.push(new DiceModel(dices.dice1, DiceType.BauCua));
    result.push(new DiceModel(dices.dice2, DiceType.BauCua));
    result.push(new DiceModel(dices.dice3, DiceType.BauCua));

    return result;
  }

  getWinners(dices: DiceModel[]) {
    const winners = [false, false, false, false, false, false, false];
    winners[dices[0].value] = true;
    winners[dices[1].value] = true;
    winners[dices[2].value] = true;
    winners[6] = dices.every((dice) => dice.value === dices[0].value);
    return winners;
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
    newResult[currentDiceNumber] = new DiceModel(value, DiceType.BauCua);
    this.setState({
      result: newResult,
      isOpenModal: false,
    });

    if (this.state.gameState == GameState.waiting) {
      const gameResult = this.state.nextRoudResult;
      gameResult[`dice-${currentDiceNumber}`] = value;

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
      winners,
      gameState,
    } = this.state;

    return (
      <Container>
        <Row className={styles["align-middle"]}>
          <Col align="center" className="col-12 col-md-4">
            <Row className={styles["align-middle"]}>
              <Col className={styles.timecounter}>
                <div className={styles.title}>VIP Fish Prawn Crab</div>
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
                total={betList.stag.total}
                type="nai"
                isWin={winners[FishPrawnCrabDice.nai]}
              ></TotalBet>
              <TotalBet
                total={betList.gourd.total}
                type="bau"
                isWin={winners[FishPrawnCrabDice.bau]}
              ></TotalBet>
              <TotalBet
                total={betList.rooster.total}
                type="ga"
                isWin={winners[FishPrawnCrabDice.ga]}
              ></TotalBet>
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.fish.total}
                type="ca"
                isWin={winners[FishPrawnCrabDice.ca]}
              ></TotalBet>
              <TotalBet
                total={betList.crab.total}
                type="cua"
                isWin={winners[FishPrawnCrabDice.cua]}
              ></TotalBet>
              <TotalBet
                total={betList.prawn.total}
                type="tom"
                isWin={winners[FishPrawnCrabDice.tom]}
              ></TotalBet>
            </Row>
          </Col>
          <Col align="center" className="col-2">
            <Row className="justify-content-center">
              <TotalBet
                total={betList.mountain.total}
                type="mountain"
                isWin={
                  winners[FishPrawnCrabDice.nai] ||
                  winners[FishPrawnCrabDice.bau] ||
                  winners[FishPrawnCrabDice.ga]
                }
              />
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.ocean.total}
                type="ocean"
                isWin={
                  winners[FishPrawnCrabDice.ca] ||
                  winners[FishPrawnCrabDice.cua] ||
                  winners[FishPrawnCrabDice.tom]
                }
              />
            </Row>
          </Col>
          <Col align="center" className="col-2">
            <TotalBet
              total={betList.anyTriple.total}
              type="anyTriple"
              isWin={winners[6]}
            ></TotalBet>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList, winners } = this.state;

    return (
      <React.Fragment>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.stag.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.nai)]}
            isWin={winners[FishPrawnCrabDice.nai]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.gourd.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.bau)]}
            isWin={winners[FishPrawnCrabDice.bau]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.rooster.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.ga)]}
            isWin={winners[FishPrawnCrabDice.ga]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.fish.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.ca)]}
            isWin={winners[FishPrawnCrabDice.ca]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.crab.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.cua)]}
            isWin={winners[FishPrawnCrabDice.cua]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.prawn.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.tom)]}
            isWin={winners[FishPrawnCrabDice.tom]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.anyTriple.dataList}
            titles={[<div className="text-center">Any Triple</div>]}
            isWin={winners[6]}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.mountain.dataList}
            titles={[<div className="text-center">Mountain</div>]}
            isWin={
              winners[FishPrawnCrabDice.nai] ||
              winners[FishPrawnCrabDice.bau] ||
              winners[FishPrawnCrabDice.ga]
            }
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.ocean.dataList}
            titles={[<div className="text-center">Ocean</div>]}
            isWin={
              winners[FishPrawnCrabDice.ca] ||
              winners[FishPrawnCrabDice.cua] ||
              winners[FishPrawnCrabDice.tom]
            }
          />
        </Col>
      </React.Fragment>
    );
  }

  renderBetListBau(diceValue) {
    const dice = new DiceModel(diceValue, DiceType.BauCua);

    return <Dice diceModel={dice} isSmall={true}></Dice>;
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
            diceModel={new DiceModel(0, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(1, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(2, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(3, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(4, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(5, DiceType.BauCua)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
        </Row>
      </Modal>
    );
  }

  render() {
    return (
      <div className={styles["fish-prawn-crab"]}>
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
