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
import {
  KenoPowerBetListModel,
  KenoPowerModel,
} from "../../common/model/KenoPowerModel";
import { Notice } from "../../shared/notice/notice";
import KenoPowerResult from "./keno-power-result";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";

const choiceLabelMapper = {
  blue: "Blue",
  red: "Red",
  big: "Big",
  small: "Small",
  odd: "Odd",
  even: "Even",
  blueBig: "Big",
  blueSmall: "Small",
  redBig: "Big",
  redSmall: "Small",
};

export default class KenoPower extends BaseMonitoring<KenoPowerModel> {
  gameName: string;

  constructor(props) {
    super(props);

    this.state = new KenoPowerModel();
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  get defaultResult(): any {
    return {
      red: [],
      blue: [],
    };
  }

  get defaultSettlementResult(): any {
    return {};
  }

  get defaultBetList(): {} {
    return new KenoPowerBetListModel();
  }

  getChoiceDisplay(key: any) {
    return choiceLabelMapper[key];
  }

  renderResultSummary() {
    const { result, settlementResult } = this.state;

    if (Object.values(settlementResult).some((x) => x)) {
      const blueTotal = result.blue.reduce((num1, num2) => num1 + num2, 0);
      const redTotal = result.red.reduce((num1, num2) => num1 + num2, 0);

      return (
        <Col xs={12} className={styles["result-summary"]}>
          <div>
            Total {blueTotal + redTotal} - Blue {blueTotal} - Red {redTotal}
          </div>
          <div className="font-weight-bold">
            {this.getSettlementResultString(settlementResult)}
          </div>
        </Col>
      );
    }

    return null;
  }

  getSettlementResultString(settlementResult) {
    const blueRed = settlementResult.blue
      ? "Blue"
      : settlementResult.red
      ? "Red"
      : "";
    const bigSmall = settlementResult.big ? "Sum Big" : "Sum Small";
    const oddEven = settlementResult.odd ? "Sum Odd" : " Sum Even";

    const blueBigSmall = settlementResult.blueBig ? "Blue Big" : " Blue Small";
    const redBigSmall = settlementResult.redBig ? "Red Big" : "Red Small";

    return `${blueRed} - ${bigSmall} - ${oddEven} - ${blueBigSmall} - ${redBigSmall}`;
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
                <div className={styles.title}>Blue</div>
                <KenoPowerResult listResult={result.blue} />
              </Col>
              <Col xs={12} xl={6}>
                <div className={styles.title}>Red</div>
                <KenoPowerResult listResult={result.red} />
              </Col>
              {this.renderResultSummary()}
            </Row>
          </Col>
          <Col align="center" className="col-5">
            <Row className="justify-content-center" noGutters>
              <TotalBet
                total={betList.blue.total}
                type="blue"
                isWin={settlementResult.blue}
              />
              <TotalBet
                total={betList.red.total}
                type="red"
                isWin={settlementResult.red}
              />
            </Row>
            <Row className="justify-content-center" noGutters>
              <TotalBet
                total={betList.big.total}
                type="sumBig"
                isWin={settlementResult.big}
              />
              <TotalBet
                total={betList.small.total}
                type="sumSmall"
                isWin={settlementResult.small}
              />
              <TotalBet
                total={betList.odd.total}
                type="sumOdd"
                isWin={settlementResult.odd}
              />
              <TotalBet
                total={betList.even.total}
                type="sumEven"
                isWin={settlementResult.even}
              />
            </Row>
            <Row className="justify-content-center" noGutters>
              <TotalBet
                total={betList.blueBig.total}
                type="blueBig"
                isWin={settlementResult.blueBig}
              />
              <TotalBet
                total={betList.blueSmall.total}
                type="blueSmall"
                isWin={settlementResult.blueSmall}
              />
              <TotalBet
                total={betList.redBig.total}
                type="redBig"
                isWin={settlementResult.redBig}
              />
              <TotalBet
                total={betList.redSmall.total}
                type="redSmall"
                isWin={settlementResult.redSmall}
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
          <Col className="col">
            <BetList
              type="withchoice"
              dataList={betList.blue.dataList.concat(betList.red.dataList)}
              titles={[<div>Blue/Red</div>]}
            />
          </Col>
          <Col className="col">
            <BetList
              type="withchoice"
              dataList={betList.big.dataList.concat(betList.small.dataList)}
              titles={[<div>Sum Big/Small</div>]}
            />
          </Col>
          <Col className="col">
            <BetList
              type="withchoice"
              dataList={betList.even.dataList.concat(betList.odd.dataList)}
              titles={[<div>Sum Even/Odd</div>]}
            />
          </Col>
          <Col className="col">
            <BetList
              type="withchoice"
              dataList={betList.blueBig.dataList.concat(
                betList.blueSmall.dataList
              )}
              titles={[<div>Blue Big/Small</div>]}
            />
          </Col>
          <Col className="col">
            <BetList
              type="withchoice"
              dataList={betList.redBig.dataList.concat(
                betList.redSmall.dataList
              )}
              titles={[<div>Red Big/Small</div>]}
            />
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
            <FormGroup>
              <Label for="blue">Blue</Label>
              <Input name="blue" />
            </FormGroup>
            <FormGroup>
              <Label for="red">Red</Label>
              <Input name="red" />
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
      blue: event.target["blue"].value.split(",").map((x) => parseInt(x)),
      red: event.target["red"].value.split(",").map((x) => parseInt(x)),
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
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangeSystemFilter={this.onChangeSystemFilter}
          onChangePartnerFilter={this.onChangePartnerFilter}
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
