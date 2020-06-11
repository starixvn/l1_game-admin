import React from 'react';
import styles from './time-counter.module.scss';

type timeCounterProps = {
  remainingTime: number
}

export default function TimeCounter(props: timeCounterProps) {
  const { remainingTime } = props;

  return (
    <div className={styles.timeCounter}>
      <span className={`${remainingTime > 5 ? '' : styles.warning}`}>
        {
          remainingTime >= 10 ? remainingTime : '0' + remainingTime
        }
      </span>
    </div>

  );
}
