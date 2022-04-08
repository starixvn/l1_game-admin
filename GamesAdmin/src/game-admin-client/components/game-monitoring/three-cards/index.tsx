import React from "react";
import styles from "./styles.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Row, Modal, ModalHeader, ModalBody } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import {
  ThreeCardsBetListModel,
  ThreeCardsModel,
  ThreeCardsSettlementResultModel,
} from "../../common/model/ThreeCardsModel";
import { Notice } from "../../shared/notice/notice";
import Card from "../../shared/card/card";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";
import CardModel from "../../common/model/CardModel";

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
export default class ThreeCards extends BaseMonitoring<ThreeCardsModel> {
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new ThreeCardsModel();
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  get defaultResult() {
    return [new CardModel(), new CardModel(), new CardModel()];
  }

  get defaultBetList() {
    return new ThreeCardsBetListModel();
  }

  get defaultSettlementResult(): any {
    return new ThreeCardsSettlementResultModel();
  }

  get totalTimeInRound(): number {
    return 30;
  }

  get waitingTime(): number {
    return 6;
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
          />
        </Col>
        <Col className="col-4 col-lg">
          <BetList
            type="simple"
            dataList={betList.black.dataList}
            titles={[<div className={styles["text-black"]}>Black</div>]}
            isWin={settlementResult.black}
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
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          disabled={this.state.gameState === GameState.disabled}
          enableFilter={true}
          partners={this.props.partners}
        />
        {this.state.gameState === GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
