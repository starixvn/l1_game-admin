import React from "react";
import { Col, Container, Row } from "reactstrap";

import BetItem from "../../common/model/BetItem";
import RouletteModel, { RouletteBetList } from "../../common/model/RouletteModel";
import BetList from "../../shared/bet-list/bet-list";
import { GameState } from "../../shared/common/GameState";
import RoundNum from "../../shared/round-num/round-num";
import TimeCounter from "../../shared/time-counter/time-counter";
import Title from "../../shared/title";
import TotalBet from "../../shared/total-bet/total-bet";
import GameTemplate from "../game-template/game-template";
import RouletteResultUpdater from "./result-updater";
import styles from "./styles.module.scss";

type Props = {
    onMessage: Function,
    sendMessage: Function,
    isProduction: boolean
};

const PlacingBetDuration = 45;
const WaitingTime = 14;

const TYPE_OF_BETS = {
    "straightUp": "Straight Up",
    "split": "Split",
    "street": "Street",
    "triangle": "Triangle",
    "corner": "Corner",
    "fourNumbers": "Four Numbers",
    "line": "Line",
    "column": "Column",
    "dozen": "Dozen",
    "redBlack": "Red/Black",
    "oddEven": "Odd/Even",
    "bigSmall": "Big/Small",
};

const TYPE_GROUPS = {
    0: ["straightUp"],
    1: ["split"],
    2: ["street", "triangle"],
    3: ["corner", "fourNumbers"],
    4: ["line"],
    5: ["column", "dozen"],
    6: ["redBlack"],
    7: ["oddEven"],
    8: ["bigSmall"],
}

const DOZEN_CHOICE_MAPPER = {
    1: "1-12",
    2: "13-24",
    3: "25-36",
};

const COLUMN_CHOICE_MAPPER = {
    1: "1st column",
    2: "2nd column",
    3: "3rd column",
};

export default class Roulette extends React.Component<Props, RouletteModel>{
    private isFirstLoad: boolean;
    private timeout: NodeJS.Timeout;

    constructor(props: Props) {
        super(props);

        this.isFirstLoad = true;
        this.state = new RouletteModel();

        this.updaterToggle = this.updaterToggle.bind(this);
        this.updaterSubmit = this.updaterSubmit.bind(this);
        this.showUpdater = this.showUpdater.bind(this);
    }

    componentDidMount() {
        this.props.onMessage((message) => this.handleData(message));
    }

    showUpdater() {
        this.setState({
            isShowResultUpdater: true
        })
    }

    updaterToggle() {
        this.setState({
            isShowResultUpdater: !this.state.isShowResultUpdater
        })
    }

    updaterSubmit(number) {
        const obj = {
            setResult: {
                result: number,
                roundId: this.state.gameState == GameState.running ? this.state.roundNumber : this.state.roundNumber + 1
            }
        }

        this.setState({
            result: number,
            isShowResultUpdater: false
        })

        this.props.sendMessage(obj);
    }

    handleData(data) {
        if (!!data.um) {
            if (this.isFirstLoad || this.state.time.remainingTime == PlacingBetDuration) {
                clearTimeout(this.timeout);
                this.setState({
                    time: { remainingTime: 0 },
                    gameState: GameState.disabled,
                    roundNumber: this.isFirstLoad ? this.state.roundNumber : this.state.roundNumber - 1,
                    result: -1
                });
                this.isFirstLoad = false;
            }
        }

        if (!!data.remainingTime) {
            const { roundId, remainingTime, result } = data;
            if (remainingTime > PlacingBetDuration) {
                this.setState({
                    gameState: GameState.waiting,
                    roundNumber: roundId - 1,
                    result: result ? result : this.state.result,
                    time: { remainingTime: remainingTime - PlacingBetDuration }
                });

                this.timeout = setTimeout(() => {
                    this.setState({
                        gameState: GameState.running,
                        roundNumber: roundId,
                        result: -1,
                        time: {
                            remainingTime: remainingTime
                        }
                    });
                }, (remainingTime - PlacingBetDuration + 1) * 1000);
            } else {
                this.setState({
                    gameState: GameState.running,
                    roundNumber: roundId,
                    result: -1,
                    time: { remainingTime: remainingTime }
                });
            }
        }

        if (!!data.finish) {
            const { result } = data.finish;
            this.isFirstLoad = false;
            this.setState({
                result: result,
                gameState: GameState.waiting,
                time: { remainingTime: WaitingTime }
            });

            setTimeout(() => {
                this.setState({
                    result: -1,
                    gameState: GameState.running,
                    roundNumber: this.state.roundNumber + 1,
                    time: { remainingTime: PlacingBetDuration }
                });
            }, (WaitingTime + 1) * 1000);
        }

        if (!!data.ingame) {
            this.updateBetList(data.ingame);
        }
    }

    updateBetList(data) {
        const betList = new RouletteBetList();
        for (const user in data) {
            const { name, betTracks } = data[user];

            Object.keys(betTracks).forEach(type => {
                const betTrack = betTracks[type];

                Object.keys(betTrack).forEach(choice => {
                    betList[type].total += betTrack[choice];
                    betList[type].dataList.unshift(new BetItem(name, betTrack[choice], '', this.getChoice(type, choice)));
                });
            });
        }
        this.setState({ betList: betList })
    }

    getChoice(type, choice) {
        switch (type) {
            case "dozen":
                return DOZEN_CHOICE_MAPPER[choice];
            case "column":
                return COLUMN_CHOICE_MAPPER[choice];
            case "line":
            case "split":
            case "street":
            case "triangle":
            case "corner":
            case "fourNumbers":
                return choice.replace(/-/g, ',');
            default:
                return choice;
        }
    }

    renderMonitor() {
        const { time, roundNumber, gameState, betList } = this.state;
        return (
            <Container fluid>
                <Row>
                    <Col className="col-12 col-lg-2">
                        <Title title="Roulette" />
                    </Col>
                    <Col align='center' className='col-12 col-lg-3'>
                        <Row className="justify-content-center">
                            <div className={styles.timer}>
                                <TimeCounter remainingTime={time.remainingTime} gameState={gameState} />
                                <RoundNum roundNumber={roundNumber} />
                            </div>
                            <div onClick={this.showUpdater} className={styles.result}>{this.state.result >= 0 ? this.state.result : ""}</div>
                        </Row>
                    </Col>
                    <Col className='p-0 col-12 col-lg-7'>
                        <Row className={styles['total-bet']}>
                            <TotalBet type='straightUp' total={betList.straightUp.total} />
                            <TotalBet type='split' total={betList.split.total} />
                            <TotalBet type='street' total={betList.street.total} />
                            <TotalBet type='triangle' total={betList.triangle.total} />
                            <TotalBet type='corner' total={betList.corner.total} />
                            <TotalBet type='fourNumbers' total={betList.fourNumbers.total} />
                        </Row>
                        <Row className={styles['total-bet']}>
                            <TotalBet type='line' total={betList.line.total} />
                            <TotalBet type='column' total={betList.column.total} />
                            <TotalBet type='dozen' total={betList.dozen.total} />
                            <TotalBet type='redBlack' total={betList.redBlack.total} />
                            <TotalBet type='oddEven' total={betList.oddEven.total} />
                            <TotalBet type='bigSmall' total={betList.bigSmall.total} />
                        </Row>
                    </Col>
                </Row>
            </Container>
        )
    }

    renderBetList() {
        const result = [];
        const { betList } = this.state;

        Object.keys(TYPE_GROUPS).forEach((key, index) => {
            let titles = [];
            let dataList = [];

            TYPE_GROUPS[key].map(type => {
                titles.push(TYPE_OF_BETS[type]);
                dataList = dataList.concat(betList[type].dataList)
            });
            result.push((
                <Col key={index}>
                    <BetList type="withchoice" isWin={false} titles={[this.renderTitles(titles)]} dataList={dataList} />
                </Col>
            ));
        });
        return (<>{result}</>);
    }

    renderTitles(titles: string[]) {
        return (<div className={styles['betlist-title']}>{titles.map(title => (<Row align='center' key={title}><span >{title}</span></Row>))}</div>)
    }

    render() {
        return (
            <div>
                <GameTemplate
                    renderMonitor={this.renderMonitor()}
                    renderBetList={this.renderBetList()}
                    disabled={this.state.gameState == GameState.disabled} />
                {!this.props.isProduction && (<RouletteResultUpdater modal={this.state.isShowResultUpdater} toggle={this.updaterToggle} submit={this.updaterSubmit} />)}
            </div>
        )
    }
}