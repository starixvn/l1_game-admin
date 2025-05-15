import React, { useState, useEffect, useRef } from "react";
import { FormControl } from "react-bootstrap";
import { useHistory, useParams } from "react-router-dom";
import Games from "./games";
import { LobbyApi } from "../lobby-api";
import {
  CurrencyGroup,
  DirectLobbyConfig,
  Game,
  Operator,
} from "../data-types";
const _ = require("lodash");

declare global {
  var jQuery: any;
}

const EditDirectLobby: React.FC = () => {
  const history = useHistory();
  let { id } = useParams();
  const [name, setName] = useState("");
  let [gameIds, setGameIds] = useState<number[]>([]);
  let [newGames, setNewGames] = useState<number[]>([]);
  let [gameSettings, setGameSettings] = useState<Game[]>([]);
  let [currencyGroupIds, setCurrencyGroupIds] = useState<string[]>([]);
  let [currentLobbyConfigs, setCurrentLobbyConfigs] =
    useState<DirectLobbyConfig>({} as any);
  let [message, setMessage] = useState({ msg: "", success: true });
  let [createId, setCreateId] = useState<string | null>(null);
  const [isSuperAdmin, setIsSuperAdmin] = useState(false);

  const timeout = useRef<any>();

  useEffect(() => {
    initLobbyConfig();

    return () => {
      // run when unmount
      if (timeout.current) {
        clearTimeout(timeout.current);
        timeout.current = 0;
      }
    };
  }, []);

  const initLobbyConfig = async () => {
    const res = await LobbyApi.getLobbyConfig(id);
    const { currencyGroupModels, isSuperAdmin, gameSettings, operators } = res;
    const defaultCurrencyGroupIds = currencyGroupModels.map(
      (currencyGroup) => currencyGroup.id
    );
    setCurrencyGroupIds(defaultCurrencyGroupIds);
    setIsSuperAdmin(isSuperAdmin);
    setGameSettings(gameSettings);
    if (res.lobbyConfig) {
      setName(res.lobbyConfig.name);
      setCurrencyGroupIds(res.lobbyConfig.currencyGroupIds);
      setGameIds(res.lobbyConfig.gameIds);
      setNewGames(res.lobbyConfig.newGames);

      cloneAndSetLobbyConfig(res.lobbyConfig);
    }
    initCurrencyGroupMultipleSelect(
      operators,
      currencyGroupModels,
      res.lobbyConfig?.currencyGroupIds
    );
  };

  const initCurrencyGroupMultipleSelect = (
    operators: Operator[],
    currencyGroupModels: CurrencyGroup[],
    selectedCurrencyGroupIds: string[]
  ) => {
    const filterGroups = (
      operatorId: string,
      currencyGroupModels: CurrencyGroup[]
    ) => currencyGroupModels.filter((group) => group.partnerId === operatorId);

    const isChecked = (currencyGroupId) => {
      return (
        selectedCurrencyGroupIds &&
        selectedCurrencyGroupIds.includes(currencyGroupId)
      );
    };

    const convertOperatorToItem =
      (currencyGroupModels: CurrencyGroup[]) => (operator: Operator) => {
        const operatorCurrencyGroups = filterGroups(
          operator.partnerId,
          currencyGroupModels
        );
        const operatorChecked =
          operatorCurrencyGroups
            .map((group) => isChecked(group.id))
            .filter((value) => value).length == operatorCurrencyGroups.length;

        return {
          id: operator.partnerId,
          value: operator.partnerId,
          text: operator.name,
          checked: operatorChecked,
          enable: true,
          cssClass: "partner-item",
          items: operatorCurrencyGroups.map((group) => {
            return {
              id: group.id,
              value: group.id,
              text: group.name,
              checked: isChecked(group.id),
              enable: true,
              cssClass: "group-item",
            };
          }),
        };
      };

    const data = operators
      .map(convertOperatorToItem(currencyGroupModels))
      .filter((operator) => operator.items.length);

    const isUncheckedCurrencyGroup = data.every(
      (currencyGroup) =>
        currencyGroup.checked === false &&
        isUnCheckedCurrencies(currencyGroup.items)
    );

    if (isUncheckedCurrencyGroup) {
      setCurrencyGroupIds([]);
    }

    const getAndSetSelectedCurrencyGroup = () => {
      const value = window
        .jQuery("#currency-groups")
        .fanexMultiSelect("getItems", true);
      setCurrencyGroupIds(value);
    };

    window.jQuery("#currency-groups").fanexMultiSelect({
      listData: data,
      showFilter: true,
      onLeave: function () {
        getAndSetSelectedCurrencyGroup();
      },
      onCheck: function () {
        getAndSetSelectedCurrencyGroup();
      },
    });
  };
  const isUnCheckedCurrencies = (currencies) => {
    return currencies.every(
      (currency) => currency.checked === false || currency.checked === undefined
    );
  };

  const cloneLobbyConfigs = (lobbyConfigs: DirectLobbyConfig) => {
    const clonedLobbyConfigs: any = {};
    clonedLobbyConfigs.currencyGroupIds = lobbyConfigs.currencyGroupIds.slice();
    clonedLobbyConfigs.name = lobbyConfigs.name;
    clonedLobbyConfigs.gameIds = [...lobbyConfigs.gameIds];
    clonedLobbyConfigs.newGames = [...lobbyConfigs.newGames];
    return clonedLobbyConfigs;
  };

  const cloneAndSetLobbyConfig = (lobbyConfigs: DirectLobbyConfig) => {
    setCurrentLobbyConfigs(cloneLobbyConfigs(lobbyConfigs));
  };

  const setMsg = (msg, success = false) => {
    setMessage({ msg: msg, success: success });
    timeout.current = setTimeout(() => {
      setMessage({ msg: "", success: true });
    }, 3000);
  };

  const checkDifference = (obj1: any, obj2: any) => {
    return JSON.stringify(obj1) !== JSON.stringify(obj2);
  };

  const hasChange = () => {
    if (currentLobbyConfigs.name !== name) {
      return true;
    }
    if (
      checkDifference(currencyGroupIds, currentLobbyConfigs.currencyGroupIds) ||
      checkDifference(gameIds, currentLobbyConfigs.gameIds) || 
      checkDifference(newGames, currentLobbyConfigs.newGames)
    ) {
      return true;
    }

    return false;
  };

  const validateLobbyConfig = () => {
    if (!name) {
      setMsg("Name cannot be empty!");
      return false;
    } else if (!currencyGroupIds || currencyGroupIds.length === 0) {
      setMsg("Currency Group cannot be empty!");
      return false;
    } else if (!gameIds || gameIds.length === 0) {
      setMsg("Lobby Layout cannot be empty!");
      return false;
    } else if (!hasChange()) {
      setMsg("Nothing changed!");
      return false;
    }

    return true;
  };

  const saveLobbyConfig = async () => {
    const lobbyModel = {
      id: createId || id,
      name,
      currencyGroupIds,
      gameIds,
      newGames,
    };
    const valid = validateLobbyConfig();
    if (valid) {
      const createdId = await LobbyApi.createOrUpdateLobbyConfig(lobbyModel);
      setCreateId(createdId);
      cloneAndSetLobbyConfig({
        currencyGroupIds,
        gameIds,
        name,
        newGames,
      });

      setMsg("Updated successfully!", true);
    }
  };

  const cancelLobbyConfig = () => {
    history.push("/direct-lobby");
  };

  const onChangeGameIds = (gameIds: number[]) => {
    setGameIds(gameIds);
  };

  const onChangeNewGames = (newGames: number[]) => {
    setNewGames(newGames);
  };

  return (
    <>
      <div className="form filter">
        <div className="form-row align-items-center">
          <div className="col-auto">
            <label className="control-label">Name</label>
            <FormControl
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </div>

          <div className="col-auto">
            <label className="control-label">Group</label>
            <select className="form-control" id="currency-groups"></select>
          </div>
        </div>

        <div className="btn-group" style={{ marginTop: "10px" }}>
          {isSuperAdmin && (
            <button className="btn-submit" onClick={saveLobbyConfig}>
              Save
            </button>
          )}

          <button className="btn-gray" onClick={cancelLobbyConfig}>
            Cancel
          </button>
        </div>
        <div
          className="btn-group"
          style={{
            marginTop: "10px",
            color: message.success ? "green" : "red",
          }}
        >
          <div className="col-auto">
            <span>{message.msg}</span>
          </div>
        </div>
      </div>

      <div className="card">
        <div className="card-body">
          <Games
            gameIds={gameIds}
            newGames={newGames}
            onChangeGameIds={onChangeGameIds}
            onChangeNewGames={onChangeNewGames}
            isSuperAdmin={isSuperAdmin}
            allGameSettings={gameSettings}
          />

          <div
            className="btn-group"
            style={{
              marginTop: "10px",
              color: message.success ? "green" : "red",
            }}
          >
            <div className="col-auto">
              <span>{message.msg}</span>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default EditDirectLobby;
