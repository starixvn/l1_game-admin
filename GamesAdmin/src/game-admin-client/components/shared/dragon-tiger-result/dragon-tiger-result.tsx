import React, { useState, useEffect } from "react";
import styles from "./dragon-tiger-result.module.scss";
import { Container, Row, Col } from "reactstrap";
import Card from "../card/card";
import CardResult from "../../common/model/CardResult";

export default function DragonTigerResult(props: {
  result: Array<CardResult>;
}) {
  let { result } = props;
  let card1 = result[0];
  let card2 = result[1];

  return (
    <div className={styles.dragontigerresult}>
      <div className={styles.dragonCol}>
        <div className={styles.dragon}></div>
        <Card value={card1.card} suit={card1.type} />
      </div>

      <div className={styles.line}></div>

      <div className={styles.tigerCol}>
        <div className={styles.tiger}></div>
        <Card value={card2.card} suit={card2.type} />
      </div>
    </div>
  );
}
