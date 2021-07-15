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
  KenoSoccerBetListModel,
  KenoSoccerModel,
} from "../../common/model/KenoSoccerModel";
import { Notice } from "../../shared/notice/notice";
import KennoResult from "../../shared/keno-result/keno-result";
import KenoSoccerResult from "./keno-soccer-result";

type KenoSoccerProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

const choiceLabelMapper = {
  preHTHome: "Home",
  preHTAway: "Away",
  preHTOver: "Over",
  preHTUnder: "Under",
  preHTOdd: "Odd",
  preHTEven: "Even",
  preFTHome: "Home",
  preFTAway: "Away",
  preFTOver: "Over",
  preFTUnder: "Under",
  preFTOdd: "Odd",
  preFTEven: "Even",
  liveFTHome: "Home",
  liveFTAway: "Away",
  liveFTOver: "Over",
  liveFTUnder: "Under",
  liveFTOdd: "Odd",
  liveFTEven: "Even",
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
export default class KenoSoccer extends React.Component<
  KenoSoccerProps,
  KenoSoccerModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  private systemId: SystemId;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new KenoSoccerModel();
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
      if (this.isFirstLoad || this.state.time.remainingTime == 52) {
        clearTimeout(this.timeout);
        const model = new KenoSoccerModel();
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

    if (!!data.finishHalfTime) {
      this.isFirstLoad = false;
      const waitingTime = 6;
      const { result } = this.state;
      result.halfTime = data.finishHalfTime;
      this.setState({
        result,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        this.setState({
          gameState: GameState.running,
          time: {
            remainingTime: 18,
          },
        });
      }, (waitingTime + 1) * 1000);
    }

    if (!!data.finish) {
      this.isFirstLoad = false;
      const waitingTime = 6;
      const { result } = this.state;
      result.fullTime = data.finish;
      this.setState({
        result,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        const newState = new KenoSoccerModel();

        this.setState({
          ...newState,
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 27,
          },
        });
      }, (waitingTime + 1) * 1000);
    }

    if (!!data.remainingTime) {
      const { result } = this.state;

      if (data.remainingTime > 53) {
        result.halfTime = data.halfTime;
        result.fullTime = data.fullTime;
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result,
          time: {
            remainingTime: data.remainingTime - 53,
          },
        });

        this.timeout = setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 27,
            },
          });
        }, (data.remainingTime - 53) * 1000);
      } else {
        if (data.remainingTime > 26) {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: data.remainingTime - 26,
            },
          });
        } else {
          if (data.remainingTime > 19 && data.remainingTime <= 26) {
            result.halfTime = data.halfTime;
            this.setState({
              gameState: GameState.waiting,
              roundNumber: data.roundId,
              result,
              time: {
                remainingTime: data.remainingTime - 19,
              },
            });

            this.timeout = setTimeout(() => {
              this.setState({
                gameState: GameState.running,
                roundNumber: data.roundId,
                time: {
                  remainingTime: 18,
                },
              });
            }, (data.remainingTime - 19) * 1000);
          } else {
            result.halfTime = data.halfTime;
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
    }
  }

  getUserBetList(data, currentData: KenoSoccerBetListModel) {
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
              choiceLabelMapper[key],
              data[user].system
            )
          );
        }
      });
    }

    return currentData;
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState } = this.state;

    return (
      <Container className={styles["kenosoccer-container"]}>
        <Row noGutters>
          <Col align="center" className="my-auto col-1">
            <TimeCounter
              remainingTime={time.remainingTime}
              gameState={gameState}
              key={gameState}
            />
            <RoundNum roundNumber={roundNumber} />
          </Col>
          <Col align="center" className="my-auto">
            <div className={styles.title}>My Dinh</div>
            <Row className={styles.result} noGutters>
              <Col xs={12} xl={6}>
                <div className={styles.title}>Home</div>
                <KenoSoccerResult
                  listResult={result.halfTime.result.home}
                  maxLength={10}
                />
                <KenoSoccerResult
                  listResult={result.fullTime.result.home}
                  maxLength={10}
                />
              </Col>
              <Col xs={12} xl={6}>
                <div className={styles.title}>Away</div>
                <KenoSoccerResult
                  listResult={result.halfTime.result.away}
                  maxLength={10}
                />
                <KenoSoccerResult
                  listResult={result.fullTime.result.away}
                  maxLength={10}
                />
              </Col>
            </Row>
          </Col>
          <Col align="center" className="col-5">
            <Row className="justify-content-center" noGutters>
              <Col className={styles.title} xs={12}>
                Pre-match HT
              </Col>
              <TotalBet
                total={betList.preHTHome.total}
                type="home"
                isWin={result.halfTime.settlementResult.home}
              />
              <TotalBet
                total={betList.preHTAway.total}
                type="away"
                isWin={result.halfTime.settlementResult.away}
              />
              <TotalBet
                total={betList.preHTOver.total}
                type="over"
                isWin={result.halfTime.settlementResult.over}
              />
              <TotalBet
                total={betList.preHTUnder.total}
                type="under"
                isWin={result.halfTime.settlementResult.under}
              />
              <TotalBet
                total={betList.preHTOdd.total}
                type="odd"
                isWin={result.halfTime.settlementResult.odd}
              />
              <TotalBet
                total={betList.preHTEven.total}
                type="even"
                isWin={result.halfTime.settlementResult.even}
              />
            </Row>
            <Row className="justify-content-center" noGutters>
              <Col className={styles.title} xs={12}>
                Pre-match FT
              </Col>
              <TotalBet
                total={betList.preFTHome.total}
                type="home"
                isWin={result.fullTime.settlementResult.home}
              />
              <TotalBet
                total={betList.preFTAway.total}
                type="away"
                isWin={result.fullTime.settlementResult.away}
              />
              <TotalBet
                total={betList.preFTOver.total}
                type="over"
                isWin={result.fullTime.settlementResult.over}
              />
              <TotalBet
                total={betList.preFTUnder.total}
                type="under"
                isWin={result.fullTime.settlementResult.under}
              />
              <TotalBet
                total={betList.preFTOdd.total}
                type="odd"
                isWin={result.fullTime.settlementResult.odd}
              />
              <TotalBet
                total={betList.preFTEven.total}
                type="even"
                isWin={result.fullTime.settlementResult.even}
              />
            </Row>
            <Row className="justify-content-center" noGutters>
              <Col className={styles.title} xs={12}>
                Live - FT
              </Col>
              <TotalBet
                total={betList.preFTHome.total}
                type="home"
                isWin={result.fullTime.settlementResult.home}
              />
              <TotalBet
                total={betList.preFTAway.total}
                type="away"
                isWin={result.fullTime.settlementResult.away}
              />
              <TotalBet
                total={betList.preFTOver.total}
                type="over"
                isWin={result.fullTime.settlementResult.over}
              />
              <TotalBet
                total={betList.preFTUnder.total}
                type="under"
                isWin={result.fullTime.settlementResult.under}
              />
              <TotalBet
                total={betList.preFTOdd.total}
                type="odd"
                isWin={result.fullTime.settlementResult.odd}
              />
              <TotalBet
                total={betList.preFTEven.total}
                type="even"
                isWin={result.fullTime.settlementResult.even}
              />
            </Row>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const { betList } = this.state;

    return (
      <div className={styles["bet-list-container"]}>
        <Row>
          <Col xs={12} md={6} lg={4}>
            <div className={styles.title}>Pre-match - HT</div>
            <Row noGutters>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.preHTHome.dataList.concat(
                    betList.preHTAway.dataList
                  )}
                  titles={[<div>Home/Away</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.preHTOver.dataList.concat(
                    betList.preHTUnder.dataList
                  )}
                  titles={[<div>Over/Under</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.preHTOdd.dataList.concat(
                    betList.preHTEven.dataList
                  )}
                  titles={[<div>Odd/Even</div>]}
                />
              </Col>
            </Row>
          </Col>
          <Col xs={12} md={6} lg={4}>
            <div className={styles.title}>Pre-match - FT</div>
            <Row noGutters>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.preFTHome.dataList.concat(
                    betList.preFTAway.dataList
                  )}
                  titles={[<div>Home/Away</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.preFTOver.dataList.concat(
                    betList.preFTUnder.dataList
                  )}
                  titles={[<div>Over/Under</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.preFTOdd.dataList.concat(
                    betList.preFTEven.dataList
                  )}
                  titles={[<div>Odd/Even</div>]}
                />
              </Col>
            </Row>
          </Col>
          <Col xs={12} md={6} lg={4}>
            <div className={styles.title}>Live - FT</div>
            <Row noGutters>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.liveFTHome.dataList.concat(
                    betList.liveFTAway.dataList
                  )}
                  titles={[<div>Home/Away</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.liveFTOver.dataList.concat(
                    betList.liveFTUnder.dataList
                  )}
                  titles={[<div>Over/Under</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.liveFTOdd.dataList.concat(
                    betList.liveFTEven.dataList
                  )}
                  titles={[<div>Odd/Even</div>]}
                />
              </Col>
            </Row>
          </Col>
        </Row>
      </div>
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
        <ModalBody></ModalBody>
      </Modal>
    );
  }

  handleChangeResult(rank, suit) {
    const { result, currentSelectedIndex } = this.state;
    result[currentSelectedIndex].rank = rank;
    result[currentSelectedIndex].suit = suit;

    const newResult = [];

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
        />
        {this.state.gameState === GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
