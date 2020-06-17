import React, {useEffect} from 'react';
import {Col, Container, Row} from 'reactstrap';
import style from './monitor-board.module.scss';
import TimeCounter from '../../../shared/time-counter/count-down';
import RoundNum from '../../../shared/round-num/round-num';
import AmountBoard from './amount-board';
import {BetChoice} from './model';

type Props = {
  remainingTime: number
  roundId: number
  renderSettlementResult: Function
  showSetResultPanel: (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void
  choices: Array<BetChoice>
  placingBetTime: number
  lockingBetTime: number,
  reset: Function,
}

export default function(props: Props) {
  const {
    remainingTime,
    roundId,
    placingBetTime,
    lockingBetTime,
    renderSettlementResult,
    showSetResultPanel,
    reset,
    choices,
  } = props;

  const timeCounterRef = React.createRef<TimeCounter>();

  useEffect(() => {
    timeCounterRef.current.setTime(props.remainingTime);
  }, [props.remainingTime, props.roundId]);

  useEffect(() => {
    timeCounterRef.current.onCountDown((time) => {
      if (time === placingBetTime) {
        reset();
      }
    });
  }, [props.placingBetTime]);

  return (
    <Container>
      <Row>
        <Col className='col-12 col-lg-4'>
          <Row className='justify-content-center'>
            <div className={style['time-counter']}>
              <TimeCounter
                ref={timeCounterRef}
                remainingTime={remainingTime}
                placingBetTime={placingBetTime}
                lockingBetTime={lockingBetTime}/>
              <RoundNum roundNumber={roundId} />
            </div>
            <div onClick={showSetResultPanel}>
              {renderSettlementResult()}
            </div>
          </Row>
        </Col>
        <Col className='col-12 col-lg-8'>
          <AmountBoard choices={choices}/>
        </Col>
      </Row>
    </Container>
  )
}
