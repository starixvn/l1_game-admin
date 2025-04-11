import React, { useState } from "react";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import Modal from "react-modal";
import { Game } from "../../data-types";

const grid = 8;

const getItemStyle = (draggableStyle) => ({
  padding: grid * 2,
  margin: `0 ${grid}px 0 0`,
  border: `1px solid #4dacee`,
  background: "white",
  ...draggableStyle,
});

const getListStyle = (isDraggingOver: boolean) => ({
  background: isDraggingOver ? "lightblue" : "lightgrey",
  display: "flex",
  padding: grid,
});

const customStyles = {
  content: {
    top: "40%",
    left: "50%",
    right: "auto",
    bottom: "auto",
  },
};

Modal.setAppElement("#root");

type Props = {
  onChangeGameIds: (gameIds: number[]) => void;
  onChangeNewGames: (newGames: number[]) => void;
  allGameSettings: Game[];
  gameIds: number[];
  newGames: number[];
  isSuperAdmin: boolean;
}; 

const reorderGameIds = (
  gameIds: number[],
  sourceIndex: number,
  destinationIndex: number
): number[] => {
  const result = Array.from(gameIds);
  const [removed] = result.splice(sourceIndex, 1);
  result.splice(destinationIndex, 0, removed);
  return result;
};

const Games: React.FC<Props> = ({
  gameIds,
  newGames,
  allGameSettings,
  onChangeGameIds,
  onChangeNewGames,
  isSuperAdmin,
}) => {
  const [availableGames, setAvailableGames] = useState<Game[]>([]);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [selectedGameId, setSelectedGameId] = useState<number>(0);
  const [selectedNewGame, setSelectedNewGame] = useState<boolean>(false);
  const [editingGameId, setEditingGameId] = useState<number>(0);

  const getAvailableGames = (selectedGameId = 0): Game[] => {
    let result: Game[] = [];
    if (gameIds) {
      const filter = (gameId) => {
        return (
          selectedGameId === +gameId.gameId || !gameIds.includes(+gameId.gameId)
        );
      };

      result = allGameSettings
        .filter(filter)
        .sort((a, b) => a.gameId - b.gameId);
      if (newGames) {
        result = result.map((game) => ({
          ...game,
          newTag: newGames.includes(game.gameId),
        }));
      }
    }

    return result;
  };

  const openModal = () => {
    setModalIsOpen(true);
  };

  let subtitle;
  const afterOpenModal = () => {
    subtitle.style.color = "#f00";
  };

  const closeModal = () => {
    setModalIsOpen(false);
    setSelectedGameId(0);
    setEditingGameId(0);
    setSelectedNewGame(false);
  };

  const onDragEnd = (result) => {
    if (!result.destination) {
      return;
    }

    const itemToSet = reorderGameIds(
      gameIds,
      result.source.index,
      result.destination.index
    );

    onChangeGameIds(itemToSet);
  };

  const clickEdit = (e: React.MouseEvent<HTMLAnchorElement>, index: number) => {
    e.stopPropagation();
    setAvailableGames(getAvailableGames(gameIds[index]));
    setSelectedGameId(gameIds[index]);
    setEditingGameId(gameIds[index]);
    setSelectedNewGame(newGames.includes(gameIds[index]));
    openModal();
  };

  const addHandler = () => {
    setAvailableGames(getAvailableGames());
    setSelectedGameId(0);
    setSelectedNewGame(false);
    openModal();
  };

  const save = () => {
    const gameId = selectedGameId || getAvailableGames()[0]?.gameId;
    if (editingGameId > 0 && editingGameId !== gameId) {
      const index = gameIds.indexOf(editingGameId);
      gameIds[index] = gameId;
    } else {
      !gameIds.includes(gameId) && gameId > 0 && gameIds.push(gameId);
    }
    if (selectedNewGame) {
      if (!newGames.includes(gameId)) {
        newGames.push(gameId);
      }
    } else {
      newGames = newGames.filter((id) => id !== gameId);
    }
    onChangeNewGames(newGames);
    closeModal();
  };

  const cancel = () => {
    closeModal();
  };

  const remove = () => {
    const index = gameIds.indexOf(selectedGameId);
    const gameId = gameIds[index];
    index >= 0 && gameIds.splice(index, 1);
    if (newGames.includes(gameId)) {
      newGames = newGames.filter((id) => id !== gameId);
    }
    onChangeNewGames(newGames);
    closeModal();
  };

  const isEditing = () => {
    return editingGameId > 0;
  };

  const getGameName = (id: number) => {
    return allGameSettings.find((game) => +game.gameId === id)?.displayName;
  };

  const getStyleNew = (isNew: boolean) => ({
    display: isNew ? `` : `none`,
  });

  const getGameIconStyle = () => ({
    width: `100px`,
    height: `100px`,
    display: `flex`,
  });

  return (
    <div className="lobby-panel">
      <div className="title">Lobby Layout</div>

      <DragDropContext onDragEnd={onDragEnd}>
        <Droppable droppableId="droppable" direction="horizontal">
          {(provided, snapshot) => (
            <div
              ref={provided.innerRef}
              style={getListStyle(snapshot.isDraggingOver)}
              {...provided.droppableProps}
            >
              {gameIds.map((item, index) => (
                <Draggable
                  key={`${item}-${index}`}
                  draggableId={`${item}`}
                  index={index}
                >
                  {(provided, snapshot) => (
                    <div
                      ref={provided.innerRef}
                      {...provided.draggableProps}
                      {...provided.dragHandleProps}
                      style={getItemStyle(provided.draggableProps.style)}
                    >
                      <span className="lobby-icon" style={getGameIconStyle()}>
                        <span>{getGameName(item)}</span>
                        <span
                          className="tag-new"
                          style={getStyleNew(newGames.includes(gameIds[index]))}
                        >
                          <span className="tag-new-inner">New</span>
                        </span>
                        {isSuperAdmin && (
                          <a
                            className="edit-button"
                            href="#"
                            onClick={(e) => clickEdit(e, index)}
                          >
                            <i className="icon-edit"></i>
                          </a>
                        )}
                      </span>
                    </div>
                  )}
                </Draggable>
              ))}

              {provided.placeholder}
            </div>
          )}
        </Droppable>
      </DragDropContext>
      {isSuperAdmin && (
        <div className="p-3">
          <a className="add-button" href="#" onClick={() => addHandler()}>
            Add Game
          </a>
        </div>
      )}
      <Modal
        isOpen={modalIsOpen}
        onAfterOpen={afterOpenModal}
        onRequestClose={closeModal}
        style={customStyles}
        contentLabel="Example Modal"
      >
        <h2 className="popup-title" ref={(_subtitle) => (subtitle = _subtitle)}>
          Game
          <button
            type="button"
            className="close"
            data-dismiss="modal"
            aria-label="Close"
            onClick={cancel}
          >
            <span aria-hidden="true">×</span>
          </button>
        </h2>

        <div className="popup-body form">
          <div className="form-group">
            <label className="required-field-label" htmlFor="GameID">
              Game Name
            </label>
            <select
              className="form-control"
              defaultValue={selectedGameId}
              aria-label="Default select example"
              onChange={(e) => {
                setSelectedGameId(+e.target.value);
              }}
            >
              {availableGames.map((game, index) => (
                <option key={index} value={game.gameId}>
                  {game.displayName}
                </option>
              ))}
            </select>
          </div>
          <div className="form-group">
            <input
              type="checkbox"
              defaultChecked={selectedNewGame}
              onChange={(e) => {
                setSelectedNewGame(e.target.checked);
              }}
              name="newTag"
              id="newTag"
            />
            <label className="control-label" htmlFor="newTag">
              New Tag
            </label>
          </div>
          <div className="form-group text-right">
            {isEditing() && (
              <button className="btn-gray m-1" onClick={remove}>
                Remove
              </button>
            )}
            {!isEditing() && (
              <button className="btn-gray m-1" onClick={closeModal}>
                Cancel
              </button>
            )}
            <button className="btn-submit m-1" onClick={save}>
              Save
            </button>
          </div>
        </div>
      </Modal>
    </div>
  );
};

export default Games;
