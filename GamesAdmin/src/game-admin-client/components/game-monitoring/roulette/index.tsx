import React from 'react';
import RouletteModel from '../../common/model/RouletteModel';
import { GameState } from '../../shared/common/GameState';
import GameTemplate from '../game-template/game-template';
import { Container, Row, Col } from 'reactstrap';
import TimeCounter from '../../shared/time-counter/time-counter';
import RoundNum from '../../shared/round-num/round-num';

type Props = {
    onMessage: Function
}

const PlacingBetDuration = 35;
const WaitingTime = 9;

export default class Roulette extends React.Component<Props, RouletteModel>{
    private isFirstLoad: boolean;
    private timeout: NodeJS.Timeout;

    constructor(props: Props) {
        super(props);

        this.isFirstLoad = true;
        this.state = new RouletteModel();
    }

    componentDidMount() {
        this.props.onMessage((message) => this.handleData(message));
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
                        time: {
                            remainingTime: remainingTime
                        }
                    });
                }, (remainingTime - PlacingBetDuration + 1) * 1000);
            } else {
                this.setState({
                    gameState: GameState.running,
                    roundNumber: roundId,
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
    }

    renderMonitor() {
        const { time, roundNumber, gameState } = this.state;
        return (
            <Container>
                <Row>
                    <Col align='center'>
                        <TimeCounter remainingTime={time.remainingTime} gameState={gameState} />
                        <div>
                            <RoundNum roundNumber={roundNumber} />
                        </div>
                    </Col>
                </Row>
            </Container>
        )
    }

    renderBetList() {
        return (<>
        </>)
    }

    render() {
        return (
            <div>
                <GameTemplate
                    renderMonitor={this.renderMonitor()}
                    renderBetList={this.renderBetList()}
                    disabled={this.state.gameState == GameState.disabled} />
            </div>
        )
    }
}