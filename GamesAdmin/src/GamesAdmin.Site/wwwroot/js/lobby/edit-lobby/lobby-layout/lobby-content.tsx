import React, { useState } from "react";
import Modal from "react-modal";
import DragDropIconsMemo from "./drag-drop-icons";
import LobbyPreview from "./lobby-preview";
import { ReorderConfig } from "../lobby-data-types";
import { Game, Category, GameSetting } from "../lobby-data-types";

type Props = {
  gameIds: Game[];
  lobbyCategory: Category;
  onChange: (lobbyCategory: Category) => void;
};

const reorder = (list: GameSetting[], startIndex, endIndex): GameSetting[] => {
  const result = Array.from(list);
  const [removed] = result.splice(startIndex, 1);
  result.splice(endIndex, 0, removed);

  return result;
};

const customStyles = {
  content: {
    top: "40%",
    left: "50%",
    right: "auto",
    bottom: "auto",
  },
};

Modal.setAppElement("#root");

const LobbyContent: React.FC<Props> = ({
  lobbyCategory,
  onChange,
  gameIds,
}) => {
  const getAvailableGames = (editingId = 0) => {
    let result: Game[] = [];
    if (lobbyCategory && lobbyCategory.gameSettings) {
      const filter = (gameId) => {
        return (
          editingId === +gameId.value ||
          !lobbyCategory.gameSettings
            .map((game) => game.gameId)
            .includes(+gameId.value)
        );
      };

      result = gameIds.filter(filter);
    }

    return result;
  };

  const getDefaultGameIdToAdd = () => {
    const availableGameIds = getAvailableGames();
    return availableGameIds.length > 0 ? +availableGameIds[0].value : 0;
  };

  let [availableGameIds, setAvailableGameIds] = useState(getAvailableGames());
  let [editedGameSetting, setEditedGameSetting] = useState({
    gameId: getDefaultGameIdToAdd(),
    bigSize: true,
    newTag: true,
  });
  let [reOrderConfig, setReOrderConfig] = useState<ReorderConfig>({
    sourceIndex: -1,
    destinationIndex: -1,
  });
  let [activeGameId, setActiveGameId] = useState(0);

  const idEditing = () => {
    let editing = false;
    if (lobbyCategory && lobbyCategory.gameSettings) {
      editing = lobbyCategory.gameSettings
        .map((game) => game.gameId)
        .includes(editedGameSetting.gameId);
    }

    return editing;
  };

  const [modalIsOpen, setModalIsOpen] = useState(false);

  const openModal = () => {
    setModalIsOpen(true);
  };

  let subtitle;
  const afterOpenModal = () => {
    subtitle.style.color = "#f00";
  };

  const closeModal = () => {
    setModalIsOpen(false);
  };

  const onDragEnd = (result) => {
    if (!result.destination) {
      return;
    }

    const itemToSet = reorder(
      lobbyCategory.gameSettings,
      result.source.index,
      result.destination.index
    );
    lobbyCategory.gameSettings = [...itemToSet];
    setActiveGameId(0);
    onChange(lobbyCategory);
  };

  const onDragStart = (result) => {
    setActiveGameId(getActiveGameId(result.source.index));
  };

  const onDragUpdate = (result) => {
    if (!result.destination) {
      return;
    }

    setActiveGameId(getActiveGameId(result.source.index));

    setReOrderConfig({
      sourceIndex: result.source.index,
      destinationIndex: result.destination.index,
    });
  };

  const getActiveGameId = (sourceIndex) => {
    let result = 0;

    if (lobbyCategory.gameSettings && lobbyCategory.gameSettings[sourceIndex]) {
      result = lobbyCategory.gameSettings[sourceIndex].gameId;
    }

    return result;
  };

  const addHandler = () => {
    setAvailableGameIds(getAvailableGames());
    setEditedGameSetting({
      gameId: getDefaultGameIdToAdd(),
      bigSize: true,
      newTag: true,
    });
    openModal();
  };

  const saveGameSetting = () => {
    const existing = lobbyCategory.gameSettings.find(
      (s) => s.gameId === editedGameSetting.gameId
    );
    if (!existing) {
      lobbyCategory.gameSettings.push(editedGameSetting);
    }
    onChange(lobbyCategory);
    closeModal();
  };

  const deleteGameSetting = () => {
    const index = lobbyCategory.gameSettings.findIndex(
      (s) => s.gameId === editedGameSetting.gameId
    );
    if (index >= 0) {
      lobbyCategory.gameSettings.splice(index, 1);
      onChange(lobbyCategory);
    }
    closeModal();
  };

  const setGameId = (value) => {
    const existing = lobbyCategory.gameSettings.find((s) => s.gameId == value);
    if (!existing) {
      editedGameSetting.gameId = +value;
      setEditedGameSetting(editedGameSetting);
    }
  };

  const setBooleanValue = (value, name) => {
    editedGameSetting[name] = value;
    setEditedGameSetting(editedGameSetting);
  };

  const clickEditGameSetting = (gameId: number) => {
    const gameSetting = lobbyCategory.gameSettings.find(
      (gameSetting) => gameSetting.gameId === gameId
    );
    if (gameSetting) {
      setAvailableGameIds(getAvailableGames(gameSetting.gameId));
      setEditedGameSetting(gameSetting);
      openModal();
    }
  };

  const onEdit = (gameId: number) => {
    clickEditGameSetting(gameId);
  };

  return (
    <div>
      <div className="layout-icon">
        <LobbyPreview
          gameSettings={lobbyCategory?.gameSettings}
          gameIds={gameIds}
          onEdit={onEdit}
          reOrderConfig={reOrderConfig}
          activeGameId={activeGameId}
        />
        <DragDropIconsMemo
          gameSettings={lobbyCategory?.gameSettings}
          onDragEnd={onDragEnd}
          onDragUpdate={onDragUpdate}
          gameIds={gameIds}
          addHandler={addHandler}
          onDragStart={onDragStart}
        />

        <Modal
          isOpen={modalIsOpen}
          onAfterOpen={afterOpenModal}
          onRequestClose={closeModal}
          style={customStyles}
          contentLabel="Example Modal"
        >
          <h2 ref={(_subtitle) => (subtitle = _subtitle)}>Game</h2>
          <form>
            <div className="row">
              <div className="col-4 bold pt-3">Game Id: </div>
              <div className="col-8">
                <select
                  defaultValue={editedGameSetting.gameId}
                  aria-label="Default select example"
                  onChange={(e) => setGameId(e.target.value)}
                >
                  {availableGameIds.map((gameId, index) => (
                    <option key={index} value={gameId.value}>
                      {gameId.text}
                    </option>
                  ))}
                </select>
              </div>
            </div>
            <div className="row">
              <div className="col-4">
                <input
                  type="checkbox"
                  defaultChecked={editedGameSetting.bigSize}
                  onChange={(e) => setBooleanValue(e.target.checked, "bigSize")}
                />
              </div>
              <div className="col-8 bold pt-3">Big Size </div>
            </div>
            <div className="row">
              <div className="col-4">
                <input
                  type="checkbox"
                  defaultChecked={editedGameSetting.newTag}
                  onChange={(e) => setBooleanValue(e.target.checked, "newTag")}
                />
              </div>
              <div className="col-8 bold pt-3">New Tag </div>
            </div>
          </form>
          <br />
          <button onClick={saveGameSetting}>Save</button>
          &nbsp;&nbsp;
          {idEditing() && <button onClick={deleteGameSetting}>Remove</button>}
          {!idEditing() && <button onClick={closeModal}>Cancel</button>}
        </Modal>
      </div>
    </div>
  );
};

export default LobbyContent;
