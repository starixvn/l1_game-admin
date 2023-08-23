import React, { useState, useEffect, useRef } from "react";
import { FormControl } from "react-bootstrap";
import LobbyLayout from "./lobby-layout";
import MiniPanel from "./minipanel";
import axios from "axios";
import { useHistory, useParams } from "react-router-dom";
import { LobbyConfigProvider } from "./lobby-config-context";
import Banners from "./banners";
const _ = require("lodash");

declare global {
  var jQuery: any;
}

const EditLobby: React.FC = () => {
  const history = useHistory();
  let { id } = useParams();
  const [name, setName] = useState("");
  let [lobbyConfig, setLobbyConfig] = useState({
    categories: [],
  });

  let [miniPanelGameIds, setMiniPanelGameIds] = useState([]);
  let [gameIds, setGameIds] = useState([]);
  let [gameSettings, setGameSettings] = useState<any>({});
  let [currencyGroupIds, setCurrencyGroupIds] = useState([]);
  let [banners, setBanners] = useState([]);
  let [currentLobbyConfigs, setCurrentLobbyConfigs] = useState<any>({});
  let [message, setMessage] = useState({ msg: "", success: true });
  let [createId, setCreateId] = useState(null);
  const [isSuperAdmin, setIsSuperAdmin] = useState(false);
  let [roundTimerGame, setRoundTimerGame] = useState<any>({games: [], main: 0});

  const timeout = useRef<number>();

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
    const res = await axios(`/lobby/GetLobbyConfig?id=${id}`);
    const {
      gameIds,
      currencyGroupModels,
      isSuperAdmin,
      gameSettings,
      partners,
    } = res.data;
    setGameIds(gameIds);
    const defaultCurrencyGroupIds = currencyGroupModels.map(
      (currencyGroup) => currencyGroup.id
    );
    setCurrencyGroupIds(defaultCurrencyGroupIds);
    setIsSuperAdmin(isSuperAdmin);
    setGameSettings(gameSettings);
    if (res.data.lobbyConfig) {
      setName(res.data.lobbyConfig.name);
      setCurrencyGroupIds(res.data.lobbyConfig.currencyGroupIds);
      setMiniPanelGameIds(res.data.lobbyConfig.miniPanelGameIds);
      setBanners(res.data.lobbyConfig.banners);
      setRoundTimerGame(res.data.lobbyConfig.roundTimerGame ?? {games: [], main: 0});

      // update context categories
      lobbyConfig.categories = res.data.lobbyConfig.categories;
      setLobbyConfig({ ...lobbyConfig });
      cloneAndSetLobbyConfig(res.data.lobbyConfig);
    }
    initCurrencyGroupMultipleSelect(
      partners,
      currencyGroupModels,
      res.data.lobbyConfig?.currencyGroupIds
    );
  };

  const initCurrencyGroupMultipleSelect = (
    partners,
    currencyGroupModels,
    selectedCurrencyGroupIds
  ) => {
    const filterGroups = (partnerId, currencyGroupModels) =>
      currencyGroupModels.filter((group) => group.partnerId === partnerId);

    const isChecked = (currencyGroupId) => {
      return (
        selectedCurrencyGroupIds &&
        selectedCurrencyGroupIds.includes(currencyGroupId)
      );
    };

    const convertPartnerToItem = (currencyGroupModels) => (partner) => {
      const partnerCurrencyGroups = filterGroups(
        partner.partnerId,
        currencyGroupModels
      );
      const partnerChecked =
        partnerCurrencyGroups
          .map((group) => isChecked(group.id))
          .filter((value) => value).length == partnerCurrencyGroups.length;

      return {
        id: partner.partnerId,
        value: partner.partnerId,
        text: partner.name,
        checked: partnerChecked,
        enable: true,
        cssClass: "partner-item",
        items: partnerCurrencyGroups.map((group) => {
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

    const data = partners
      .map(convertPartnerToItem(currencyGroupModels))
      .filter((partner) => partner.items.length);

    const isUncheckedCurrencyGroup = data.every(
      (currencyGroup) =>
        currencyGroup.checked === false &&
        isUnCheckedCurrencies(currencyGroup.items)
    );

    if (isUncheckedCurrencyGroup) {
      setCurrencyGroupIds([]);
    }

    const getAndSetSelectedCurrencyGroup = () => {
      var value = window
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
  const onChangeMiniPanel = (miniPanelGameIds, roundTimerGame) => {
    setMiniPanelGameIds(miniPanelGameIds);
    setRoundTimerGame(roundTimerGame);
  };

  const cloneCategories = (categories) => {
    return categories.map((category) => {
      return {
        name: Object.assign({}, category.name),
        gameSettings: category.gameSettings.map((gameSetting) =>
          Object.assign({}, gameSetting)
        ),
      };
    });
  };

  const cloneLobbyConfigs = (lobbyConfigs) => {
    const clonedLobbyConfigs: any = {};
    clonedLobbyConfigs.currencyGroupIds = lobbyConfigs.currencyGroupIds.slice();
    clonedLobbyConfigs.miniPanelGameIds = lobbyConfigs.miniPanelGameIds.slice();
    clonedLobbyConfigs.name = lobbyConfigs.name;
    clonedLobbyConfigs.roundTimerGame = _.cloneDeep(
      lobbyConfigs.roundTimerGame
    );
    clonedLobbyConfigs.banners = lobbyConfigs.banners.map((banner) =>
      Object.assign({}, banner)
    );
    clonedLobbyConfigs.categories = cloneCategories(lobbyConfigs.categories);

    return clonedLobbyConfigs;
  };

  const cloneAndSetLobbyConfig = (lobbyConfigs) => {
    setCurrentLobbyConfigs(cloneLobbyConfigs(lobbyConfigs));
  };

  const setMsg = (msg, success = false) => {
    setMessage({ msg: msg, success: success });
    timeout.current = setTimeout(() => {
      setMessage({ msg: "", success: true });
    }, 3000);
  };

  const checkDifference = (obj1, obj2) => {
    return JSON.stringify(obj1) !== JSON.stringify(obj2);
  };

  const hasChange = () => {
    console.log(
      "difference",
      roundTimerGame,
      currentLobbyConfigs.roundTimerGame
    );
    let result = false;
    if (currentLobbyConfigs.name !== name) {
      result = true;
    } else if (
      checkDifference(currencyGroupIds, currentLobbyConfigs.currencyGroupIds) ||
      checkDifference(
        cloneCategories(lobbyConfig.categories),
        currentLobbyConfigs.categories
      ) ||
      checkDifference(miniPanelGameIds, currentLobbyConfigs.miniPanelGameIds) ||
      checkDifference(banners, currentLobbyConfigs.banners) ||
      checkDifference(roundTimerGame, currentLobbyConfigs.roundTimerGame)
    ) {
      result = true;
    }

    return result;
  };

  const validateLobbyConfig = () => {
    let result = true;
    if (!name) {
      setMsg("Name cannot be empty!");
      result = false;
    } else if (!currencyGroupIds || currencyGroupIds.length === 0) {
      setMsg("Currency Group cannot be empty!");
      result = false;
    } else if (!lobbyConfig.categories || lobbyConfig.categories.length === 0) {
      setMsg("Lobby Layout cannot be empty!");
      result = false;
    } else if (!miniPanelGameIds || miniPanelGameIds.length === 0) {
      setMsg("MiniPanel Layout cannot be empty!");
      result = false;
    } else if (!hasChange()) {
      setMsg("Nothing changed!");
      result = false;
    }

    return result;
  };

  const saveLobbyConfig = async () => {
    const lobbyModel = {
      id: createId || id,
      name,
      currencyGroupIds,
      categories: lobbyConfig.categories,
      miniPanelGameIds,
      banners,
      roundTimerGame,
    };
    const valid = validateLobbyConfig();

    if (valid) {
      const lobbyEntity = await axios.post("/lobby/createorupdate", lobbyModel);
      setCreateId(lobbyEntity.data);
      cloneAndSetLobbyConfig({
        currencyGroupIds,
        miniPanelGameIds,
        banners,
        categories: lobbyConfig.categories,
        name,
        roundTimerGame,
      });

      setMsg("Updated successfully!", true);
      // history.push("/lobby");
    }
  };

  const cancelLobbyConfig = () => {
    history.push("/lobby");
  };

  const onChangeCategories = (categories) => {
    lobbyConfig.categories = categories;
    setLobbyConfig({ ...lobbyConfig });
  };

  const onChangeBanner = (banners) => {
    setBanners(banners);
  };

  return (
    <LobbyConfigProvider
      value={{ lobbyConfig, onChangeCategories, isSuperAdmin }}
    >
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
          <LobbyLayout gameIds={gameIds} />

          <MiniPanel
            miniPanelGameIds={miniPanelGameIds}
            onChange={onChangeMiniPanel}
            gameIds={gameIds}
            isSuperAdmin={isSuperAdmin}
            roundTimerGame={roundTimerGame}
            gameSettings={gameSettings}
          />

          <Banners
            banners={banners}
            onChange={onChangeBanner}
            gameIds={gameIds}
            isSuperAdmin={isSuperAdmin}
          />

          <div className="btn-group">
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
      </div>
    </LobbyConfigProvider>
  );
};

export default EditLobby;
