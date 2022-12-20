import React, { useState } from "react";
import LobbyMenu from "./lobby-menu";

type Props = {};

const getStyleNew = (isNew) => ({
  display: isNew ? `` : `none`,
});

const getGameIconStyle = (gameSetting) => ({
  width: `100px`,
  height: gameSetting.bigSize ? `205px` : `100px`,
  display: `flex`,
});

const getBlockStyle = () => ({
  height: `205px`,
});

const reorder = (list: any[], startIndex, endIndex): any[] => {
  const result = Array.from(list);
  const [removed] = result.splice(startIndex, 1);
  result.splice(endIndex, 0, removed);

  return result;
};

const LobbyPreview: React.FC<Props> = ({
  gameSettings,
  onEdit,
  gameIds,
  reOrder,
}) => {
  console.log("LobbyPreview reOrder", reOrder);

  const clickEditGameSetting = (e, gameSetting) => {
    onEdit(gameSetting.gameId);
  };

  const getListToRender = () => {
    if (gameSettings) {
      let itemToSet = gameSettings.map((gameSetting) =>
        Object.assign({}, gameSetting)
      );
      if (reOrder && reOrder.source) {
        itemToSet = reorder(
          gameSettings,
          reOrder.source.index,
          reOrder.destination.index
        );
      }

      const lobbyMenu = new LobbyMenu(itemToSet);
      const result = lobbyMenu.getListToRender();
      console.log("get list to render ", itemToSet, result);
      return result;
    } else {
      return [];
    }
  };

  const getGameSettingById = (id) => {
    return gameSettings.find((gameSetting) => gameSetting.gameId === id);
  };

  const getGameName = (id) => {
    return gameIds.find((gameId) => gameId.value == id)?.text;
  };

  const RenderIcons = (props) => {
    const { item, index } = props;
    if (item.bigSize) {
      const gameSetting = getGameSettingById(item.ids[0]);
      return (
        <span className="lobby-icon" style={getBlockStyle()}>
          <span style={getGameIconStyle(gameSetting)} key={index}>
            <span className="game-name">{gameSetting.gameId}</span>
            <span className="tag-new" style={getStyleNew(gameSetting.newTag)}>
              <span className="tag-new-inner">New</span>
            </span>
            <span
              className="edit-button"
              onClick={(e) => clickEditGameSetting(e, gameSetting)}
            >
              <i className="icon-edit"></i>
            </span>
          </span>
        </span>
      );
    } else {
      const gameSetting1 = getGameSettingById(item.ids[0]);
      const gameSetting2 = getGameSettingById(item.ids[1]);

      if (gameSetting2) {
        return (
          <span style={getBlockStyle()} key={index}>
            <span
              className="lobby-icon"
              style={getGameIconStyle(gameSetting1)}
              key={0}
            >
              <span className="game-name">
                {getGameName(gameSetting1.gameId)}
              </span>
              <span
                className="tag-new"
                style={getStyleNew(gameSetting1.newTag)}
              >
                <span className="tag-new-inner">New</span>
              </span>
              <span
                className="edit-button"
                onClick={(e) => clickEditGameSetting(e, gameSetting1)}
              >
                <i className="icon-edit"></i>
              </span>
            </span>

            <span
              className="lobby-icon bottom-icon"
              style={getGameIconStyle(gameSetting2)}
              key={1}
            >
              <span className="game-name">
                {getGameName(gameSetting2.gameId)}
              </span>
              <span
                className="tag-new"
                style={getStyleNew(gameSetting2.newTag)}
              >
                <span className="tag-new-inner">New</span>
              </span>
              <span
                className="edit-button"
                onClick={(e) => clickEditGameSetting(e, gameSetting2)}
              >
                <i className="icon-edit"></i>
              </span>
            </span>
          </span>
        );
      } else {
        return (
          <span style={getBlockStyle()} key={index}>
            <span
              className="lobby-icon"
              style={getGameIconStyle(gameSetting1)}
              key={0}
            >
              <span className="game-name">
                {getGameName(gameSetting1.gameId)}
              </span>
              <span
                className="tag-new"
                style={getStyleNew(gameSetting1.newTag)}
              >
                <span className="tag-new-inner">New</span>
              </span>
              <span
                className="edit-button"
                onClick={(e) => clickEditGameSetting(e, gameSetting1)}
              >
                <i className="icon-edit"></i>
              </span>
            </span>
          </span>
        );
      }
    }
  };

  return (
    <div className="layout-icon">
      <div className="preview-icon">
        <div>
          <strong>Preview Layout</strong>
        </div>
        <div className="group-icon">
          {getListToRender().map((item, index) => (
            <RenderIcons item={item} index={index} key={index} />
          ))}
        </div>
      </div>
    </div>
  );
};

export default LobbyPreview;
