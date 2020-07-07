import React from 'react';
import Dice from '../dice/dice';
import styles from './dire-result.module.scss';
import DiceModel from '../../common/model/DiceModel';

type DiceResultProps = {
  dices: Array<DiceModel>,
  diceClickHandle?: Function
}

export default function DiceResult(props: DiceResultProps) {
  const { dices, diceClickHandle } = props;

  return (
    <div className={styles['dice-result']}>
      <div className={`row ${styles['dice-frame']}`}>
        <Dice
          id={'dice-0'}
          diceModel={dices != undefined ? dices[0] : new DiceModel()}
          isSmall={false}
          onClickHandle={diceClickHandle != undefined ? diceClickHandle : null}
        />
      </div>
      <div className={`row ${styles['dice-frame']}`}>
        <Dice
          id={'dice-1'}
          diceModel={dices != undefined ? dices[1] : new DiceModel()}
          isSmall={false}
          onClickHandle={diceClickHandle != undefined ? diceClickHandle : null}
        />
        <Dice
          id={'dice-2'}
          diceModel={dices != undefined ? dices[2] : new DiceModel()}
          isSmall={false}
          onClickHandle={diceClickHandle != undefined ? diceClickHandle : null}
        />
      </div>
    </div>
  );
}
