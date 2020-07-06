import React from "react";
import CardResult from "../../common/model/CardResult";
import Card from "../../shared/card/card";
import styles from "./styles.module.scss";

type Props = {
    selectedCard: string,
    cardResult: CardResult[],
    onCardClick?: (selectedCard: string, cardIndex: number) => void
}

export default function BaccaratCardResult(props: Props) {
    const { selectedCard, cardResult, onCardClick } = props;

    return (
        <div className={styles['card-result']}>
            {cardResult.map((result, index) => (
                result &&
                (<div key={index} className={`${styles.card} ${index == 2 ? styles["third-card"] : ""}`} onClick={() => onCardClick(selectedCard, index)}>
                    <Card value={result.card} suit={result.type} />
                </div>)
            ))}
        </div>
    );
}