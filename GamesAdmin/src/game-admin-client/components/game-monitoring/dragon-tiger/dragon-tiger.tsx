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
import DragonTigerModel from "../../common/model/DragonTigerModel";
import BetItem, { SystemId } from "../../common/model/BetItem";
import { getTotalBetOnSystem, upperFirstLetter } from "../../util/util";
import { GameState } from "../../shared/common/GameState";
import DragonTigerCardResult from "./dragon-tiger-card-result";
import DeskOfCards from "../baccarat//desk-of-cards";

type Props = {
  onMessage: Function;
  isProduction: boolean;
  sendMessage: Function;
};

type State = {
  data: DragonTigerModel;
  gameState: number;
  isShowDeskOfCards: boolean;
  selectedCard: string;
};

const TYPES = ["dragon", "tie", "tiger"];
const TOTAL_TIME = 30;
const WAITING_TIME = 5;

enum CardResultMapping {
  dragon,
  tiger,
}

export default class DragonTiger extends React.Component<Props, State> {
  private isFirstLoad: boolean;
  private timeOut: NodeJS.Timeout;
  private systemFilter: SystemId;

  constructor(props: any) {
    super(props);

    this.isFirstLoad = true;
    this.systemFilter = SystemId.None;
    this.state = {
      data: new DragonTigerModel(),
      gameState: 0,
      isShowDeskOfCards: false,
      selectedCard: "",
    };

    this.showDeskOfCards = this.showDeskOfCards.bind(this);
    this.updaterDeskOfCardsToggle = this.updaterDeskOfCardsToggle.bind(this);
    this.onPickCard = this.onPickCard.bind(this);
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleMessage(message));
  }

  handleMessage(message) {
    if (message.um !== undefined) {
      if (
        this.isFirstLoad ||
        this.state.data.time.remainingTime == TOTAL_TIME
      ) {
        clearTimeout(this.timeOut);
        let data = this.state.data;
        data.time.remainingTime = 0;
        data.roundNumber = this.isFirstLoad
          ? data.roundNumber
          : data.roundNumber - 1;
        this.setState({
          data: data,
          gameState: GameState.disabled,
        });
        this.isFirstLoad = false;
      }
    } else if (message.roundId !== undefined) {
      this.handleStart(message);
    } else if (message.finish !== undefined) {
      this.handleStop(message);
    } else {
      this.handleBet(message);
    }
  }

  handleBet(message) {
    const newData = new DragonTigerModel();
    newData.roundNumber = this.state.data.roundNumber;
    newData.time.remainingTime = this.state.data.time.remainingTime;

    Object.keys(message.inGame).forEach((key) => {
      const userObj = message.inGame[key];
      const nameObj = userObj.name;
      TYPES.forEach((type) => {
        const betItem = new BetItem();
        if (userObj.betTracks[type] > 0) {
          betItem.bet = userObj.betTracks[type];
          betItem.name = nameObj;
          betItem.system = userObj.system;
          newData[type].dataList.unshift(betItem);
          newData[type].total += userObj.betTracks[type];
        }
      });
    });

    this.setState({ data: newData });
  }

  handleStart(message) {
    const newData = new DragonTigerModel();
    let isFinished = message.remainingTime > TOTAL_TIME;
    newData.time.remainingTime = isFinished
      ? message.remainingTime - TOTAL_TIME - 1
      : message.remainingTime;
    newData.roundNumber = isFinished ? message.roundId - 1 : message.roundId;
    newData.result = this.state.data.result;
    if (isFinished && message.result) {
      newData.result = this.getResult(message.result);
    }

    this.setState({
      data: newData,
      gameState: isFinished ? GameState.waiting : GameState.running,
    });

    if (isFinished) {
      this.countDownToStart.call(this, newData, newData.time.remainingTime + 1);
    }
  }

  handleStop(message) {
    const newData = this.state.data;
    newData.roundNumber = message.finish.roundId;
    newData.result = this.getResult(message.finish.result);
    newData.time.remainingTime = WAITING_TIME;
    this.isFirstLoad = false;

    this.setState({
      data: newData,
      gameState: GameState.waiting,
    });

    this.countDownToStart.call(this, newData, WAITING_TIME + 1);
  }

  getBet(type, userObj) {
    const betItem = new BetItem();
    betItem.name = userObj.name;
    betItem.bet = userObj[type];

    return betItem;
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

  countDownToStart(newData, time) {
    this.timeOut = setTimeout(() => {
      newData.time.remainingTime = TOTAL_TIME;
      newData.roundNumber += 1;
      this.setState({
        data: newData,
        gameState: GameState.running,
      });
    }, time * 1000);
  }
  updaterDeskOfCardsToggle() {
    this.setState({
      isShowDeskOfCards: !this.state.isShowDeskOfCards,
    });
  }

  showDeskOfCards(selectedCard: string) {
    const newData = this.state.data;
    this.setState({
      data: newData,
      isShowDeskOfCards: true,
      selectedCard: selectedCard,
    });
  }

  getWiner() {
    const { result } = this.state.data;
    const [dragon, tiger] = result;

    let winer = dragon.card > tiger.card ? "dragon" : "tiger";
    winer = dragon.card == tiger.card ? "tie" : winer;

    return winer;
  }

  renderMonitor() {
    const { gameState, data } = this.state;
    const { result } = data;
    const winer =
      this.state.gameState == GameState.waiting ? this.getWiner() : "";

    if (this.systemFilter) {
      TYPES.forEach((type) => {
        data[type].total = getTotalBetOnSystem(
          data[type].dataList,
          this.systemFilter
        );
      });
    }

    return (
      <Container className={styles["dragonTiger-container"]}>
        <Row>
          <Col className="col-12 col-lg-4">
            <Row>
              <div className={styles.counter}>
                <div className={styles.title}>Dragon Tiger</div>
                <TimeCounter
                  remainingTime={data.time.remainingTime}
                  gameState={gameState}
                  key={gameState}
                />
                <RoundNum roundNumber={data.roundNumber} />
              </div>
            </Row>
            <Row>
              <Col align="center" className="col-12 col-xl-6">
                <div className={styles.title}>Dragon</div>
                <DragonTigerCardResult
                  cardResult={result[0]}
                  onCardClick={this.showDeskOfCards}
                  selectedCard="dragon"
                />
              </Col>
              <Col align="center" className="col-12 col-xl-6">
                <div className={styles.title}>Tiger</div>
                <DragonTigerCardResult
                  cardResult={result[1]}
                  onCardClick={this.showDeskOfCards}
                  selectedCard="tiger"
                />
              </Col>
            </Row>
          </Col>

          <Col align="center">
            <TotalBet
              type="dragon"
              total={data.dragon.total}
              isWin={winer == "dragon"}
            />
          </Col>

          <Col align="center">
            <Row>
              <Col>
                <DragonTigerResult result={data.result} />
              </Col>
            </Row>
            <Row>
              <Col className={styles.draw}>
                <TotalBet
                  type="draw"
                  total={data.tie.total}
                  isWin={winer == "tie"}
                />
              </Col>
            </Row>
          </Col>

          <Col align="center">
            <TotalBet
              type="tiger"
              total={data.tiger.total}
              isWin={winer == "tiger"}
            />
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
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
            dataList={_this.state.data[type].dataList}
            isWin={winer == type}
            systemFilter={this.systemFilter}
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
    const { gameState, selectedCard } = this.state;
    const { result, roundNumber } = this.state.data;
    const newData = this.state.data;
    console.log(newData);
    console.log(
      CardResultMapping[selectedCard],
      "CardResultMapping[selectedCard]"
    );
    let cardResult = new CardResult();
    cardResult.card = card;
    cardResult.type = type;
    result[CardResultMapping[selectedCard]] = cardResult;

    newData.result = result;
    console.log(newData);
    this.setState({
      data: newData,
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
          renderBetList={this.renderBetList()}
          renderMonitor={this.renderMonitor()}
          disabled={this.state.gameState == GameState.disabled}
          onUpdateFilter={(value: SystemId) => (this.systemFilter = value)}
        />
        {!this.props.isProduction && (
          <DeskOfCards
            modal={this.state.isShowDeskOfCards}
            toggle={this.updaterDeskOfCardsToggle}
            onPickCard={this.onPickCard}
          />
        )}
      </div>
    );
  }
}
