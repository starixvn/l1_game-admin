import React from 'react';
import style from './amount-board.module.scss'
import { BetChoice } from './model';
import { Row } from 'reactstrap';
import TotalBet from '../../../shared/total-bet/total-bet';

type Props = {
  choices: Array<BetChoice>
};

export default function (props: Props) {
  const { choices } = props;
  const topLength = 4;
  const topElements = [...choices].splice(0, topLength);
  const otherElements = [...choices].splice(topLength);
  const renderChoice = (choice: BetChoice, index: number) => {
    return (
      <TotalBet
        key={index}
        type={choice.type}
        total={choice.total()}
        isWin={choice.isWin}
      />);
  }

  return (
    <>
      {[topElements, otherElements].map((elements, index) => {
        return (
          <Row className={style.row} key={index}>
            {elements.map(renderChoice)}
          </Row>
        );
      })}
    </>
  );
}
