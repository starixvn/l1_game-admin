import React from "react";
import CardResult from "../../common/model/CardResult";
import Card from "../../shared/card/card";
import styles from "../baccarat/styles.module.scss";

type Props = {
  selectedCard: string;
  cardResult: CardResult;
  onCardClick?: (selectedCard: string) => void;
};

export default function DragonTigerCardResult(props: Props) {
  const { selectedCard, cardResult, onCardClick } = props;

  return (
    <div className={styles["card-result"]}>
      {cardResult && (
        <div
          className={`${styles.card}`}
          onClick={() => onCardClick(selectedCard)}
        >
          <Card value={cardResult.card} suit={cardResult.type} />
        </div>
      )}
    </div>
  );
}
