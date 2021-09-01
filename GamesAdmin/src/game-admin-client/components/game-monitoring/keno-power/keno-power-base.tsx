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
} from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import BetItem, { SystemId } from "../../common/model/BetItem";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import {
  KenoPowerBetListModel,
  KenoPowerModel,
} from "../../common/model/KenoPowerModel";
import { Notice } from "../../shared/notice/notice";
import KenoPowerResult from "./keno-power-result";

type KenoPowerProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

const choiceLabelMapper = {
  playerBig: "big",
  playerSmall: "small",
  bankerBig: "big",
  bankerSmall: "small",
};

export default class KenoPower extends React.Component<
  KenoPowerProps,
  KenoPowerModel
> {
  isFirstLoad: boolean;
  timeout: NodeJS.Timeout;
  gameName: string;

  private systemId: SystemId;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new KenoPowerModel();
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

  handleData(data) {}

  getUserBetList(data, currentData: KenoPowerBetListModel) {
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
              choiceLabelMapper[key] || key,
              data[user].system
            )
          );
        }
      });
    }

    return currentData;
  }

  renderResultSummary() {
    const { result, settlementResult } = this.state;

    if (Object.values(settlementResult).some((x) => x)) {
      return (
        <>
          <Col xs={12} className={styles["result-summary"]}>
            <div></div>
            <div className="font-weight-bold">
              {this.getSettlementResultString(settlementResult)}
            </div>
          </Col>
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
    const { betList, roundNumber, time, result, gameState, settlementResult } =
      this.state;

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
            {!this.props.isProduction && (
              <Button onClick={this.openModal}>Set Result</Button>
            )}
          </Col>
          <Col align="center" className="my-auto">
            <div className={styles.title}>{this.gameName}</div>
            <Row className={styles.result} noGutters>
              <Col xs={12} xl={6}>
                <div className={styles.title}>Player</div>
                <KenoPowerResult listResult={result.player} />
              </Col>
              <Col xs={12} xl={6}>
                <div className={styles.title}>Banker</div>
                <KenoPowerResult listResult={result.banker} />
              </Col>
              {this.renderResultSummary()}
            </Row>
          </Col>
          <Col align="center" className="col-5">
            <Row className="justify-content-center" noGutters>
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
            </Row>
            <Row className="justify-content-center" noGutters>
              <TotalBet
                total={betList.big.total}
                type="big"
                isWin={settlementResult.big}
              />
              <TotalBet
                total={betList.small.total}
                type="small"
                isWin={settlementResult.small}
              />
            </Row>
            <Row className="justify-content-center" noGutters></Row>
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
                  dataList={betList.player.dataList.concat(
                    betList.banker.dataList
                  )}
                  titles={[<div>Player/Banker</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.big.dataList.concat(betList.small.dataList)}
                  titles={[<div>Big/Small</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.even.dataList.concat(betList.odd.dataList)}
                  titles={[<div>Even/Odd</div>]}
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
                  dataList={betList.red.dataList.concat(
                    betList.orange.dataList
                  )}
                  titles={[<div>Color</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.playerBig.dataList.concat(
                    betList.playerSmall.dataList
                  )}
                  titles={[<div>Player Big/Small</div>]}
                />
              </Col>
              <Col className="col">
                <BetList
                  type="withchoice"
                  dataList={betList.bankerBig.dataList.concat(
                    betList.bankerSmall.dataList
                  )}
                  titles={[<div>Banker Big/Small</div>]}
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
    const { isOpenModal, roundNumber } = this.state;

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
            <h3>#{roundNumber + 1}</h3>
            <FormGroup>
              <Label for="player">Player</Label>
              <Input name="player" />
            </FormGroup>
            <FormGroup>
              <Label for="banker">Banker</Label>
              <Input name="banker" />
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
      player: event.target["player"].value.split(",").map((x) => parseInt(x)),
      banker: event.target["banker"].value.split(",").map((x) => parseInt(x)),
    };

    this.props.sendMessage({
      set_result: {
        roundId: this.state.roundNumber + 1,
        result: newResult,
      },
    });

    this.setState({
      isOpenModal: false,
    });
  }

  render() {
    return (
      <div>
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
