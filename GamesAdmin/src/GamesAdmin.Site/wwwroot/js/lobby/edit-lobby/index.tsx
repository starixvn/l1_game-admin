import React, { useState, useEffect } from "react";
import { FormControl } from "react-bootstrap";
import LobbyLayout from "./lobby-layout";
import MiniPanel from "./minipanel";
import axios from "axios";
import { useHistory, useParams } from "react-router-dom";
import { LobbyConfigProvider } from "./lobby-config-context";
import Banners from "./banners";

declare global {
  var jQuery: any;
}

const EditLobby: React.FC = () => {
  const history = useHistory();
  let { id } = useParams();

  const [name, setName] = useState("INH Lobby");
  const [isCreatePage, setIsCreatePage] = useState(true);

  let [lobbyConfig, setLobbyConfig] = useState({
    categories: [],
  });

  let [miniPanelGameIds, setMiniPanelGameIds] = useState([]);
  let [gameIds, setGameIds] = useState([]);
  let [currencyGroupIds, setCurrencyGroupIds] = useState([]);
  let [banners, setBanners] = useState([
    { gameId: 1, displayTime: 2 },
    { gameId: 2, displayTime: 3 },
  ]);

  useEffect(() => {
    initLobbyConfig();
  }, []);

  const initLobbyConfig = async () => {
    const res = await axios(`/lobby/GetLobbyConfig?id=${id}`);
    setIsCreatePage(!res.data.lobbyConfig);
    setGameIds(res.data.gameIds);
    const defaultCurrencyGroupIds = res.data.currencyGroupModels.map(
      (currencyGroup) => currencyGroup.id
    );
    setCurrencyGroupIds(defaultCurrencyGroupIds);

    if (res.data.lobbyConfig) {
      setName(res.data.lobbyConfig.name);
      setCurrencyGroupIds(res.data.lobbyConfig.currencyGroupIds);
      setMiniPanelGameIds(res.data.lobbyConfig.miniPanelGameIds);
      setBanners(res.data.lobbyConfig.banners);

      // update context categories
      lobbyConfig.categories = res.data.lobbyConfig.categories;
      setLobbyConfig({ ...lobbyConfig });
    }

    initCurrencyGroupMultipleSelect(
      res.data.partners,
      res.data.currencyGroupModels,
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

    window.jQuery("#currency-groups").fanexMultiSelect({
      listData: data,
      showFilter: true,
      onLeave: function () {
        var value = window
          .jQuery("#currency-groups")
          .fanexMultiSelect("getItems", true);
        setCurrencyGroupIds(value);
      },
    });
  };

  const onChangeMiniPanel = (miniPanelGameIds) => {
    setMiniPanelGameIds(miniPanelGameIds);
  };

  const saveLobbyConfig = async () => {
    const lobbyModel = {
      id,
      name,
      currencyGroupIds,
      categories: lobbyConfig.categories,
      miniPanelGameIds,
      banners,
    };

    await axios.post("/lobby/createorupdate", lobbyModel);

    history.push("/lobby");
  };

  const cancelLobbyConfig = () => {
    history.push("/lobby");
  };

  const onChangeCategories = (categories) => {
    lobbyConfig.categories = categories;
    setLobbyConfig({ ...lobbyConfig });
  };

  const onChangeBanner = (banners) => {
    console.log("change banner", banners);
    setBanners(banners);
  };

  return (
    <LobbyConfigProvider value={{ lobbyConfig, onChangeCategories }}>
      <form className="form filter">
        <div className="form-row align-items-center">
          <div className="col-auto">
            <label className="control-label">Name</label>
            <FormControl
              type="text"
              value={name}
              onChange={(e) => isCreatePage && setName(e.target.value)}
              disabled={!isCreatePage}
            />
          </div>

          <div className="col-auto">
            <label className="control-label">Group</label>
            <select className="form-control" id="currency-groups"></select>
          </div>
        </div>
      </form>

      <div className="card">
        <div className="card-body">
          <LobbyLayout gameIds={gameIds} />

          <MiniPanel
            miniPanelGameIds={miniPanelGameIds}
            onChange={onChangeMiniPanel}
            gameIds={gameIds}
          />

          <Banners
            banners={banners}
            onChange={onChangeBanner}
            gameIds={gameIds}
          />

          <div className="btn-group">
            <button className="btn-submit" onClick={saveLobbyConfig}>
              Save
            </button>
            <button className="btn-gray" onClick={cancelLobbyConfig}>
              Cancel
            </button>
          </div>
        </div>
      </div>
    </LobbyConfigProvider>
  );
};

export default EditLobby;
