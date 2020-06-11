import React from 'react';
import styles from './dice.module.scss';
import DiceModel from '../../common/model/DiceModel';
import DiceType from '../../common/model/DiceType';

type DiceProps = {
  diceModel: DiceModel,
  isSmall: boolean,
  id?: string,
  onClickHandle?: Function
}

export default function Dice(props: DiceProps) {
  const { diceModel, isSmall, onClickHandle, id } = props;
  const { value, type } = diceModel;

  const BauCuaDice = ['nai', 'bau', 'ga', 'ca', 'cua', 'tom'];

  const getDiceIconURL = (value, type) => {
    let rootURL = `/game-admin/images/dice/${DiceType[type].toLocaleLowerCase()}/dice-{0}.png`;
    const unknowDiceURL = '/game-admin/images/dice/dice-unknow.png';

    if (value == undefined || type == undefined || value == -1) {
      return unknowDiceURL;
    }

    switch (type) {
      case DiceType.BauCua:
        return rootURL.replace('{0}', BauCuaDice[value]);
      case DiceType.XocXoc:
        return rootURL.replace('{0}', value ? 'red' : 'white');
      case DiceType.TaiXiu:
        return rootURL.replace('{0}', value);
      default:
        return unknowDiceURL;
    }
  }

  const renderDice = () => {
    if (onClickHandle != undefined) {
      return (
        <div
          id={id}
          className={`${styles.dice} ${styles[DiceType[type]]} ${isSmall ? styles.small : ''}`}
          onClick={(() => onClickHandle(id, value))}
        >
          <img className={styles['dice-value']} src={getDiceIconURL(value, type)}></img>
        </div>
      )
    }

    return (
      <div id={id} className={`${styles.dice} ${styles[DiceType[type]]} ${isSmall ? styles.small : ''}`}>
        <img className={styles['dice-value']} src={getDiceIconURL(value, type)}></img>
      </div>
    )
  }

  return (
    renderDice()
  );
}
