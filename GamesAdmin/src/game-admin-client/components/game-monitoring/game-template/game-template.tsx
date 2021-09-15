import React, { useState } from "react";
import styles from "./game-template.module.scss";
import { Notice } from "../../shared/notice/notice";
import { SystemId } from "../../common/model/BetItem";

type Props = {
  renderMonitor: Function;
  renderBetList: Function;
  enableFilter?: boolean;
  disabled: boolean;
};

export default function GameTemplate(props: Props) {
  const { renderMonitor, renderBetList, disabled, enableFilter } = props;
  const [systemFilter, setSystemFilter] = useState(SystemId.None);

  const onChangeFilter = (event) => {
    const filterValue = +event.target.value;
    setSystemFilter(filterValue);
  };

  return (
    <div className={styles["game-template"]}>
      <div className={styles["monitor"]}>
        <div className={styles["background-img"]}></div>
        {renderMonitor(systemFilter)}
        {enableFilter && (
          <div className={styles.filter}>
            <select value={systemFilter} onChange={onChangeFilter}>
              <option value={SystemId.None}>All</option>
              <option value={SystemId.VG}>VG</option>
              <option value={SystemId.VGM}>VGM</option>
            </select>
          </div>
        )}
      </div>
      <div className={`${styles["bet-list"]}`}>
        {renderBetList(systemFilter)}
      </div>
      {disabled && <Notice isOpen={true} message="Game is disabled." />}
    </div>
  );
}
