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
  haido: 'haidoBg',
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
  anyTriple: 'anyTripleBg',
  mountain: 'mountainBg',
  ocean: 'oceanBg',
  specificNumber: 'specificNumberBg',
  totalPoints: 'totalPointsBg',
  sicboBig: 'sicboBigBg',
  sicboSmallBg: 'sicboSmallBg'
}

export default function TotalBet(props: { type: string, total: number, isWin?: boolean }) {
  const { type, total, isWin } = props;
  const className = classNames[type] ? classNames[type] : `${type}Bg`;

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
