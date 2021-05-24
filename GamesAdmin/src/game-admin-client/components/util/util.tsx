import BetItem, { SystemId } from "../common/model/BetItem";

export function upperFirstLetter(text: string) {
  return text.charAt(0).toUpperCase() + text.slice(1);
}

export const getTotalBetOnSystem = (
  betList: Array<BetItem>,
  systemFilter: SystemId,
  excludeUSS?: boolean
) => {
  return betList.reduce(
    (total, betInfo) =>
      betInfo.system == systemFilter &&
      (!excludeUSS || betInfo.currency.toLocaleUpperCase() != "UUS")
        ? total + betInfo.bet
        : total,
    0
  );
};
