import React from "react";
import { LotteryGameResult } from "../../common/model/LotteryModel";
import EditableText from "../editable-text";
import styles from "./styles.module.scss";

type LotteryProps = {
  result: LotteryGameResult;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  isProduction: boolean;
};

export default class LotteryRoundResult extends React.Component<LotteryProps> {
  renderPrizeNumber(name, value) {
    if (this.props.isProduction) {
      return value || "?";
    }

    return value ? (
      <EditableText
        disabled={false}
        labelStyle=""
        inputStyle=""
        name={name}
        onChange={this.props.onChange}
        value={value}
        onBlur={() => {}}
      />
    ) : (
      "?"
    );
  }

  renderSecondPrizes() {
    const { result } = this.props;
    return (
      <tr>
        <td>Second</td>
        {result.second.map((number, index) => (
          <td key={index} colSpan={6}>
            {this.renderPrizeNumber(`second.${index}`, number)}
          </td>
        ))}
      </tr>
    );
  }

  renderThirdPrizes() {
    const { result } = this.props;
    return (
      <>
        <tr>
          <td rowSpan={2}>Third</td>
          {result.third.slice(0, 3).map((number, index) => (
            <td key={index} colSpan={4}>
              {this.renderPrizeNumber(`third.${index}`, number)}
            </td>
          ))}
        </tr>
        <tr>
          {result.third.slice(3, 6).map((number, index) => (
            <td key={index + 3} colSpan={4}>
              {this.renderPrizeNumber(`third.${index + 3}`, number)}
            </td>
          ))}
        </tr>
      </>
    );
  }

  renderFourthPrizes() {
    const { result } = this.props;
    return (
      <tr>
        <td>Fourth</td>
        {result.fourth.map((number, index) => (
          <td key={index} colSpan={3}>
            {this.renderPrizeNumber(`fourth.${index}`, number)}
          </td>
        ))}
      </tr>
    );
  }

  renderFifthPrizes() {
    const { result } = this.props;
    return (
      <>
        <tr>
          <td rowSpan={2}>Fifth</td>
          {result.fifth.slice(0, 3).map((number, index) => (
            <td key={index} colSpan={4}>
              {this.renderPrizeNumber(`fifth.${index}`, number)}
            </td>
          ))}
        </tr>
        <tr>
          {result.fifth.slice(3, 6).map((number, index) => (
            <td key={index + 3} colSpan={4}>
              {this.renderPrizeNumber(`fifth.${index + 3}`, number)}
            </td>
          ))}
        </tr>
      </>
    );
  }

  renderSixthPrizes() {
    const { result } = this.props;
    return (
      <tr>
        <td>Sixth</td>
        {result.sixth.map((number, index) => (
          <td key={index} colSpan={4}>
            {this.renderPrizeNumber(`sixth.${index}`, number)}
          </td>
        ))}
      </tr>
    );
  }

  renderSeventhPrizes() {
    const { result } = this.props;
    return (
      <tr>
        <td>Seventh</td>
        {result.seventh.map((number, index) => (
          <td key={index} colSpan={3} className={styles.highlight}>
            {this.renderPrizeNumber(`seventh.${index}`, number)}
          </td>
        ))}
      </tr>
    );
  }

  render() {
    const { result } = this.props;

    return (
      <table
        className={`${styles["lottery-table"]} table-bordered text-center`}
      >
        <thead>
          <tr>
            <th>Prizes</th>
            <th colSpan={12}>RNG Lottery</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Special</td>
            <td colSpan={12} className={styles.highlight}>
              {this.renderPrizeNumber("special.0", result.special[0])}
            </td>
          </tr>
          <tr>
            <td>First</td>
            <td colSpan={12}>
              {this.renderPrizeNumber("first.0", result.first[0])}
            </td>
          </tr>
          {this.renderSecondPrizes()}
          {this.renderThirdPrizes()}
          {this.renderFourthPrizes()}
          {this.renderFifthPrizes()}
          {this.renderSixthPrizes()}
          {this.renderSeventhPrizes()}
        </tbody>
      </table>
    );
  }
}
