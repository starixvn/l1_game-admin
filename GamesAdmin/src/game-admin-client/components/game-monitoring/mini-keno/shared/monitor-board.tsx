import React, { useEffect } from 'react';
import { Col, Container, Row } from 'reactstrap';
import style from './monitor-board.module.scss';
import TimeCounter from '../../../shared/time-counter/count-down';
import RoundNum from '../../../shared/round-num/round-num';
import AmountBoard from './amount-board';
import { BetChoice } from './model';

type Props = {
  gameName: string,
  remainingTime: number
  roundId: number
  renderSettlementResult: Function
  showSetResultPanel: (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void
  choices: Array<BetChoice>
  placingBetTime: number
  lockingBetTime: number,
  reset: Function,
  disabled: boolean
};

const GAME_NAMES = {
  "kenoMax": "Keno Max",
  "kenoMax2": "Keno Max 2",
  "kenoMini": "Keno Mini",
  "kenoMini2": "Keno Mini 2",
  "kenoEast": "Keno East",
  "kenoWest": "Keno West",
  "kenoSouth": "Keno South",
  "kenoNorth": "Keno North"
};

export default function (props: Props) {
  const {
    gameName,
    remainingTime,
    roundId,
    placingBetTime,
    lockingBetTime,
    renderSettlementResult,
    showSetResultPanel,
    reset,
    choices,
    disabled
  } = props;

  const timeCounterRef = React.createRef<TimeCounter>();

  useEffect(() => {
    timeCounterRef.current.setTime(props.remainingTime);
  }, [props.remainingTime, props.roundId]);

  useEffect(() => {
    timeCounterRef.current.onCountDown((time) => {
      if (time === placingBetTime && time !== 0) {
        reset();
      }
    });
  }, [props.placingBetTime]);

  return (
    <Container fluid>
      <Row>
        <Col align="center" className='col-12 col-lg-2'>
          <span className={style["game-name"]}>{GAME_NAMES[gameName]}</span>
        </Col>
        <Col className='col-12 col-lg-3'>
          <Row className='justify-content-center'>
            <div className={style['time-counter']}>
              <TimeCounter
                ref={timeCounterRef}
                remainingTime={remainingTime}
                placingBetTime={placingBetTime}
                lockingBetTime={lockingBetTime} />
              <RoundNum roundNumber={roundId} />
            </div>
            <div onClick={showSetResultPanel}>
              {renderSettlementResult()}
            </div>
          </Row>
        </Col>
        <Col className='col-12 col-lg-7'>
          <AmountBoard choices={choices} />
        </Col>
      </Row>
    </Container>
  )
}
