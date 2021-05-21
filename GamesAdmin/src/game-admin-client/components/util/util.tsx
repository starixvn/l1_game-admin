import BetItem, { SystemId } from "../common/model/BetItem";

export function upperFirstLetter(text: string) {
  return text.charAt(0).toUpperCase() + text.slice(1);
}

export const getTotalBetOnSystem = (
  betList: Array<BetItem>,
  systemFilter: SystemId
) => {
  return betList.reduce(
    (total, betInfo) =>
      betInfo.system == systemFilter ? total + betInfo.bet : total,
    0
  );
};
