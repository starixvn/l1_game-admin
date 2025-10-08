export interface NewCurrencyData {
    partnerId: string;
    currencyName: string;
    currencyCode: string;
    multiplier: number;
    isTest: boolean;
    enabled: boolean;
    fractionalOddsFormat: boolean;
    isUncountOnTotal: boolean;
}

export interface NewGroupData {
    groupName: string;
    partnerId: string;
    enabled: boolean;
    fractionalOddsFormat: boolean;
}

export interface SuggestedCurrencyGroup {
    groupId: string;
    groupName: string;
    partnerId: string;
    rate: number;
    valueDifference: number;
    isSmallerThanExpected: boolean;
}

export interface AddCurrencyToGroupProps {
    showModal: boolean;
    suggestedCurrencyGroup?: SuggestedCurrencyGroup;
    partners: string[];
    exchangeCodes: { text: string; value: string }[];
    onCurrencyAdded: () => void;
    onClose: () => void;
}

export interface CurrencyFormData {
    currencyName: string;
    currencyCode: string;
    multiplier: number;
    isTest: boolean;
    enabled: boolean;
    selectedGroupId: string;
    selectedPartnerId: string;
    useSuggestedGroup: boolean;
    newGroupName: string;
    fractionalOddsFormat: boolean;
}

export interface AddCurrencyGroupGameFormProps {
    showModal: boolean;
    suggestions: CurrencyGroupGame[];
    partners: string[];
    exchangeCodes: { text: string; value: string }[];
    onSuccess: () => void;
    onClose: () => void;
}

export interface CurrencyGroupGameFormData {
    partnerId: string;
    groupName: string;
    currencyName: string;
    currencyCode: string;
    multiplier: number;
    isTest: boolean;
    enabled: boolean;
    fractionalOddsFormat: boolean;
}
export interface ExistingRangeSuggestionResult {
    bestSuggestion?: ExistingCurrencyGroupSuggestion;
    bestSmallerSuggestion?: ExistingCurrencyGroupSuggestion;
}

export interface CurrencyGroupGame {
    gameId: number;
    gameName: string;
    minBet: number;
    maxBet: number;
    maxBetPerChoices: Record<string, number>;
    chips: string[];
    betLevels: any[];
    stakeLevels: any[];
    coinValues: any[];
    defaultCoinValue: number | null;
    defaultBetLevel: number | null;
    multiplier: number;
    maxWinPerRound: number;
}

export interface NewCurrencyGroupSuggestion {
    betLevelGames: CurrencyGroupGame[];
    coinValueGames: CurrencyGroupGame[];
    maxBetPerChoiceGames: CurrencyGroupGame[];
    noSpecialConfigGames: CurrencyGroupGame[];
}

export interface ExistingCurrencyGroupSuggestion {
    groupId: string;
    groupName: string;
    partnerId: string;
    rate: number;
    valueDifference: number;
    isSmallerThanExpected: boolean;
    gameComparisons: GameBetLimitComparison[];
}

export interface GameBetLimitComparison {
    gameId: number;
    gameName: string;
    gameDisplayName: string;
    minBetComparison: BetLimitComparison;
    maxBetComparison: BetLimitComparison;
    maxBetPerChoiceComparison: BetLimitComparison;
    minCoinValueComparison: BetLimitComparison;
    maxCoinValueComparison: BetLimitComparison;
    minBetLevelComparison: BetLimitComparison;
    maxBetLevelComparison: BetLimitComparison;
}

export interface BetLimitComparison {
    baseValue: number;
    candidateValue: number;
    expectedValue: number;
    difference: number;
    differencePercentage: number;
    isSmallerThanExpected: boolean;
}
