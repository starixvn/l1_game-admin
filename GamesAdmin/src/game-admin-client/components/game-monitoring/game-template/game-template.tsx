import React, { useState } from "react";
import styles from "./game-template.module.scss";
import { Notice } from "../../shared/notice/notice";
import { SystemId } from "../../common/model/BetItem";

type Props = {
  renderMonitor: React.ReactElement;
  renderBetList: React.ReactElement;
  onUpdateFilter?: Function;
  disabled: boolean;
};

export default function GameTemplate(props: Props) {
  const { renderMonitor, renderBetList, disabled } = props;
  const [systemFilter, setSystemFilter] = useState(SystemId.None);

  const onChangeFilter = (event) => {
    const filterValue = +event.target.value;
    setSystemFilter(filterValue);
    props.onUpdateFilter && props.onUpdateFilter(filterValue);
  };

  return (
    <div className={styles["game-template"]}>
      <div className={styles["monitor"]}>
        <div className={styles["background-img"]}></div>
        {renderMonitor}
        {props.onUpdateFilter && (
          <div className={styles.filter}>
            <select
              value={systemFilter}
              onChange={(event) => onChangeFilter(event)}
            >
              <option value={SystemId.None}>All</option>
              <option value={SystemId.SC}>SC</option>
              <option value={SystemId.SM}>SM</option>
              <option value={SystemId.Mini}>Mini SC</option>
            </select>
          </div>
        )}
      </div>
      <div className={`${styles["bet-list"]}`}>{renderBetList}</div>
      {disabled && <Notice isOpen={true} message="Game is disabled." />}
    </div>
  );
}
