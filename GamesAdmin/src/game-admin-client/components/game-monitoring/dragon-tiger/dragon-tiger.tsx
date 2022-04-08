import React from "react";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import { Container, Row, Col } from "reactstrap";
import RoundNum from "../../shared/round-num/round-num";
import DragonTigerResult from "../../shared/dragon-tiger-result/dragon-tiger-result";
import styles from "./dragon-tiger.module.scss";
import CardResult from "../../common/model/CardResult";
import DragonTigerModel, {
  DragonTigerBetList,
} from "../../common/model/DragonTigerModel";
import { upperFirstLetter } from "../../util/util";
import { GameState } from "../../shared/common/GameState";
import DeskOfCards from "../baccarat//desk-of-cards";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";

const TYPES = ["dragon", "tie", "tiger"];
const TOTAL_TIME = 30;
const WAITING_TIME = 5;

enum CardResultMapping {
  dragon,
  tiger,
}

export default class DragonTigerState extends BaseMonitoring<DragonTigerModel> {
  constructor(props) {
    super(props);

    this.state = {
      ...new DragonTigerModel(),
      result: this.defaultResult,
      betList: this.defaultBetList,
    };

    this.showDeskOfCards = this.showDeskOfCards.bind(this);
    this.onPickCard = this.onPickCard.bind(this);
  }

  get defaultBetList(): {} {
    return new DragonTigerBetList();
  }

  get defaultResult(): any {
    return [new CardResult(), new CardResult()];
  }

  get totalTimeInRound(): number {
    return TOTAL_TIME;
  }

  get waitingTime(): number {
    return WAITING_TIME;
  }

  getCardResult(resultObj) {
    const cardResult = new CardResult();
    cardResult.card = resultObj.card;
    cardResult.type = resultObj.type;

    return cardResult;
  }

  getResult(result) {
    return [
      this.getCardResult(result.dragon),
      this.getCardResult(result.tiger),
    ];
  }

  updaterDeskOfCardsToggle() {
    this.setState({
      isShowDeskOfCards: !this.state.isShowDeskOfCards,
    });
  }

  showDeskOfCards(selectedCard: string) {
    this.setState({
      isShowDeskOfCards: true,
      selectedCard: selectedCard,
    });
  }

  getWiner() {
    const { result } = this.state;
    const [dragon, tiger] = result;

    let winer = dragon.card > tiger.card ? "dragon" : "tiger";
    winer = dragon.card == tiger.card ? "tie" : winer;

    return winer;
  }

  renderMonitor() {
    const { gameState, result, betList, time, roundNumber } = this.state;
    const winer =
      this.state.gameState == GameState.waiting ? this.getWiner() : "";

    return (
      <Container className={styles["dragonTiger-container"]}>
        <Row>
          <Col className="col-12 col-lg-4">
            <Row>
              <div className={styles.counter}>
                <div className={styles.title}>Dragon Tiger</div>
                <TimeCounter
                  remainingTime={time.remainingTime}
                  gameState={gameState}
                  key={gameState}
                />
                <RoundNum roundNumber={roundNumber} />
              </div>
            </Row>
          </Col>

          <Col align="center">
            <TotalBet
              type="dragon"
              total={betList.dragon.total}
              isWin={winer == "dragon"}
            />
          </Col>

          <Col align="center">
            <Row>
              <Col>
                <DragonTigerResult
                  result={result}
                  onClickCard={this.showDeskOfCards}
                />
              </Col>
            </Row>
            <Row>
              <Col className={styles.draw}>
                <TotalBet
                  type="draw"
                  total={betList.tie.total}
                  isWin={winer == "tie"}
                />
              </Col>
            </Row>
          </Col>

          <Col align="center">
            <TotalBet
              type="tiger"
              total={betList.tiger.total}
              isWin={winer == "tiger"}
            />
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList } = this.state;
    const winer =
      this.state.gameState == GameState.waiting ? this.getWiner() : "";

    const result = [];
    const _this = this;
    TYPES.forEach((type, i) => {
      result.push(
        <Col className="p-1" key={i}>
          <BetList
            type="simple"
            titles={[_this.renderTitle(upperFirstLetter(type))]}
            dataList={betList[type].dataList}
            isWin={winer == type}
          />
        </Col>
      );
    });

    return (
      <Container>
        <Row>{result}</Row>
      </Container>
    );
  }

  onPickCard(card: number, type: number) {
    const { gameState, selectedCard, result, roundNumber } = this.state;

    const newResult = result.map((cardInfo, index) => {
      if (CardResultMapping[selectedCard] == index || !cardInfo.card) {
        cardInfo.card = card;
        cardInfo.type = type;
      }

      return cardInfo;
    });

    this.setState({
      result: newResult,
      isShowDeskOfCards: false,
    });

    const message = {
      set_result: {
        result: {
          dragon: result[CardResultMapping.dragon],
          tiger: result[CardResultMapping.tiger],
        },
        roundId: gameState == GameState.running ? roundNumber : roundNumber + 1,
      },
    };

    this.props.sendMessage(message);
  }

  renderTitle(title) {
    return <div>{title}</div>;
  }

  render() {
    return (
      <div className={styles.dragontiger}>
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          disabled={this.state.gameState == GameState.disabled}
          enableFilter={true}
          partners={this.props.partners}
        />
        {!this.props.isProduction && (
          <DeskOfCards
            isOpen={this.state.isShowDeskOfCards}
            toggle={this.updaterDeskOfCardsToggle}
            onPickCard={this.onPickCard}
          />
        )}
      </div>
    );
  }
}
