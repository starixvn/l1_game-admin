import React from "react";
import Dice from "../dice/dice";
import styles from "./dire-result.module.scss";
import DiceModel from "../../common/model/DiceModel";

type DiceResultProps = {
  dices: Array<DiceModel>;
  diceClickHandle?: Function;
};

export default function DiceResult(props: DiceResultProps) {
  const { dices, diceClickHandle } = props;

  return (
    <div className={styles["dice-result"]}>
      <div className={`row ${styles["dice-frame"]}`}>
        <Dice
          id={"dice-0"}
          diceModel={dices[0] != undefined ? dices[0] : new DiceModel()}
          isSmall={false}
          onClickHandle={diceClickHandle != undefined ? diceClickHandle : null}
        />
      </div>
      <div className={`row ${styles["dice-frame"]}`}>
        {dices[1] && (
          <Dice
            id={"dice-1"}
            diceModel={dices[1]}
            isSmall={false}
            onClickHandle={
              diceClickHandle != undefined ? diceClickHandle : null
            }
          />
        )}
        {dices[2] && (
          <Dice
            id={"dice-2"}
            diceModel={dices[2]}
            isSmall={false}
            onClickHandle={
              diceClickHandle != undefined ? diceClickHandle : null
            }
          />
        )}
      </div>
    </div>
  );
}
