import React from "react";
import styles from "./vip-sedie-war.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Modal, Row } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import Dice from "../../shared/dice/dice";
import DiceModel from "../../common/model/DiceModel";
import DiceType from "../../common/model/DiceType";
import RoundNum from "../../shared/round-num/round-num";
import { GameState } from "../../shared/common/GameState";
import VipSeDieWarModel, {
  VipSeDieWarBetList,
  VipSeDieWarResult,
} from "../../common/model/VipSeDieWarModel";
import { Notice } from "../../shared/notice/notice";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";
import { VipFishPrawnCrabSettlementResult } from "../../common/model/VipFishPrawnCrabModel";

export default class VipSeDieWar extends BaseMonitoring<VipSeDieWarModel> {
  constructor(props) {
    super(props);

    this.state = new VipSeDieWarModel();
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  get totalTimeInRound(): number {
    return 45;
  }

  get waitingTime(): number {
    return 10;
  }

  get defaultResult(): any {
    return new VipSeDieWarResult();
  }

  get defaultSettlementResult(): any {
    return new VipFishPrawnCrabSettlementResult();
  }

  get defaultBetList(): {} {
    return new VipSeDieWarBetList();
  }

  getResult(data) {
    const result = new VipSeDieWarResult();
    result.banker = [];
    result.player = [];

    Object.keys(data.player).forEach((key) => {
      result.player.push(
        new DiceModel(data.player[key] ? 1 : 0, DiceType.XocXoc)
      );
    });

    Object.keys(data.banker).forEach((key) => {
      result.banker.push(
        new DiceModel(data.banker[key] ? 1 : 0, DiceType.XocXoc)
      );
    });

    return result;
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState, settlementResult } =
      this.state;

    return (
      <Container className={styles["sedie-container"]}>
        <Row>
          <Col align="center" className={styles["sedie-timmer"]}>
            <TimeCounter
              remainingTime={time.remainingTime}
              gameState={gameState}
            />
            <div>
              <div className={styles.title}>Se Die War</div>
              <div className={styles.title}>Player</div>
              <Row className={styles.result}>
                <Dice
                  id="player-0"
                  diceModel={result.player[0]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="player-1"
                  diceModel={result.player[1]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="player-2"
                  diceModel={result.player[2]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="player-3"
                  diceModel={result.player[3]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
              </Row>
              <div className={styles.title}>Banker</div>
              <Row className={styles.result}>
                <Dice
                  id="banker-0"
                  diceModel={result.banker[0]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="banker-1"
                  diceModel={result.banker[1]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="banker-2"
                  diceModel={result.banker[2]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
                <Dice
                  id="banker-3"
                  diceModel={result.banker[3]}
                  isSmall={false}
                  onClickHandle={this.openModal}
                />
              </Row>
              <RoundNum roundNumber={roundNumber} />
            </div>
          </Col>
          <Col align="center" className={styles["sedie-betList"]}>
            <Row>
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
              <TotalBet
                total={betList.tie.total}
                type="tie"
                isWin={settlementResult.tie}
              />
              <TotalBet
                total={betList.even.total}
                type="totalEven"
                isWin={settlementResult.even}
              ></TotalBet>
              <TotalBet
                total={betList.odd.total}
                type="totalOdd"
                isWin={settlementResult.odd}
              ></TotalBet>
              <TotalBet
                total={betList.playerEven.total}
                type="playerEven"
                isWin={settlementResult.playerEven}
              />
              <TotalBet
                total={betList.playerOdd.total}
                type="playerOdd"
                isWin={settlementResult.playerOdd}
              />
              <TotalBet
                total={betList.bankerEven.total}
                type="bankerEven"
                isWin={settlementResult.bankerEven}
              />
              <TotalBet
                total={betList.bankerOdd.total}
                type="bankerOdd"
                isWin={settlementResult.bankerOdd}
              />
            </Row>
            <Row>
              <TotalBet
                total={betList.playerRed4.total}
                type="playerRed4"
                isWin={settlementResult.playerRed4}
              />
              <TotalBet
                total={betList.playerRed3.total}
                type="playerRed3"
                isWin={settlementResult.playerRed3}
              />
              <TotalBet
                total={betList.playerWhite3.total}
                type="playerWhite3"
                isWin={settlementResult.playerWhite3}
              />
              <TotalBet
                total={betList.playerWhite4.total}
                type="playerWhite4"
                isWin={settlementResult.playerWhite4}
              />
              <TotalBet
                total={betList.bankerRed4.total}
                type="bankerRed4"
                isWin={settlementResult.bankerRed4}
              />
              <TotalBet
                total={betList.bankerRed3.total}
                type="bankerRed3"
                isWin={settlementResult.bankerRed3}
              />
              <TotalBet
                total={betList.bankerWhite3.total}
                type="bankerWhite3"
                isWin={settlementResult.bankerWhite3}
              />
              <TotalBet
                total={betList.bankerWhite4.total}
                type="bankerWhite4"
                isWin={settlementResult.bankerWhite4}
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
        <Col className="col-12">
          <Row>
            <Col>
              <BetList
                titles={[<div className="text-center">Player</div>]}
                type="simple"
                dataList={betList.player.dataList}
                isWin={settlementResult.player}
              />
            </Col>
            <Col>
              <BetList
                titles={[<div className="text-center">Banker</div>]}
                type="simple"
                dataList={betList.banker.dataList}
                isWin={settlementResult.banker}
              />
            </Col>
            <Col>
              <BetList
                titles={[<div className="text-center">Tie</div>]}
                type="simple"
                dataList={betList.tie.dataList}
                isWin={settlementResult.tie}
              />
            </Col>
            <Col>
              <BetList
                titles={[<div className="text-center">Total Even</div>]}
                type="simple"
                dataList={betList.even.dataList}
                isWin={settlementResult.even}
              />
            </Col>
            <Col>
              <BetList
                titles={[<div className="text-center">Total Odd</div>]}
                type="simple"
                dataList={betList.odd.dataList}
                isWin={settlementResult.odd}
              />
            </Col>
            <Col>
              <BetList
                titles={[<div className="text-center">Player Even</div>]}
                type="simple"
                dataList={betList.playerEven.dataList}
                isWin={settlementResult.playerEven}
              />
            </Col>
            <Col>
              <BetList
                titles={[<div className="text-center">Player Odd</div>]}
                type="simple"
                dataList={betList.playerOdd.dataList}
                isWin={settlementResult.playerOdd}
              />
            </Col>
            <Col>
              <BetList
                titles={[<div className="text-center">Banker Even</div>]}
                type="simple"
                dataList={betList.bankerEven.dataList}
                isWin={settlementResult.bankerEven}
              />
            </Col>
            <Col>
              <BetList
                titles={[<div className="text-center">Banker Odd</div>]}
                type="simple"
                dataList={betList.bankerOdd.dataList}
                isWin={settlementResult.bankerOdd}
              />
            </Col>
          </Row>
        </Col>
        <Col className="col-12">
          <Row>
            <Col>
              <BetList
                titles={[
                  <div className="text-center">
                    Player
                    <div className={styles.bondo}></div>
                  </div>,
                ]}
                type="simple"
                dataList={betList.playerRed4.dataList}
                isWin={settlementResult.playerRed4}
              />
            </Col>
            <Col>
              <BetList
                titles={[
                  <div className="text-center">
                    Player
                    <div className={styles.mottrangbado}></div>
                  </div>,
                ]}
                type="simple"
                dataList={betList.playerRed3.dataList}
                isWin={settlementResult.playerRed3}
              />
            </Col>
            <Col>
              <BetList
                titles={[
                  <div className="text-center">
                    Player
                    <div className={styles.motdobatrang}></div>
                  </div>,
                ]}
                type="simple"
                dataList={betList.playerWhite3.dataList}
                isWin={settlementResult.playerWhite3}
              />
            </Col>
            <Col>
              <BetList
                titles={[
                  <div className="text-center">
                    Player
                    <div className={styles.bontrang}></div>
                  </div>,
                ]}
                type="simple"
                dataList={betList.playerWhite4.dataList}
                isWin={settlementResult.playerWhite4}
              />
            </Col>
            <Col>
              <BetList
                titles={[
                  <div className="text-center">
                    Banker
                    <div className={styles.bondo}></div>
                  </div>,
                ]}
                type="simple"
                dataList={betList.bankerRed4.dataList}
                isWin={settlementResult.bankerRed4}
              />
            </Col>
            <Col>
              <BetList
                titles={[
                  <div className="text-center">
                    Banker
                    <div className={styles.mottrangbado}></div>
                  </div>,
                ]}
                type="simple"
                dataList={betList.bankerRed3.dataList}
                isWin={settlementResult.bankerRed3}
              />
            </Col>
            <Col>
              <BetList
                titles={[
                  <div className="text-center">
                    Banker
                    <div className={styles.motdobatrang}></div>
                  </div>,
                ]}
                type="simple"
                dataList={betList.bankerWhite3.dataList}
                isWin={settlementResult.bankerWhite3}
              />
            </Col>
            <Col>
              <BetList
                titles={[
                  <div className="text-center">
                    Banker
                    <div className={styles.bontrang}></div>
                  </div>,
                ]}
                type="simple"
                dataList={betList.bankerWhite4.dataList}
                isWin={settlementResult.bankerWhite4}
              />
            </Col>
          </Row>
        </Col>
      </React.Fragment>
    );
  }

  handleChangeResult(id, value) {
    const { selectedDice, result } = this.state;
    const splits = selectedDice.split("-");
    const side = splits[0];
    const position = parseInt(splits[1]);
    result[side][position] = new DiceModel(value, DiceType.XocXoc);
    const newResult = { player: {}, banker: {} };

    result.player.forEach(
      (dice, index) => (newResult.player[`red${index + 1}`] = dice.value === 1)
    );
    result.banker.forEach(
      (dice, index) => (newResult.banker[`red${index + 1}`] = dice.value === 1)
    );

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

  openModal(id) {
    this.setState({
      isOpenModal: true,
      selectedDice: id,
    });
  }
  toggleModal() {
    this.setState((prevState) => ({
      isOpenModal: !prevState.isOpenModal,
    }));
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
            diceModel={new DiceModel(1, DiceType.XocXoc)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
          <Dice
            diceModel={new DiceModel(0, DiceType.XocXoc)}
            isSmall={false}
            onClickHandle={this.handleChangeResult}
          />
        </Row>
      </Modal>
    );
  }

  render() {
    return (
      <div>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          enableFilter={true}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          disabled={this.state.gameState === GameState.disabled}
          partners={this.props.partners}
        ></GameTemplate>
        {this.state.gameState == GameState.disabled && (
          <Notice isOpen={true} message="Game is disabled." />
        )}
      </div>
    );
  }
}
