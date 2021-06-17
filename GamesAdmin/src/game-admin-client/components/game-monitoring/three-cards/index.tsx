import React from "react";
import styles from "./styles.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Row, Modal, ModalHeader, ModalBody } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import BetItem, { SystemId } from "../../common/model/BetItem";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import {
  CardModel,
  ThreeCardsBetListModel,
  ThreeCardsModel,
} from "../../common/model/ThreeCardsModel";
import { Notice } from "../../shared/notice/notice";
import Card from "../../shared/card/card";
import Chance from "chance";

type ThreeCardsProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

const cardSuit = [0, 1, 2, 3];
const cardRank = [
  "A",
  "2",
  "3",
  "4",
  "5",
  "6",
  "7",
  "8",
  "9",
  "10",
  "J",
  "Q",
  "K",
];
export default class ThreeCards extends React.Component<
  ThreeCardsProps,
  ThreeCardsModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  private systemId: SystemId;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new ThreeCardsModel();
    this.systemId = SystemId.None;
    this.handleUpdateFilter = this.handleUpdateFilter.bind(this);
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleUpdateFilter(value: SystemId) {
    this.systemId = value;
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 30) {
        clearTimeout(this.timeout);
        const model = new ThreeCardsModel();
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
      const waitingTime = 6;
      this.setState({
        result: data.finish.result,
        settlementResult: data.finish.settlementResult,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        const newState = new ThreeCardsModel();

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
        this.setState((prevState) => ({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result: data.result ? data.result : prevState.result,
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

  getUserBetList(data, currentData: ThreeCardsBetListModel) {
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

  renderMonitor() {
    const { betList, roundNumber, time, result, settlementResult, gameState } =
      this.state;

    return (
      <Container className={styles["threecards-container"]}>
        <Row>
          <Col align="center" className="my-auto col-2">
            <TimeCounter
              remainingTime={time.remainingTime}
              gameState={gameState}
              key={gameState}
            />
          </Col>
          <Col align="center" className="my-auto">
            <div className={styles.title}>Three Cards</div>
            <Row className={styles.result}>
              {result.map((card, index) => (
                <div key={index} onClick={() => this.openModal(index)}>
                  <Card value={card.rank} suit={card.suit} />
                </div>
              ))}
            </Row>
            <RoundNum roundNumber={roundNumber} />
          </Col>
          <Col align="center" className="col-4">
            <Row className="justify-content-center">
              <TotalBet
                total={betList.red.total}
                type="red"
                isWin={settlementResult.red}
              />
              <TotalBet
                total={betList.black.total}
                type="black"
                isWin={settlementResult.black}
              />
            </Row>
            <Row className="justify-content-center">
              <TotalBet
                total={betList.diamonds.total}
                type="diamonds"
                isWin={settlementResult.diamonds}
              />
              <TotalBet
                total={betList.hearts.total}
                type="hearts"
                isWin={settlementResult.hearts}
              />
              <TotalBet
                total={betList.spades.total}
                type="spades"
                isWin={settlementResult.spades}
              />
              <TotalBet
                total={betList.clubs.total}
                type="clubs"
                isWin={settlementResult.clubs}
              />
            </Row>
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
            dataList={betList.red.dataList}
            titles={[<div className={styles["text-red"]}>Red</div>]}
            isWin={settlementResult.red}
            systemFilter={this.systemId}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.black.dataList}
            titles={[<div className={styles["text-black"]}>Black</div>]}
            isWin={settlementResult.black}
            systemFilter={this.systemId}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.diamonds.dataList}
            titles={[
              <div className={`${styles["text-red"]} ${styles["text-suit"]}`}>
                ♦️
              </div>,
            ]}
            isWin={settlementResult.diamonds}
            systemFilter={this.systemId}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.hearts.dataList}
            titles={[
              <div className={`${styles["text-red"]} ${styles["text-suit"]}`}>
                ♥️
              </div>,
            ]}
            isWin={settlementResult.hearts}
            systemFilter={this.systemId}
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.spades.dataList}
            titles={[
              <div className={`${styles["text-black"]} ${styles["text-suit"]}`}>
                ♠️
              </div>,
            ]}
            isWin={settlementResult.spades}
            systemFilter={this.systemId}
          />
        </Col>{" "}
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.clubs.dataList}
            titles={[
              <div className={`${styles["text-black"]} ${styles["text-suit"]}`}>
                ♣️
              </div>,
            ]}
            isWin={settlementResult.clubs}
            systemFilter={this.systemId}
          />
        </Col>
      </React.Fragment>
    );
  }

  openModal(cardIndex) {
    this.setState({
      isOpenModal: true,
      currentSelectedIndex: cardIndex,
    });
  }

  toggleModal() {
    this.setState({
      isOpenModal: !this.state.isOpenModal,
    });
  }

  renderGameControl() {
    const { isOpenModal } = this.state;

    return (
      <Modal
        isOpen={isOpenModal}
        toggle={this.toggleModal}
        className={styles["desk-of-cards"]}
      >
        <ModalHeader
          toggle={this.toggleModal}
          className={styles["modal-header"]}
        />
        <ModalBody>
          {cardSuit.map((suit, suitIndex) => (
            <Row key={suitIndex} className={styles["card-row"]}>
              {cardRank.map((rank, rankIndex) => (
                <div
                  key={rankIndex}
                  className={styles.card}
                  onClick={() => this.handleChangeResult(rank, suit)}
                >
                  <Card value={rank} suit={suit} />
                </div>
              ))}
            </Row>
          ))}
        </ModalBody>
      </Modal>
    );
  }

  handleChangeResult(rank, suit) {
    const { result, currentSelectedIndex } = this.state;
    result[currentSelectedIndex].rank = rank;
    result[currentSelectedIndex].suit = suit;

    const newResult = [];
    for (const card of result) {
      if (card.rank) {
        newResult.push(card);
      } else {
        newResult.push({ rank: cardRank[0], suit: cardSuit[0] });
      }
    }
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

  render() {
    return (
      <div className={styles["three-cards"]}>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate
          renderBetList={this.renderBetList()}
          renderMonitor={this.renderMonitor()}
          disabled={this.state.gameState === GameState.disabled}
          onUpdateFilter={this.handleUpdateFilter}
        />
        {this.state.gameState === GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
