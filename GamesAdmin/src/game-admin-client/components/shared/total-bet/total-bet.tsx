import React from 'react';
import style from './total-bet.module.scss';

const classNames = {
  tiger: 'tigerBg',
  dragon: 'dragonBg',
  draw: 'drawBg',
  big: 'bigBg',
  small: 'smallBg',
  tai: 'bigBg',
  xiu: 'smallBg',
  tom: 'tomBg',
  cua: 'cuaBg',
  bau: 'bauBg',
  ca: 'caBg',
  nai: 'naiBg',
  ga: 'gaBg',
  motdobatrang: 'motdobatrangBg',
  mottrangbado: 'mottrangbadoBg',
  bondo: 'bondoBg',
  bontrang: 'bontrangBg',
  plateeven: 'plateevenBg',
  plateodd: 'plateoddBg',
  chan: 'chanBg',
  le: 'leBg',
  kenogold: 'kenogoldBg',
  kenowood: 'kenowoodBg',
  kenowater: 'kenowaterBg',
  kenofire: 'kenofireBg',
  kenoearth: 'kenoearthBg',
  kenobig: 'kenobigBg',
  kenosmall: 'kenosmallBg',
  kenoeven: 'kenoevenBg',
  kenoodd: 'kenooddBg',
  player: 'playerBg',
  banker: 'bankerBg',
  tie: 'tieBg',
  baccaratBig: 'baccaratBigBg',
  baccaratSmall: 'baccaratSmallBg',
  playerPair: 'playerPairBg',
  bankerPair: 'bankerPairBg',
  straightUp: 'straightUpBg',
  split: 'splitBg',
  street: 'streetBg',
  triangle: 'triangleBg',
  corner: 'cornerBg',
  fourNumbers: 'fourNumbersBg',
  line: 'lineBg',
  column: 'columnBg',
  dozen: 'dozenBg',
  redBlack: 'redBlackBg',
  oddEven: 'oddEvenBg',
  bigSmall: 'bigSmallBg',
}

export default function TotalBet(props: { type: string, total: number, isWin?: boolean }) {
  const { type, total, isWin } = props;
  const className = classNames[type];

  const amount = total.toLocaleString('en-US');
  return (
    <div className={`${style.wrapper} ${style[className]} ${isWin ? style.win : ''}`}>
      <div className={`${style.bg} `}>
        <div className={`${style.outerBg}`}></div>
        <div className={`${style.innerBg}`}></div>
      </div>
      <div className={`${style.totalText}`}>{amount}</div>
    </div>
  );
}
