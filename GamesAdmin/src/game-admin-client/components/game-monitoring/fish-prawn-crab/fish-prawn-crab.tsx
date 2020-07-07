import React from 'react';
import styles from './fish-prawn-crab.module.scss';
import TimeCounter from '../../shared/time-counter/time-counter';
import DiceResult from '../../shared/dice-result/dice-result';
import TotalBet from '../../shared/total-bet/total-bet';
import { Col, Container, Row, Modal } from 'reactstrap';
import GameTemplate from '../game-template/game-template';
import BetList from '../../shared/bet-list/bet-list';
import Dice from '../../shared/dice/dice';
import BetItem from '../../common/model/BetItem';
import RoundNum from '../../shared/round-num/round-num';
import DiceType from '../../common/model/DiceType';
import DiceModel from '../../common/model/DiceModel';
import { GameState } from '../../shared/common/GameState';
import { FishPrawnCrabModel, GameResult } from '../../common/model/FishPrawnCrabModel';
import { Notice } from '../../shared/notice/notice';

type FishPrawnCrabProps = {
  onMessage: Function,
  sendMessage: Function,
  isProduction: boolean
}

enum FishPrawnCrabDice {
  nai = 0,
  bau,
  ga,
  ca,
  cua,
  tom
}

export default class FishPrawnCrab extends React.Component<FishPrawnCrabProps, FishPrawnCrabModel> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new FishPrawnCrabModel();
    this.openModal = this.openModal.bind(this);
    this.handleChangeResult = this.handleChangeResult.bind(this);
    this.toggleModal = this.toggleModal.bind(this);
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 30) {
        clearTimeout(this.timeout);
        this.setState({
          time: { remainingTime: 0 },
          gameState: GameState.disabled
        });
        this.isFirstLoad = false;
      }
    }

    if (!!data.info) {
      this.setState({
        betList: this.getTotalBetList(data.info, this.state.betList)
      });
    }

    if (!!data.ingame) {
      this.setState({
        betList: this.getUserBetList(data.ingame, this.state.betList)
      })
    }

    if (!!data.finish) {
      this.isFirstLoad = false;
      this.setState({
        result: this.getResult(data.finish),
        winners: this.getWiners(data.finish),
      });
    }

    if (!!data.time_remain) {
      clearTimeout(this.timeout);

      if (data.time_remain > 31) {
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.phien - 1,
          time: {
            remainingTime: data.time_remain - 31
          }
        })

        this.timeout = setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            time: {
              remainingTime: 30,
            },
            roundNumber: data.phien,
            winners: [false, false, false, false, false, false],
            result: [
              new DiceModel(-1, DiceType.BauCua),
              new DiceModel(-1, DiceType.BauCua),
              new DiceModel(-1, DiceType.BauCua)
            ]
          })
        }, (data.time_remain - 30) * 1000);
      } else {
        this.setState({
          gameState: GameState.running,
          roundNumber: data.phien,
          time: {
            remainingTime: data.time_remain
          },
        })
      }
    }
  }

  getTotalBetList(data, currentData) {
    currentData.redBau.total = data.redBau;
    currentData.redCua.total = data.redCua;
    currentData.redTom.total = data.redTom;
    currentData.redCa.total = data.redCa;
    currentData.redHuou.total = data.redHuou;
    currentData.redGa.total = data.redGa;

    return currentData;
  }

  getUserBetList(data, currentData) {
    Object.keys(currentData).forEach(key => {
      currentData[key].dataList = new Array<BetItem>();
    });

    data.forEach(user => {
      if (user['0'] > 0) {
        currentData.redHuou.dataList.push(new BetItem(user.name, user['0']))
      }

      if (user['1'] > 0) {
        currentData.redBau.dataList.push(new BetItem(user.name, user['1']))
      }

      if (user['2'] > 0) {
        currentData.redGa.dataList.push(new BetItem(user.name, user['2']))
      }

      if (user['3'] > 0) {
        currentData.redCa.dataList.push(new BetItem(user.name, user['3']))
      }

      if (user['4'] > 0) {
        currentData.redCua.dataList.push(new BetItem(user.name, user['4']))
      }

      if (user['5'] > 0) {
        currentData.redTom.dataList.push(new BetItem(user.name, user['5']))
      }
    });

    return currentData;

  }

  getResult(data) {
    const result = new Array<DiceModel>();

    result.push(new DiceModel(data.dices[0], DiceType.BauCua));
    result.push(new DiceModel(data.dices[1], DiceType.BauCua));
    result.push(new DiceModel(data.dices[2], DiceType.BauCua));

    return result;
  }

  getWiners(data) {
    const winers = this.state.winners;
    winers[data.dices[0]] = true;
    winers[data.dices[1]] = true;
    winers[data.dices[2]] = true;

    return winers;
  }

  handleChangeResult(id, value) {
    const { currentDiceNumber } = this.state;

    this.props.sendMessage({
      set_dice: {
        [currentDiceNumber]: value
      }
    });

    const newResult = this.state.result;
    newResult[currentDiceNumber] = new DiceModel(value, DiceType.BauCua);
    this.setState({
      result: newResult,
      isOpenModal: false,
    })

    if (this.state.gameState == GameState.waiting) {
      const gameResult = this.state.nextRoudResult;
      gameResult[`dice-${currentDiceNumber}`] = value;

      this.setState({
        isHandleNextRoundResult: true,
        nextRoudResult: gameResult
      });
    }
  }

  openModal(id) {
    this.setState({
      isOpenModal: true,
      currentDiceNumber: id.split('-')[1]
    });
  }

  toggleModal() {
    this.setState({
      isOpenModal: !this.state.isOpenModal
    });
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, winners, gameState } = this.state;

    return (
      <Container>
        <Row className={styles["align-middle"]}>
          <Col align="center" className="col-12 col-sm-6">
            <Row className={styles["align-middle"]}>
              <Col className={styles.timecounter}>
                <div className={styles.title}>Fish Prawn Crab</div>
                <TimeCounter remainingTime={time.remainingTime} gameState={gameState}></TimeCounter>
                <RoundNum roundNumber={roundNumber} />
              </Col>
              <Col className={styles.result}>
                <DiceResult dices={result} diceClickHandle={this.openModal} />
              </Col>
            </Row>
          </Col>
          <Col align="center" className={styles.timecounter} >
            <Row className="justify-content-center">
              <TotalBet total={betList.redHuou.total} type='nai' isWin={winners[FishPrawnCrabDice.nai]}></TotalBet>
              <TotalBet total={betList.redBau.total} type='bau' isWin={winners[FishPrawnCrabDice.bau]}></TotalBet>
              <TotalBet total={betList.redGa.total} type='ga' isWin={winners[FishPrawnCrabDice.ga]}></TotalBet>
            </Row>
            <Row className="justify-content-center">
              <TotalBet total={betList.redCa.total} type='ca' isWin={winners[FishPrawnCrabDice.ca]}></TotalBet>
              <TotalBet total={betList.redCua.total} type='cua' isWin={winners[FishPrawnCrabDice.cua]}></TotalBet>
              <TotalBet total={betList.redTom.total} type='tom' isWin={winners[FishPrawnCrabDice.tom]}></TotalBet>
            </Row>
          </Col>
        </Row>
      </Container>
    )
  }

  renderBetList() {
    const { betList, winners } = this.state;

    return (
      <React.Fragment>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.redHuou.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.nai)]}
            isWin={winners[FishPrawnCrabDice.nai]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.redBau.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.bau)]}
            isWin={winners[FishPrawnCrabDice.bau]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.redGa.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.ga)]}
            isWin={winners[FishPrawnCrabDice.ga]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.redCa.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.ca)]}
            isWin={winners[FishPrawnCrabDice.ca]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.redCua.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.cua)]}
            isWin={winners[FishPrawnCrabDice.cua]}
          />
        </Col>
        <Col className="col-4 col-lg-2">
          <BetList
            type="simple"
            dataList={betList.redTom.dataList}
            titles={[this.renderBetListBau(FishPrawnCrabDice.tom)]}
            isWin={winners[FishPrawnCrabDice.tom]}
          />
        </Col>
      </React.Fragment>
    )
  }

  renderBetListBau(diceValue) {
    const dice = new DiceModel(diceValue, DiceType.BauCua);

    return (
      <Dice diceModel={dice} isSmall={true}></Dice>
    )
  }

  renderGameControl() {
    const { isOpenModal } = this.state;

    return (
      <Modal isOpen={isOpenModal} toggle={this.toggleModal} className={styles['game-control-panel']}>
        <Row>
          <Dice diceModel={new DiceModel(0, DiceType.BauCua)} isSmall={false} onClickHandle={this.handleChangeResult} />
          <Dice diceModel={new DiceModel(1, DiceType.BauCua)} isSmall={false} onClickHandle={this.handleChangeResult} />
          <Dice diceModel={new DiceModel(2, DiceType.BauCua)} isSmall={false} onClickHandle={this.handleChangeResult} />
          <Dice diceModel={new DiceModel(3, DiceType.BauCua)} isSmall={false} onClickHandle={this.handleChangeResult} />
          <Dice diceModel={new DiceModel(4, DiceType.BauCua)} isSmall={false} onClickHandle={this.handleChangeResult} />
          <Dice diceModel={new DiceModel(5, DiceType.BauCua)} isSmall={false} onClickHandle={this.handleChangeResult} />
        </Row>
      </Modal>
    )
  }

  render() {
    return (
      <div className={styles['fish-prawn-crab']}>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate renderBetList={this.renderBetList()} renderMonitor={this.renderMonitor()} disabled={this.state.gameState == GameState.disabled} />
        {this.state.gameState == GameState.disabled && <Notice isOpen={true} message="Game is disabled." />}
      </div>
    );
  }
}
