import React from "react";
import styles from "./game-template.module.scss";
import { Notice } from "../../shared/notice/notice";
import { Partners, SystemId } from "../../common/model/BetItem";

type Props = {
  renderMonitor: Function;
  renderBetList: Function;
  enableFilter?: boolean;
  onChangeSystemFilter?: Function;
  onChangePartnerFilter?: Function;
  disabled: boolean;
  partners: Array<string>;
};

export default function GameTemplate(props: Props) {
  const { renderMonitor, renderBetList, disabled, enableFilter, partners } =
    props;

  const onChangeSystemFilter = (event) => {
    const filterValue = +event.target.value;
    props.onChangeSystemFilter(filterValue);
  };

  const onChangePartnerFilter = (event) => {
    const filterValue = event.target.value;
    props.onChangePartnerFilter(filterValue);
  };

  return (
    <div className={styles["game-template"]}>
      <div className={styles["monitor"]}>
        <div className={styles["background-img"]}></div>
        {renderMonitor()}
        {enableFilter && (
          <div className={styles.filter}>
            <div>
              <label>Partner</label>
              <select onChange={onChangePartnerFilter}>
                {partners.length > 1 && <option value={""}>All</option>}
                {partners.map((key) => (
                  <option value={key}>{Partners[key]}</option>
                ))}
              </select>
            </div>
            <div>
              <label>App Type</label>
              <select onChange={onChangeSystemFilter}>
                <option key={0} value={SystemId.None}>
                  All
                </option>
                <option key={1} value={SystemId.SC}>
                  SC
                </option>
                <option key={2} value={SystemId.SM}>
                  SM
                </option>
                <option key={3} value={SystemId.Mini}>
                  Mini SC
                </option>
              </select>
            </div>
          </div>
        )}
      </div>
      <div className={`${styles["bet-list"]}`}>{renderBetList()}</div>
      {disabled && <Notice isOpen={true} message="Game is disabled." />}
    </div>
  );
}
