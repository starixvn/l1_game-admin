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
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import { KenoMatchModel } from "../../common/model/KenoMatchModel";
import { Notice } from "../../shared/notice/notice";
import KenoMatchResult from "./keno-match-result";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";

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

export default class KenoMatch extends BaseMonitoring<KenoMatchModel> {
  isFirstLoad: boolean;
  timeout: NodeJS.Timeout;
  gameName: string;

  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new KenoMatchModel();
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  getChoiceDisplay(key: any) {
    return choiceLabelMapper[key];
  }

  renderResultSummary() {
    const { result: hfResult, settlementResult: hfSettlementResult } =
      this.state.result.firstHalf;
    console.log(this.state.result);

    if (Object.values(hfSettlementResult).some((x) => x)) {
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
              First Half: Total {hfBlueTotal + hfRedTotal} - Blue {hfBlueTotal}{" "}
              - Red {hfRedTotal}
            </div>
            <div className="font-weight-bold">
              {this.getSettlementResultString(hfSettlementResult)}
            </div>
          </Col>
          {Object.values(ftSettlementResult).some((x) => x) && (
            <Col xs={12} className={styles["result-summary"]}>
              <div>
                Full Time: Total {ftBlueTotal + ftRedTotal} - Blue {ftBlueTotal}{" "}
                - Red {ftRedTotal}
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
            {!this.props.isProduction && (
              <Button onClick={this.openModal}>Set Result</Button>
            )}
          </Col>
          <Col align="center" className="my-auto">
            <div className={styles.title}>{this.gameName}</div>
            <Row className={styles.result} noGutters>
              <Col xs={12} xl={6}>
                <div className={styles.title}>Blue</div>
                <KenoMatchResult
                  listResult={result.firstHalf.result.blue}
                  maxLength={10}
                />
                <KenoMatchResult
                  listResult={result.fullTime.result.blue}
                  maxLength={10}
                />
              </Col>
              <Col xs={12} xl={6}>
                <div className={styles.title}>Red</div>
                <KenoMatchResult
                  listResult={result.firstHalf.result.red}
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
                isWin={result.firstHalf.settlementResult.blue}
              />
              <TotalBet
                total={betList.fhRed.total}
                type="red"
                isWin={result.firstHalf.settlementResult.red}
              />
              <TotalBet
                total={betList.fhOver.total}
                type="over"
                isWin={result.firstHalf.settlementResult.over}
              />
              <TotalBet
                total={betList.fhUnder.total}
                type="under"
                isWin={result.firstHalf.settlementResult.under}
              />
              <TotalBet
                total={betList.fhOdd.total}
                type="odd"
                isWin={result.firstHalf.settlementResult.odd}
              />
              <TotalBet
                total={betList.fhEven.total}
                type="even"
                isWin={result.firstHalf.settlementResult.even}
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
                total={betList.liveFTBlue.total}
                type="blue"
                isWin={result.fullTime.settlementResult.blue}
              />
              <TotalBet
                total={betList.liveFTRed.total}
                type="red"
                isWin={result.fullTime.settlementResult.red}
              />
              <TotalBet
                total={betList.liveFTOver.total}
                type="over"
                isWin={result.fullTime.settlementResult.over}
              />
              <TotalBet
                total={betList.liveFTUnder.total}
                type="under"
                isWin={result.fullTime.settlementResult.under}
              />
              <TotalBet
                total={betList.liveFTOdd.total}
                type="odd"
                isWin={result.fullTime.settlementResult.odd}
              />
              <TotalBet
                total={betList.liveFTEven.total}
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
            <h3 className={styles.title}>#{roundNumber + 1}</h3>
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
      firstHalf: {
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
      <div className={styles["three-cards"]}>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          enableFilter={true}
          disabled={this.state.gameState === GameState.disabled}
          partners={this.props.partners}
        />
        {this.state.gameState === GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
