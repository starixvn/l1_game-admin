import React from 'react';
import styles from './keno-result.module.scss'

export default function KenoResult(props: { listResult: Array<number>, maxLength?: number }) {
  const { listResult, maxLength } = props;

  const generateResult = result => {
    let kenoResult = [];
    for (let i = 0; i < (maxLength || 20); i++) {
      kenoResult.push(
        (
          <div key={`result-${i}`} className={styles['result-value']}>{result != undefined && result[i] != undefined ? result[i] : ' '}</div>
        )
      )
    }

    return kenoResult;
  }

  return (
    <div className={styles['keno-result']}>
      {
        generateResult(listResult)
      }
    </div>
  );
}
