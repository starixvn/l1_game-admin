import React from "react";
import styles from "./dragon-tiger-result.module.scss";
import Card from "../card/card";
import CardResult from "../../common/model/CardResult";

export default function DragonTigerResult(props: {
  result: Array<CardResult>;
  onClickCard?: Function;
}) {
  const { result, onClickCard } = props;
  const card1 = result[0];
  const card2 = result[1];

  return (
    <div className={styles.dragontigerresult}>
      <div className={styles.dragonCol}>
        <div className={styles.dragon}></div>
        <Card
          cardName={"dragon"}
          value={card1.card}
          suit={card1.type}
          onClickHandle={onClickCard ? onClickCard : null}
        />
      </div>

      <div className={styles.line}></div>

      <div className={styles.tigerCol}>
        <div className={styles.tiger}></div>
        <Card
          cardName={"tiger"}
          value={card2.card}
          suit={card2.type}
          onClickHandle={onClickCard ? onClickCard : null}
        />
      </div>
    </div>
  );
}
