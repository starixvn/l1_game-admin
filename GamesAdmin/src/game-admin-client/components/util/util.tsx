import BetItem, { SystemId } from "../common/model/BetItem";

export function upperFirstLetter(text: string) {
  return text.charAt(0).toUpperCase() + text.slice(1);
}

export const updateTotalBetOfBetList = (
  betList: object,
  systemFilter?: SystemId,
  partnerFilter?: string,
  excludeUSS?: boolean
) => {
  if (systemFilter) {
    Object.keys(betList).forEach(
      (choice) =>
        (betList[choice].total = getTotalBetOnSystem(
          betList[choice].dataList,
          systemFilter,
          partnerFilter,
          excludeUSS
        ))
    );
  }
};

export const getTotalBetOnSystem = (
  betList: Array<BetItem>,
  systemFilter: SystemId,
  partnerFilter: string,
  excludeUSS?: boolean
) => {
  return betList.reduce(
    (total, betInfo) =>
      (!betInfo.system || betInfo.system == systemFilter) &&
      (!betInfo.partnerId || betInfo.partnerId == partnerFilter) &&
      (!excludeUSS || betInfo.currency.toLocaleUpperCase() != "UUS")
        ? total + betInfo.bet
        : total,
    0
  );
};
