import React from "react";
import styles from "./keno-pro-max.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import { Col, Container, Row } from "reactstrap";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import KennoResult from "../../shared/keno-result/keno-result";
import RoundNum from "../../shared/round-num/round-num";
import KenoProMaxModel, {
  KenoProMaxBetList,
} from "../../common/model/KenoProMaxModel";
import BetItem from "../../common/model/BetItem";
import { GameState } from "../../shared/common/GameState";
import { updateTotalBetOfBetList, upperFirstLetter } from "../../util/util";
import KenoResultUpdater from "../../shared/keno-result-updater/keno-result-updater";
import BaseMonitoring from "../../shared/base-monitoring/base-monitoring";

type Props = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

type State = {
  data: KenoProMaxModel;
  gameState: number;
  isShowResultUpdater: boolean;
};

const TYPES = [
  "big",
  "small",
  "even",
  "odd",
  "earth",
  "fire",
  "gold",
  "water",
  "wood",
];
const TOTAL_TIME = 30;
const WAITING_TIME = 9;

export default class KenoProMax extends BaseMonitoring<KenoProMaxModel> {
  constructor(props: any) {
    super(props);

    this.isFirstLoad = true;
    this.state = new KenoProMaxModel();

    this.showUpdater = this.showUpdater.bind(this);
    this.updaterToggle = this.updaterToggle.bind(this);
    this.updaterSubmit = this.updaterSubmit.bind(this);
  }

  get totalTimeInRound(): number {
    return TOTAL_TIME;
  }

  get waitingTime(): number {
    return WAITING_TIME;
  }

  get defaultBetList(): {} {
    return new KenoProMaxBetList();
  }

  get defaultResult(): any {
    return [];
  }

  get defaultSettlementResult(): any {
    return "";
  }

  getSettlementResult(result) {
    const resultArr = [];
    TYPES.forEach(function (type) {
      if (result[type]) {
        resultArr.push(type.toUpperCase());
      }
    });

    return resultArr.join(" - ");
  }

  showUpdater() {
    this.setState({
      isShowResultUpdater: true,
    });
  }

  updaterToggle() {
    this.setState({
      isShowResultUpdater: !this.state.isShowResultUpdater,
    });
  }

  updaterSubmit(list) {
    const obj = {
      set_result: {
        result: list,
        roundId:
          this.state.gameState == GameState.running
            ? this.state.roundNumber
            : this.state.roundNumber + 1,
      },
    };

    this.setState({
      isShowResultUpdater: false,
    });
    this.props.sendMessage(obj);
  }

  checkWinner(betType) {
    const result = this.state.settlementResult;
    return result ? result.split(" - ").includes(betType.toUpperCase()) : false;
  }

  renderMonitor() {
    return (
      <Container>
        <Row>
          <Col className={`col-12 col-lg-2 ${styles.title}`}>
            <div>Keno Pro Max</div>
          </Col>
          <Col className="col-12 col-lg-3">
            <Row className="justify-content-center">
              <div className={styles.timecounter}>
                <TimeCounter
                  remainingTime={this.state.time.remainingTime}
                  gameState={this.state.gameState}
                  key={this.state.gameState}
                ></TimeCounter>
                <RoundNum roundNumber={this.state.roundNumber} />
                <div className={styles["result-sum"]}>
                  {this.state.resultSumText}
                </div>
              </div>
              <div onClick={this.showUpdater}>
                <KennoResult listResult={this.state.result}></KennoResult>
                <div className={styles["setttlement-result"]}>
                  {this.state.settlementResult}
                </div>
              </div>
            </Row>
          </Col>
          <Col className="p-0 col-12 col-lg-7">
            <Row className={styles["row-style"]}>
              <TotalBet
                type="kenogold"
                total={this.state.betList.gold.total}
                isWin={this.checkWinner("gold")}
              />
              <TotalBet
                type="kenowood"
                total={this.state.betList.wood.total}
                isWin={this.checkWinner("wood")}
              />
              <TotalBet
                type="kenowater"
                total={this.state.betList.water.total}
                isWin={this.checkWinner("water")}
              />
              <TotalBet
                type="kenofire"
                total={this.state.betList.fire.total}
                isWin={this.checkWinner("fire")}
              />
              <TotalBet
                type="kenoearth"
                total={this.state.betList.earth.total}
                isWin={this.checkWinner("earth")}
              />
            </Row>
            <Row className={styles["row-style"]}>
              <TotalBet
                type="kenobig"
                total={this.state.betList.big.total}
                isWin={this.checkWinner("big")}
              />
              <TotalBet
                type="kenosmall"
                total={this.state.betList.small.total}
                isWin={this.checkWinner("small")}
              />
              <TotalBet
                type="kenoeven"
                total={this.state.betList.even.total}
                isWin={this.checkWinner("even")}
              />
              <TotalBet
                type="kenoodd"
                total={this.state.betList.odd.total}
                isWin={this.checkWinner("odd")}
              />
            </Row>
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const result = [];
    const _this = this;

    TYPES.forEach(function (type, i) {
      result.push(
        <Col className={styles["keno-betlist"]} key={i}>
          <BetList
            type="onecolumn"
            isWin={_this.checkWinner(type)}
            titles={[<div>{_this.renderTitle(upperFirstLetter(type))}</div>]}
            dataList={_this.state.betList[type].dataList}
          />
        </Col>
      );
    });

    return <React.Fragment>{result}</React.Fragment>;
  }

  renderTitle(value) {
    return <span className={styles["keno-title"]}>{value}</span>;
  }

  render() {
    return (
      <div>
        <GameTemplate
          renderMonitor={this.renderMonitor}
          renderBetList={this.renderBetList}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          disabled={this.state.gameState == GameState.disabled}
          enableFilter={true}
          partners={this.props.partners}
        />
        $
        {!this.props.isProduction && (
          <KenoResultUpdater
            modal={this.state.isShowResultUpdater}
            toggle={this.updaterToggle}
            submit={this.updaterSubmit}
          />
        )}
      </div>
    );
  }
}
