import { ExistingRangeSuggestionResult, CurrencyGroupGame, NewGroupData, NewCurrencyGroupSuggestion } from "./types";

export interface CurrencyGroup {
    id: string;
    name: string;
    enabled: boolean;
    partnerId: string;
    fractionalOddsFormat: boolean;
    currencies: Currency[];
}

export interface Currency {
    id: string;
    name: string;
    code: string;
    multiplier: number;
    enabled: boolean;
    isTest: boolean;
    partnerId: string;
    groupId: string;
    isUncountOnTotal: boolean;
}

export interface GetBetLimitSuggestionRequest {
    currencyRate: number;
    baseCurrencyGroupId: string;
    candidateCurrencyGroupIds: string[];
}

export interface CreateCurrencyRequest {
    name: string;
    code: string;
    multiplier: number;
    enabled: boolean;
    isTest: boolean;
    partnerId: string;
    groupId: string;
    isUncountOnTotal: boolean;
}

export interface CloneCurrencyGroupRequest {
    name: string;
    partnerId: string;
    enabled: boolean;
    fractionalOddsFormat: boolean;
    sourceGroupId: string;
}

export interface MessageResult {
    success: boolean;
    message: string;
}

export class IntegrateNewCurrencyApi {
    private static readonly BASE_URL = "/betLimitSuggestion/currentInfoSystem";
    private static readonly SUGGESTION_URL = "/betLimitSuggestion/getBetLimitSuggestion";
    private static readonly NEW_RANGE_SUGGESTION_URL = "/betLimitSuggestion/getNewRangeBetLimitSuggestion";
    private static readonly PARTNERS_AND_EXCHANGE_CODES_URL = "/betLimitSuggestion/getPartnersAndExchangeCodes";
    private static readonly CREATE_CURRENCY_URL = "/currency/create";
    private static readonly CLONE_CURRENCY_GROUP_URL = "/currencyGroup/clone";
    private static readonly GET_CURRENCY_GROUP_GAMES_URL = "/currencyGroupGame/GetByGroup";
    private static readonly ADD_RANGE_CURRENCY_GROUP_GAME_URL = "/currencyGroupGame/AddRange";

    /**
     * Get all partners/operators
     */
    static async getCurrentSystemInfo(): Promise<{
        currencyGroups: CurrencyGroup[];
    }> {
        try {
            const response = await fetch(this.BASE_URL, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error("Error fetching partners:", error);
            throw error;
        }
    }

    static async getBetLimitSuggestion(request: GetBetLimitSuggestionRequest): Promise<ExistingRangeSuggestionResult> {
        try {
            const response = await fetch(this.SUGGESTION_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(request),
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error("Error getting bet limit suggestions:", error);
            throw error;
        }
    }

    static async getNewRangeBetLimitSuggestion(request: GetBetLimitSuggestionRequest): Promise<NewCurrencyGroupSuggestion> {
        try {
            const response = await fetch(this.NEW_RANGE_SUGGESTION_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(request),
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error("Error getting new range bet limit suggestions:", error);
            throw error;
        }
    }

    static async getPartnersAndExchangeCodes(): Promise<{
        partners: { text: string; value: string }[];
        codes: { text: string; value: string }[];
    }> {
        try {
            const response = await fetch(this.PARTNERS_AND_EXCHANGE_CODES_URL, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error("Error getting partners and exchange codes:", error);
            throw error;
        }
    }

    static async createCurrency(currency: CreateCurrencyRequest): Promise<MessageResult> {
        try {
            const params = new URLSearchParams();
            params.append("name", currency.name);
            params.append("code", currency.code);
            params.append("multiplier", currency.multiplier.toString());
            params.append("enabled", currency.enabled.toString());
            params.append("isTest", currency.isTest.toString());
            params.append("partnerId", currency.partnerId);
            params.append("groupId", currency.groupId);
            params.append("isUncountOnTotal", currency.isUncountOnTotal.toString());

            const response = await fetch(`${this.CREATE_CURRENCY_URL}?${params.toString()}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error("Error creating currency:", error);
            throw error;
        }
    }

    static async cloneCurrencyGroup(request: CloneCurrencyGroupRequest): Promise<CurrencyGroup | null> {
        try {
            const params = new URLSearchParams();
            params.append("name", request.name);
            params.append("partnerId", request.partnerId);
            params.append("enabled", request.enabled.toString());
            params.append("fractionalOddsFormat", request.fractionalOddsFormat.toString());
            params.append("sourceGroupId", request.sourceGroupId);

            const response = await fetch(`${this.CLONE_CURRENCY_GROUP_URL}?${params.toString()}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            return result;
        } catch (error) {
            console.error("Error cloning currency group:", error);
            throw error;
        }
    }

    static async getCurrencyGroupsByPartner(partnerId: string): Promise<CurrencyGroup[]> {
        try {
            const response = await fetch(`/api/currency_group/partner/${partnerId}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error("Error fetching currency groups by partner:", error);
            throw error;
        }
    }

    static async getCurrencyGroupGamesByGroupId(groupId: string): Promise<CurrencyGroupGame[]> {
        try {
            const response = await fetch(`${this.GET_CURRENCY_GROUP_GAMES_URL}?groupId=${groupId}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error("Error fetching currency group games by groupId:", error);
            throw error;
        }
    }

    static async addRangeCurrencyGroupGames(currencyGroupGames: CurrencyGroupGame[]): Promise<boolean> {
        try {
            const response = await fetch(this.ADD_RANGE_CURRENCY_GROUP_GAME_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(currencyGroupGames),
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error("Error adding range of currency group games:", error);
            throw error;
        }
    }
}
