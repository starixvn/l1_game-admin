import React from 'react';
import style from './bet-table.module.scss';
import {BetChoice} from './model';
import {Col} from 'reactstrap';
import BetList from '../../../shared/bet-list/bet-list';
import {upperFirstLetter} from '../../../util/util';

type Props = {
  choices: Array<BetChoice>
};

export default function (props: Props) {
  const renderTitle = (title: string) => [<span>{upperFirstLetter(title)}</span>];

  return <>
    {props.choices.map((choice, index) => {
      const {isWin, name} = choice;

      return (
        <Col key={index} className={style.choice}>
          <BetList
            type='onecolumn'
            isWin={isWin}
            titles={renderTitle(name)}
            dataList={choice.bets}
          />
        </Col>
      );
    })}
  </>
}
