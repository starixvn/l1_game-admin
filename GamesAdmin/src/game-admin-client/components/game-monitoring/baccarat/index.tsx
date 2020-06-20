import React from 'react';
import styles from './styles.module.scss';
import BaccaratModel from '../../common/model/BaccaratModel';
import { GameState } from '../../shared/common/GameState';
import BetItem from '../../common/model/BetItem';
import { Col, Container, Row } from 'reactstrap';
import TimeCounter from '../../shared/time-counter/time-counter';
import RoundNum from '../../shared/round-num/round-num';
import TotalBet from '../../shared/total-bet/total-bet';
import BetList from '../../shared/bet-list/bet-list';
import { upperFirstLetter } from '../../util/util';
import GameTemplate from '../game-template/game-template';
import BaccaratCardResult from './baccarat-card-result';
import BaccaratResultUpdater from './baccarat-result-updater';

type Props = {
    onMessage: Function,
    sendMessage: Function,
    isProduction: boolean
};

const TOTAL_TIME = 30;
const WAITING_TIME = 15;
const CHOICES = new Array("player", "banker", "tie", "playerPair", "bankerPair", "big", "small");

export default class Baccarat extends React.Component<Props, BaccaratModel>{
    constructor(props: Props) {
        super(props);

        this.state = new BaccaratModel();
        this.showUpdater = this.showUpdater.bind(this);
        this.updaterToggle = this.updaterToggle.bind(this);
    }

    componentDidMount() {
        this.props.onMessage((message) => this.handleMessage(message));
    }

    handleMessage(message) {
        if (!!message.remainingTime) {
            this.handleStart(message);
        }

        if (!!message.finish) {
            this.handleStop(message.finish);
        }

        if (!!message.ingame) {
            this.handleBet(message.ingame);
        }
    }

    handleStart(message) {
        const { remainingTime, roundId } = message;
        let isFinished = remainingTime > TOTAL_TIME;

        if (isFinished) {
            this.setState({
                gameState: GameState.waiting,
                roundNumber: roundId - 1,
                time: {
                    remainingTime: remainingTime - TOTAL_TIME
                }
            });

            setTimeout(() => {
                this.setState({
                    gameState: GameState.running,
                    roundNumber: this.state.roundNumber + 1,
                    time: {
                        remainingTime: TOTAL_TIME
                    }
                })
            }, (remainingTime - TOTAL_TIME - 1) * 1000)
        } else {
            this.setState({
                gameState: GameState.running,
                roundNumber: roundId,
                time: {
                    remainingTime: remainingTime
                }
            })
        }
    }

    handleBet(data) {
        let betList = new BaccaratModel().betList;

        Object.keys(data).forEach(key => {
            const userObj = data[key];
            CHOICES.forEach(type => {
                if (userObj[type] > 0) {
                    betList[type].total += userObj[type];
                    betList[type].dataList.unshift(new BetItem(userObj.name, userObj[type]));
                }
            });
        });

        this.setState({ betList: betList });
    }

    handleStop(data) {
        const { result, settlementResult } = data;

        this.setState({
            result: result,
            settlementResult: settlementResult,
            gameState: GameState.waiting,
            time: {
                remainingTime: WAITING_TIME
            }
        });

        setTimeout(() => {
            this.setState({
                time: {
                    remainingTime: TOTAL_TIME
                },
                result: new BaccaratModel().result,
                settlementResult: {},
                roundNumber: this.state.roundNumber + 1,
                gameState: GameState.running
            });
        }, WAITING_TIME * 1000);
    }

    getSettlementResult(result) {
        const resultArr = [];
        CHOICES.forEach(function (type) {
            if (result[type]) {
                resultArr.push(type.toUpperCase());
            }
        })

        return resultArr.join(" - ");
    }

    getBet(type, userObj) {
        const betItem = new BetItem();
        betItem.name = userObj.name;
        betItem.bet = userObj[type];

        return betItem;
    }

    showUpdater() {
        this.setState({
            isShowResultUpdater: true
        });
    }

    checkWinner(choice) {
        const { settlementResult } = this.state;

        return settlementResult && settlementResult[choice]
    }

    renderMonitor() {
        const { time, roundNumber, betList, result } = this.state;
        return (
            <Container>
                <Row>
                    <Col className='col-12 col-lg-4'>
                        <Row className='justify-content-center'>
                            <Col align="center" className={styles.timecounter}>
                                <TimeCounter remainingTime={time.remainingTime} gameState={this.state.gameState} key={this.state.gameState}></TimeCounter>
                                <RoundNum roundNumber={roundNumber} />
                                {/* <input type="button" value="Set Result" className={styles['set-button']} onClick={this.showUpdater}></input> */}
                            </Col>

                        </Row>
                        <Row>
                            <Col align="center">
                                <div className={styles.title}>Player</div>
                                <BaccaratCardResult cardResult={result.playerCards} />
                            </Col>
                            <Col align="center">
                                <div className={styles.title}>Banker</div>
                                <BaccaratCardResult cardResult={result.bankerCards} />
                            </Col>
                        </Row>
                    </Col>
                    <Col className='p-0 col-12 col-lg-8'>
                        <Row className={styles['row-style']}>
                            <TotalBet type='player' total={betList.player.total} isWin={this.checkWinner('player')} />
                            <TotalBet type='banker' total={betList.banker.total} isWin={this.checkWinner('banker')} />
                            <TotalBet type='baccaratBig' total={betList.big.total} isWin={this.checkWinner('big')} />
                            <TotalBet type='baccaratSmall' total={betList.small.total} isWin={this.checkWinner('small')} />
                        </Row>
                        <Row className={styles['row-style']}>
                            <TotalBet type='tie' total={betList.tie.total} isWin={this.checkWinner('tie')} />
                            <TotalBet type='playerPair' total={betList.playerPair.total} isWin={this.checkWinner('playerPair')} />
                            <TotalBet type='bankerPair' total={betList.bankerPair.total} isWin={this.checkWinner('bankerPair')} />
                        </Row>
                    </Col>
                </Row>
            </Container >
        );
    }

    renderBetList() {
        const result = [];
        const _this = this;
        CHOICES.forEach(function (choice, i) {
            result.push((
                <Col className={styles['baccarat-betlist']} key={i}>
                    <BetList type="onecolumn" isWin={_this.checkWinner(choice)} titles={[<div>{_this.renderTitle(upperFirstLetter(choice))}</div>]} dataList={_this.state.betList[choice].dataList} />
                </Col>
            ));
        })

        return (
            <React.Fragment>
                {result}
            </React.Fragment>
        )
    }

    renderTitle(value) {
        return <span className={styles['title']}>{value}</span>
    }

    updaterToggle() {
        this.setState({
            isShowResultUpdater: !this.state.isShowResultUpdater
        })
    }

    updaterSubmit(list) {
        const obj = {
            setResult: {
                result: list,
                roundId: this.state.gameState == GameState.running ? this.state.roundNumber : this.state.roundNumber + 1
            }
        }

        this.setState({
            isShowResultUpdater: false
        })
        this.props.sendMessage(obj);
    }

    render() {
        return (
            <div>
                <GameTemplate renderMonitor={this.renderMonitor()} renderBetList={this.renderBetList()} />
                ${!this.props.isProduction && (<BaccaratResultUpdater modal={this.state.isShowResultUpdater} toggle={this.updaterToggle} submit={this.updaterSubmit} />)}
            </div>
        );
    }
}
