import React, { useState, useContext } from "react";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import Modal from "react-modal";
import { FormControl } from "react-bootstrap";
import LobbyConfigContext from "../lobby-config-context";

type Props = {};

type GameCategory = {
  id: string;
  content: string;
};

const reorder = (
  list: GameCategory[],
  startIndex,
  endIndex
): GameCategory[] => {
  const result = Array.from(list);
  const [removed] = result.splice(startIndex, 1);
  result.splice(endIndex, 0, removed);

  return result;
};

const grid = 8;

const getItemStyle = (isDragging, draggableStyle) => ({
  padding: grid * 2,
  margin: `0 ${grid}px 0 0`,
  border: `1px solid #4dacee`,

  background: "white",

  ...draggableStyle,
});

const getListStyle = (isDraggingOver) => ({
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

const Banners: React.FC<Props> = ({ banners, onChange, gameIds }) => {
  const getAvailableGames = (editingId = null) => {
    let result = [];
    if (banners) {
      const filter = (gameId) => {
        return (
          editingId === +gameId.value ||
          !banners.map((banner) => banner.gameId).includes(+gameId.value)
        );
      };

      result = gameIds.filter(filter);
    }

    console.log("result ", result);
    return result;
  };

  const getDefaultGameIdToAdd = () => {
    const availableGameIds = getAvailableGames();
    return availableGameIds.length > 0 ? +availableGameIds[0].value : 0;
  };

  let [availableGameIds, setAvailableGameIds] = useState([]);

  const [editedCategory, setEditedCategory] = useState({
    gameId: getDefaultGameIdToAdd(),
    displayTime: 0,
  });

  console.log("availableGameIds ", availableGameIds, editedCategory);

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
    setEditedCategory({ gameId: 0, displayTime: 0 });
  };

  const onDragEnd = (result) => {
    if (!result.destination) {
      return;
    }

    const itemToSet = reorder(
      banners,
      result.source.index,
      result.destination.index
    );

    onChange(itemToSet);
    //lobbyConfigContext.onChangeCategories(itemToSet);
  };

  const clickEditCategory = (e, index) => {
    e.stopPropagation();
    setAvailableGameIds(getAvailableGames(banners[index]?.gameId));

    const category = banners[index];
    category["index"] = index;
    setEditedCategory(category);
    openModal();
  };

  const addHandler = (e) => {
    setAvailableGameIds(getAvailableGames());
    setEditedCategory({ gameId: getDefaultGameIdToAdd(), displayTime: 0 });
    openModal();
  };

  const setDisplayTime = (value) => {
    editedCategory.displayTime = +value;
    setEditedCategory(editedCategory);
  };

  const setGameId = (value) => {
    if (!banners.map((banner) => banner.gameId).includes(+value)) {
      editedCategory.gameId = +value;
      setEditedCategory(editedCategory);
    }
  };

  const saveCategory = () => {
    if (!(editedCategory["index"] >= 0)) {
      banners.push(editedCategory);
    }
    closeModal();

    //lobbyConfigContext.onChangeCategories(categories);
  };

  const removeCategory = () => {
    if (editedCategory["index"] >= 0) {
      banners.splice(editedCategory["index"], 1);
      //lobbyConfigContext.onChangeCategories(categories);
    }
    closeModal();
  };

  const isEditing = () => {
    return (
      banners &&
      banners.map((banner) => banner.gameId).includes(editedCategory.gameId)
    );
  };

  const getGameName = (id) => {
    return gameIds.find((gameId) => gameId.value == id)?.text;
  };

  return (
    <div className="lobby-panel">
      <div className="title">Banner Layout</div>

      <DragDropContext onDragEnd={onDragEnd}>
        <Droppable droppableId="droppable" direction="horizontal">
          {(provided, snapshot) => (
            <div
              ref={provided.innerRef}
              style={getListStyle(snapshot.isDraggingOver)}
              {...provided.droppableProps}
            >
              {banners.map((item, index) => (
                <Draggable
                  key={`${item.gameId}-${index}`}
                  draggableId={`${item.gameId}`}
                  index={index}
                >
                  {(provided, snapshot) => (
                    <div
                      ref={provided.innerRef}
                      {...provided.draggableProps}
                      {...provided.dragHandleProps}
                      style={getItemStyle(
                        snapshot.isDragging,
                        provided.draggableProps.style
                      )}
                    >
                      <span>
                        <span>{getGameName(item.gameId)}</span> &nbsp;
                        <span>{`(${item.displayTime}s)`}</span> &nbsp;
                        <a
                          className="edit-button"
                          href="#"
                          onClick={(e) => clickEditCategory(e, index)}
                        >
                          <i className="icon-edit"></i>
                        </a>
                      </span>
                    </div>
                  )}
                </Draggable>
              ))}
              <div>
                <a
                  className="add-button"
                  href="#"
                  onClick={(e) => addHandler(e)}
                >
                  Add Banner
                </a>
              </div>
              {provided.placeholder}
            </div>
          )}
        </Droppable>
      </DragDropContext>

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
            onClick={closeModal}
          >
            <span aria-hidden="true">×</span>
          </button>
        </h2>

        <form className="popup-body form">
          <div className="form-group">
            <label className="required-field-label" htmlFor="GameID">
              Game Id
            </label>
            <select
              className="form-control"
              defaultValue={editedCategory.gameId}
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
          <div className="form-group">
            <label className="required-field-label" htmlFor="Vietnamese">
              Display Time
            </label>
            <FormControl
              type="text"
              defaultValue={editedCategory.displayTime}
              onChange={(e) => setDisplayTime(e.target.value)}
            />
          </div>
          <div className="form-group text-right">
            {isEditing() && (
              <button className="btn-gray m-1" onClick={removeCategory}>
                Remove
              </button>
            )}
            {!isEditing() && (
              <button className="btn-gray m-1" onClick={closeModal}>
                Cancel
              </button>
            )}
            <button className="btn-submit m-1" onClick={saveCategory}>
              Save
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
};

export default Banners;
