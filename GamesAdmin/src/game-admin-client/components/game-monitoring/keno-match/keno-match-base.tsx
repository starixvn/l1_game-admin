import React, { FormEvent } from "react";
import styles from "./styles.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import {
  Col,
  Container,
  Row,
  Modal,
  ModalHeader,
  ModalBody,
  Form,
  FormGroup,
  Label,
  Input,
  Button,
  FormProps,
} from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import BetItem, { SystemId } from "../../common/model/BetItem";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import {
  KenoMatchBetListModel,
  KenoMatchModel,
} from "../../common/model/KenoMatchModel";
import { Notice } from "../../shared/notice/notice";
import KenoMatchResult from "./keno-match-result";

type KenoMatchProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
  gameName: string;
};

const choiceLabelMapper = {
  fhBlue: "Blue",
  fhRed: "Red",
  fhOver: "Over",
  fhUnder: "Under",
  fhOdd: "Odd",
  fhEven: "Even",
  ftBlue: "Blue",
  ftRed: "Red",
  ftOver: "Over",
  ftUnder: "Under",
  ftOdd: "Odd",
  ftEven: "Even",
  liveFTBlue: "Blue",
  liveFTRed: "Red",
  liveFTOver: "Over",
  liveFTUnder: "Under",
  liveFTOdd: "Odd",
  liveFTEven: "Even",
};

export default class KenoMatch extends React.Component<
  KenoMatchProps,
  KenoMatchModel
> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  private systemId: SystemId;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new KenoMatchModel();
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
        const model = new KenoMatchModel();
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
        const newState = new KenoMatchModel();

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
          const newState = new KenoMatchModel();

          this.setState({
            ...newState,
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 27,
            },
          });
        }, (data.remainingTime - 52) * 1000);
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

  getUserBetList(data, currentData: KenoMatchBetListModel) {
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

  renderResultSummary() {
    const { result: hfResult, settlementResult: hfSettlementResult } =
      this.state.result.halfTime;

    if (Object.values(hfSettlementResult).some(x => x)) {
      const hfBlueTotal = hfResult.blue.reduce((num1, num2) => num1 + num2, 0);
      const hfRedTotal = hfResult.red.reduce((num1, num2) => num1 + num2, 0);

      const { result: ftResult, settlementResult: ftSettlementResult } =
        this.state.result.fullTime;
      const ftBlueTotal =
        ftResult.blue.reduce((num1, num2) => num1 + num2, 0) + hfBlueTotal;
      const ftRedTotal =
        ftResult.red.reduce((num1, num2) => num1 + num2, 0) + hfRedTotal;

      return (
        <>
          <Col xs={12} className={styles["result-summary"]}>
            <div>
              First Half: Total {hfBlueTotal + hfRedTotal} - Blue {hfBlueTotal} - Red{" "}
              {hfRedTotal}
            </div>
            <div className="font-weight-bold">
              {this.getSettlementResultString(hfSettlementResult)}
            </div>
          </Col>
          {Object.values(ftSettlementResult).some(x => x) && (
            <Col xs={12} className={styles["result-summary"]}>
              <div>
                Full Time: Total {ftBlueTotal + ftRedTotal} - Blue {ftBlueTotal} -
                Red {ftRedTotal}
              </div>
              <div className="font-weight-bold">
                {this.getSettlementResultString(ftSettlementResult)}
              </div>
            </Col>
          )}
        </>
      );
    }

    return null;
  }

  getSettlementResultString(settlementResult) {
    const blueRed = settlementResult.blue
      ? "Blue - "
      : settlementResult.red
      ? "Red - "
      : "";
    const overUnder = settlementResult.over ? "Over" : "Under";
    const oddEven = settlementResult.odd ? "Odd" : "Even";

    return `${blueRed}${overUnder} - ${oddEven}`;
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
            <div className={styles.title}>{this.props.gameName}</div>
            <Row className={styles.result} noGutters>
              <Col xs={12} xl={6} onClick={this.openModal}>
                <div className={styles.title}>Blue</div>
                <KenoMatchResult
                  listResult={result.halfTime.result.blue}
                  maxLength={10}
                />
                <KenoMatchResult
                  listResult={result.fullTime.result.blue}
                  maxLength={10}
                />
              </Col>
              <Col xs={12} xl={6} onClick={this.openModal}>
                <div className={styles.title}>Red</div>
                <KenoMatchResult
                  listResult={result.halfTime.result.red}
                  maxLength={10}
                />
                <KenoMatchResult
                  listResult={result.fullTime.result.red}
                  maxLength={10}
                />
              </Col>
              {this.renderResultSummary()}
            </Row>
          </Col>
          <Col align="center" className="col-5">
            <Row className="justify-content-center" noGutters>
              <Col className={styles.title} xs={12}>
                First Half
              </Col>
              <TotalBet
                total={betList.fhBlue.total}
                type="blue"
                isWin={result.halfTime.settlementResult.blue}
              />
              <TotalBet
                total={betList.fhRed.total}
                type="red"
                isWin={result.halfTime.settlementResult.red}
              />
              <TotalBet
                total={betList.fhOver.total}
                type="over"
                isWin={result.halfTime.settlementResult.over}
              />
              <TotalBet
                total={betList.fhUnder.total}
                type="under"
                isWin={result.halfTime.settlementResult.under}
              />
              <TotalBet
                total={betList.fhOdd.total}
                type="odd"
                isWin={result.halfTime.settlementResult.odd}
              />
              <TotalBet
                total={betList.fhEven.total}
                type="even"
                isWin={result.halfTime.settlementResult.even}
              />
            </Row>
            <Row className="justify-content-center" noGutters>
              <Col className={styles.title} xs={12}>
                Full Time
              </Col>
              <TotalBet
                total={betList.ftBlue.total}
                type="blue"
                isWin={result.fullTime.settlementResult.blue}
              />
              <TotalBet
                total={betList.ftRed.total}
                type="red"
                isWin={result.fullTime.settlementResult.red}
              />
              <TotalBet
                total={betList.ftOver.total}
                type="over"
                isWin={result.fullTime.settlementResult.over}
              />
              <TotalBet
                total={betList.ftUnder.total}
                type="under"
                isWin={result.fullTime.settlementResult.under}
              />
              <TotalBet
                total={betList.ftOdd.total}
                type="odd"
                isWin={result.fullTime.settlementResult.odd}
              />
              <TotalBet
                total={betList.ftEven.total}
                type="even"
                isWin={result.fullTime.settlementResult.even}
              />
            </Row>
            <Row className="justify-content-center" noGutters>
              <Col className={styles.title} xs={12}>
                Full Time (Live)
              </Col>
              <TotalBet
                total={betList.ftBlue.total}
                type="blue"
                isWin={result.fullTime.settlementResult.blue}
              />
              <TotalBet
                total={betList.ftRed.total}
                type="red"
                isWin={result.fullTime.settlementResult.red}
              />
              <TotalBet
                total={betList.ftOver.total}
                type="over"
                isWin={result.fullTime.settlementResult.over}
              />
              <TotalBet
                total={betList.ftUnder.total}
                type="under"
                isWin={result.fullTime.settlementResult.under}
              />
              <TotalBet
                total={betList.ftOdd.total}
                type="odd"
                isWin={result.fullTime.settlementResult.odd}
              />
              <TotalBet
                total={betList.ftEven.total}
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
            <div className={styles.title}>First Half</div>
            <Row noGutters>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.fhBlue.dataList.concat(
                    betList.fhRed.dataList
                  )}
                  titles={[<div>Blue/Red</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.fhOver.dataList.concat(
                    betList.fhUnder.dataList
                  )}
                  titles={[<div>Over/Under</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.fhOdd.dataList.concat(
                    betList.fhEven.dataList
                  )}
                  titles={[<div>Odd/Even</div>]}
                />
              </Col>
            </Row>
          </Col>
          <Col xs={12} md={6} lg={4}>
            <div className={styles.title}>Full Time</div>
            <Row noGutters>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.ftBlue.dataList.concat(
                    betList.ftRed.dataList
                  )}
                  titles={[<div>Blue/Red</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.ftOver.dataList.concat(
                    betList.ftUnder.dataList
                  )}
                  titles={[<div>Over/Under</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.ftOdd.dataList.concat(
                    betList.ftEven.dataList
                  )}
                  titles={[<div>Odd/Even</div>]}
                />
              </Col>
            </Row>
          </Col>
          <Col xs={12} md={6} lg={4}>
            <div className={styles.title}>Full Time (Live)</div>
            <Row noGutters>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.liveFTBlue.dataList.concat(
                    betList.liveFTRed.dataList
                  )}
                  titles={[<div>Blue/Red</div>]}
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

  openModal() {
    this.setState({
      isOpenModal: true,
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
        className={styles["result-control"]}
      >
        <ModalHeader
          toggle={this.toggleModal}
          className={styles["modal-header"]}
        />
        <ModalBody>
          <Form onSubmit={this.handleChangeResult}>
            <div className={styles.title}>First Half</div>
            <FormGroup>
              <Label for="hfBlue">Blue</Label>
              <Input name="hfBlue" />
            </FormGroup>
            <FormGroup>
              <Label for="hfRed">Red</Label>
              <Input name="hfRed" />
            </FormGroup>
            <div className={styles.title}>Full Time</div>
            <FormGroup>
              <Label for="ftBlue">Blue</Label>
              <Input name="ftBlue" />
            </FormGroup>
            <FormGroup>
              <Label for="ftRed">Red</Label>
              <Input name="ftRed" />
            </FormGroup>
            <Button type="submit">Submit</Button>
          </Form>
        </ModalBody>
      </Modal>
    );
  }

  handleChangeResult(event: FormEvent) {
    event.preventDefault();
    const newResult = {
      halfTime: {
        result: {
          blue: event.target["hfBlue"].value.split(",").map((x) => parseInt(x)),
          red: event.target["hfRed"].value.split(",").map((x) => parseInt(x)),
        },
      },
      fullTime: {
        result: {
          blue: event.target["ftBlue"].value.split(",").map((x) => parseInt(x)),
          red: event.target["ftRed"].value.split(",").map((x) => parseInt(x)),
        },
      },
    };

    this.props.sendMessage({
      set_result: {
        roundId: this.state.roundNumber,
        result: newResult
      },
    });
    const {result} = this.state;
    result.halfTime.result = newResult.halfTime.result;
    result.fullTime.result = newResult.fullTime.result;
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
