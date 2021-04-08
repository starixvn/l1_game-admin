import React from "react";
import styles from "./fish-prawn-crab.module.scss";
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
  FishPrawnCrabModel,
  GameResult,
  FishPrawnCrabBetList,
} from "../../common/model/FishPrawnCrabModel";
import { Notice } from "../../shared/notice/notice";

type FishPrawnCrabProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

const CHOICES = ["stag", "gourd", "rooster", "fish", "crab", "prawn"];

enum FishPrawnCrabDice {
  stag = 0,
  gourd,
  rooster,
  fish,
  crab,
  prawn,
}

export default class FishPrawnCrab extends React.Component<
  FishPrawnCrabProps,
  FishPrawnCrabModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new FishPrawnCrabModel();
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
        const model = new FishPrawnCrabModel();
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

    if (!!data.inGame) {
      this.setState({
        betList: this.getUserBetList(data.inGame),
      });
    }

    if (!!data.finish) {
      this.isFirstLoad = false;
      const { result, settlementResult } = data.finish;
      this.setState({
        result: this.getResult(result),
        winners: this.getWinners(settlementResult),
        time: {
          remainingTime: 10,
        },
        gameState: GameState.waiting,
      });

      setTimeout(() => {
        const model = new FishPrawnCrabModel();
        this.setState({
          result: model.result,
          winners: model.winners,
          nextRoundResult: new GameResult(),
          isHandleNextRoundResult: false,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 30,
          },
          gameState: GameState.running,
        });
      }, 10500);
    }

    if (!!data.remainingTime) {
      if (data.remainingTime > 31) {
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          winners: data.result ? data.settlementResult : this.state.winners,
          result: data.result ? this.getResult(data.result) : this.state.result,
          time: {
            remainingTime: data.remainingTime - 31,
          },
        });

        this.timeout = setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            time: {
              remainingTime: 30,
            },
            roundNumber: data.phien,
            winners: [false, false, false, false, false, false],
            result: [
              new DiceModel(-1, DiceType.BauCua),
              new DiceModel(-1, DiceType.BauCua),
              new DiceModel(-1, DiceType.BauCua),
            ],
          });
        }, (data.time_remain - 30) * 1000);
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

  getUserBetList(data) {
    const betInfo = new FishPrawnCrabBetList();

    Object.keys(data).forEach(function (userName) {
      const { name, betTracks } = data[userName];

      CHOICES.forEach((choice) => {
        if (betTracks[choice] > 0) {
          betInfo[choice].dataList.unshift(
            new BetItem(name, betTracks[choice])
          );

          betInfo[choice].total += betTracks[choice];
        }
      });
    });

    return betInfo;
  }

  getResult(result) {
    const dices = new Array<DiceModel>();

    dices.push(new DiceModel(result.dice1, DiceType.BauCua));
    dices.push(new DiceModel(result.dice2, DiceType.BauCua));
    dices.push(new DiceModel(result.dice3, DiceType.BauCua));

    return dices;
  }

  getWinners(settlementResult) {
    const winners = [];

    CHOICES.forEach((choice) => {
      winners.push(settlementResult[choice]);
    });

    return winners;
  }

  handleChangeResult(id, value) {
    const { currentDiceNumber } = this.state;
    const gameResult = this.state.nextRoundResult;
    const newResult = this.state.result;

    gameResult[`dice${+currentDiceNumber + 1}`] = value;

    newResult[currentDiceNumber] = new DiceModel(value, DiceType.BauCua);
    this.setState({
      result: newResult,
      isOpenModal: false,
    });

    if (this.state.gameState == GameState.waiting) {
      this.setState({
        isHandleNextRoundResult: true,
        nextRoundResult: gameResult,
      });
    }

    this.props.sendMessage({
      set_result: {
        result: gameResult,
        roundId:
          this.state.gameState == GameState.running
            ? this.state.roundNumber
            : this.state.roundNumber + 1,
      },
    });
  }

  openModal(id) {
    this.setState({
      isOpenModal: true,
      currentDiceNumber: id.split("-")[1],
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
          <Col align="center" className="col-12 col-sm-6">
            <Row className={styles["align-middle"]}>
              <Col className={styles.timecounter}>
                <div className={styles.title}>Fish Prawn Crab</div>
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
          <Col align="center" className={styles.timecounter}>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.stag.total}
                type="nai"
                isWin={winners[FishPrawnCrabDice.stag]}
              ></TotalBet>
              <TotalBet
                total={betList.gourd.total}
                type="bau"
                isWin={winners[FishPrawnCrabDice.gourd]}
              ></TotalBet>
              <TotalBet
                total={betList.rooster.total}
                type="ga"
                isWin={winners[FishPrawnCrabDice.rooster]}
              ></TotalBet>
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.fish.total}
                type="ca"
                isWin={winners[FishPrawnCrabDice.fish]}
              ></TotalBet>
              <TotalBet
                total={betList.crab.total}
                type="cua"
                isWin={winners[FishPrawnCrabDice.crab]}
              ></TotalBet>
              <TotalBet
                total={betList.prawn.total}
                type="tom"
                isWin={winners[FishPrawnCrabDice.prawn]}
              ></TotalBet>
            </Row>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList, winners } = this.state;

    return (
      <React.Fragment>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.stag.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.stag)]}
            isWin={winners[FishPrawnCrabDice.stag]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.gourd.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.gourd)]}
            isWin={winners[FishPrawnCrabDice.gourd]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.rooster.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.rooster)]}
            isWin={winners[FishPrawnCrabDice.rooster]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.fish.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.fish)]}
            isWin={winners[FishPrawnCrabDice.fish]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.crab.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.crab)]}
            isWin={winners[FishPrawnCrabDice.crab]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.prawn.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.prawn)]}
            isWin={winners[FishPrawnCrabDice.prawn]}
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
