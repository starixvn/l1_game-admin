import React, { useEffect } from "react";
import LobbyMenu from "./lobby-menu";
import { Game, GameSetting, ReorderConfig } from "../lobby-data-types";
import { LobbyMenuColumn } from "./lobby-menu";

type Props = {
  gameSettings: GameSetting[];
  gameIds: Game[];
  reOrderConfig: ReorderConfig;
  activeGameId: number;
  onEdit: (gameId: number) => void;
};

declare global {
  var jQuery: any;
}

const getStyleNew = (isNew: boolean) => ({
  display: isNew ? `` : `none`,
});

const getGameIconStyle = (gameSetting, activeGameId) => ({
  width: `100px`,
  height: gameSetting.bigSize ? `205px` : `100px`,
  display: `flex`,
  background: gameSetting.gameId == activeGameId ? `yellow` : `#f3f7fa`,
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
  reOrderConfig,
  activeGameId,
}) => {
  const clickEditGameSetting = (e, gameSetting: GameSetting) => {
    onEdit(gameSetting.gameId);
  };

  const getListToRender = () => {
    if (gameSettings) {
      let itemToSet = gameSettings.map((gameSetting) =>
        Object.assign({}, gameSetting)
      );
      if (reOrderConfig && reOrderConfig.sourceIndex >= 0) {
        itemToSet = reorder(
          gameSettings,
          reOrderConfig.sourceIndex,
          reOrderConfig.destinationIndex
        );
      }

      const lobbyMenu = new LobbyMenu(itemToSet);
      const result = lobbyMenu.getListToRender();

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

  useEffect(() => {
    if (activeGameId) {
      const element = window.jQuery(`.icon-${activeGameId}`);
      element.parent().parent()[0].scrollLeft = element.offset().left - 400;
    }
  }, [reOrderConfig, activeGameId]);

  const Icon: React.FC<{
    gameSetting: GameSetting;
    isBottom: boolean;
  }> = ({ gameSetting, isBottom }) => {
    return (
      <span
        className={
          isBottom
            ? `lobby-icon bottom-icon icon-${gameSetting.gameId}`
            : `lobby-icon icon-${gameSetting.gameId}`
        }
        style={getGameIconStyle(gameSetting, activeGameId)}
        key={isBottom ? 1 : 0}
      >
        <span className="game-name">{getGameName(gameSetting.gameId)}</span>
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
    );
  };

  const RenderIcons: React.FC<{ item: LobbyMenuColumn; index: number }> = ({
    item,
    index,
  }) => {
    const gameSetting1 = getGameSettingById(item.ids[0]);
    const gameSetting2 = getGameSettingById(item.ids[1]);

    if (gameSetting1 && gameSetting2) {
      return (
        <span style={getBlockStyle()} key={index}>
          <Icon gameSetting={gameSetting1} isBottom={false} />
          <Icon gameSetting={gameSetting2} isBottom={true} />
        </span>
      );
    } else if (gameSetting1) {
      return (
        <span style={getBlockStyle()} key={index}>
          <Icon gameSetting={gameSetting1} isBottom={false} />
        </span>
      );
    } else {
      return <span></span>;
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
