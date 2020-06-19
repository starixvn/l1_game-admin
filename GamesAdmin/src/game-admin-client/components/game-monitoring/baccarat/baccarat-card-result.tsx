import React from "react";
import CardResult from "../../common/model/CardResult";
import Card from "../../shared/card/card";
import styles from "./styles.module.scss";

export default function BaccaratCardResult(props: { cardResult: CardResult[] }) {
    const { cardResult } = props;

    return (
        <div className={styles['card-result']}>
            {cardResult.map((result, index) => (<div key={index} className={styles.card}><Card value={result.card} suit={result.type} /></div>))}
        </div>
    );
}