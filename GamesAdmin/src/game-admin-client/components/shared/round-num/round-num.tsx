import React from 'react';
import styles from './round-num.module.scss';

export default function RoundNum(props: { roundNumber: number }) {
  const { roundNumber } = props;

  return (
    <div className={styles.roundnum}>
      #{roundNumber}
    </div>

  );
}
