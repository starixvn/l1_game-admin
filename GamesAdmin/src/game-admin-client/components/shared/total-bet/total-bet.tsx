import React from 'react';
import style from './total-bet.module.scss';

const classNames = {
  tiger: 'tigerBg',
  dragon: 'dragonBg',
  draw: 'drawBg',
  big: 'bigBg',
  small: 'smallBg',
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
  kenoodd: 'kenooddBg'
}

export default function TotalBet(props: { type: string, total: number }) {
  const { type, total } = props;
  const className = classNames[type];

  const amount = total.toLocaleString('en-US');
  return (
    <div className={`${style[className]}`}>
      <div className={`${style.bg} `}>
        <div className={`${style.outerBg}`}></div>
        <div className={`${style.innerBg}`}></div>
      </div>
      <div className={`${style.totalText}`}>{amount}</div>
    </div>
  );
}
