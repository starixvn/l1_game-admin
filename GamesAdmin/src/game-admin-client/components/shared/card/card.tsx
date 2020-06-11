import React from 'react';
import styles from './card.module.scss';
import CardModel from '../../common/model/CardModel';

export default function Card(props: CardModel) {
  const { value, suit } = props;

  const suitEmojis = ['♥️', '♦️', '♣️', '♠️'];

  const getCard = value => {
    let result = '';

    switch (value) {
      case 0:
        result = 'A';
        break;
      case 12:
        result = 'K';
        break;
      case 11:
        result = 'Q';
        break;
      case 10:
        result = 'J';
        break;
      default:
        result = value + 1;
    }

    return result;
  }

  const getColor = suit => {
    if (suit === 0 || suit === 1) {
      return 'red';
    }

    return 'black';
  }

  const renderCard = (value, suit) => {
    if (value == undefined || value == -1) {
      return (
        <div className={styles.card}>
          <div className={styles.back}></div>
        </div>
      )
    }
    else {
      return (
        <div className={`${styles.card} ${styles[getColor(suit)]}`}>
          <span className={styles['card-title']}>{getCard(value)}</span>
          <div className={styles['card-suit']}>{suitEmojis[suit]}</div>
        </div>
      )
    }
  }

  return (
    renderCard(value, suit)
  );
}
