import React, { useState, useContext } from "react";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import Modal from "react-modal";
import { FormControl } from "react-bootstrap";
import LobbyConfigContext from "../lobby-config-context";
import { Category } from "../lobby-data-types";

type Props = {
  switchCategory: (index: number) => void;
  editingIndex: number;
};

const reorder = (list: Category[], startIndex, endIndex): Category[] => {
  const result = Array.from(list);
  const [removed] = result.splice(startIndex, 1);
  result.splice(endIndex, 0, removed);

  return result;
};

const grid = 8;

const getItemStyle = (isDragging, draggableStyle, isEditing) => ({
  padding: grid * 2,
  margin: `0 ${grid}px 0 0`,
  border: `1px solid #4dacee`,

  background: isEditing ? "#afd7f3" : isDragging ? "white" : "white",

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

const LobbyHeader: React.FC<Props> = ({ switchCategory, editingIndex }) => {
  const lobbyConfigContext = useContext(LobbyConfigContext);
  const [editedCategory, setEditedCategory] = useState<Category>({
    name: { eN_US: "", vI_VN: "", tH_TH: "", iD_ID: "", zH_CN: "", zH_TW: "" },
    gameSettings: [],
  });
  const [editedNames, setEditedNames] = useState({
    eN_US: "",
    vI_VN: "",
    tH_TH: "",
    iD_ID: "",
    zH_CN: "",
    zH_TW: "",
  });
  const categories = lobbyConfigContext.lobbyConfig.categories;

  const [modalIsOpen, setModalIsOpen] = useState(false);
  let [message, setMessage] = useState("");

  const openModal = () => {
    setModalIsOpen(true);
  };

  let subtitle;
  const afterOpenModal = () => {
    subtitle.style.color = "#f00";
  };

  const closeModal = () => {
    setModalIsOpen(false);
    setEditedCategory({
      name: {
        eN_US: "",
        vI_VN: "",
        tH_TH: "",
        iD_ID: "",
        zH_CN: "",
        zH_TW: "",
      },
      gameSettings: [],
    });
    setMessage("");
  };

  const onDragEnd = (result) => {
    if (!result.destination) {
      return;
    }

    const itemToSet = reorder(
      categories,
      result.source.index,
      result.destination.index
    );

    lobbyConfigContext.onChangeCategories(itemToSet);
  };

  const cancel = () => {
    if (JSON.stringify(editedNames) !== JSON.stringify(editedCategory.name)) {
      editedCategory.name = editedNames;
      setEditedCategory(editedCategory);
    }

    closeModal();
  };

  const clickEditCategory = (e, index) => {
    e.stopPropagation();
    const category = categories[index];
    category["index"] = index;
    setEditedNames(Object.assign({}, category.name));
    setEditedCategory(category);
    openModal();
  };

  const switchTab = (index) => {
    switchCategory(index);
  };

  const addHandler = (e) => {
    setEditedCategory({
      name: {
        eN_US: "",
        vI_VN: "",
        tH_TH: "",
        iD_ID: "",
        zH_CN: "",
        zH_TW: "",
      },
      gameSettings: [],
    });
    openModal();
  };

  const setCategoryName = (value, name) => {
    editedCategory.name[name] = value;
    setEditedCategory(editedCategory);
  };

  const saveCategory = () => {
    if (editedCategory.name["eN_US"]) {
      if (!(editedCategory["index"] >= 0)) {
        categories.push(editedCategory);
      }
      closeModal();

      lobbyConfigContext.onChangeCategories(categories);
      setMessage("");
    } else {
      setMessage("English Name cannot be empty!");
    }
  };

  const removeCategory = () => {
    if (editedCategory["index"] >= 0) {
      categories.splice(editedCategory["index"], 1);
      lobbyConfigContext.onChangeCategories(categories);
    }
    closeModal();
  };

  return (
    <>
      <DragDropContext onDragEnd={onDragEnd}>
        <Droppable droppableId="droppable" direction="horizontal">
          {(provided, snapshot) => (
            <div
              ref={provided.innerRef}
              style={getListStyle(snapshot.isDraggingOver)}
              {...provided.droppableProps}
            >
              {categories
                .map((l) => l.name)
                .map((item, index) => (
                  <Draggable
                    key={`${item.eN_US}-${index}`}
                    draggableId={item.eN_US}
                    index={index}
                  >
                    {(provided, snapshot) => (
                      <div
                        ref={provided.innerRef}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}
                        style={getItemStyle(
                          snapshot.isDragging,
                          provided.draggableProps.style,
                          editingIndex === index
                        )}
                        onClick={() => switchTab(index)}
                      >
                        <span>
                          <span>{item.eN_US}</span> &nbsp;
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
                  Add Category
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
          Category
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
            <label className="required-field-label" htmlFor="English">
              English <span style={{ color: "red" }}>(*)</span>
            </label>
            <FormControl
              type="text"
              defaultValue={editedCategory.name["eN_US"]}
              onChange={(e) => setCategoryName(e.target.value, "eN_US")}
              className="form-control"
            />
          </div>

          <div className="form-group">
            <label className="required-field-label" htmlFor="Vietnamese">
              Vietnamese
            </label>
            <FormControl
              type="text"
              defaultValue={editedCategory.name["vI_VN"]}
              onChange={(e) => setCategoryName(e.target.value, "vI_VN")}
            />
          </div>

          <div className="form-group">
            <label className="required-field-label" htmlFor="Thai">
              Thai
            </label>
            <FormControl
              type="text"
              defaultValue={editedCategory.name["tH_TH"]}
              onChange={(e) => setCategoryName(e.target.value, "tH_TH")}
            />
          </div>

          <div className="form-group">
            <label className="required-field-label" htmlFor="Indo">
              Indo
            </label>
            <FormControl
              type="text"
              defaultValue={editedCategory.name["iD_ID"]}
              onChange={(e) => setCategoryName(e.target.value, "iD_ID")}
            />
          </div>

          <div className="form-group">
            <label className="required-field-label" htmlFor="CN">
              Chinese Simplified
            </label>
            <FormControl
              type="text"
              defaultValue={editedCategory.name["zH_CN"]}
              onChange={(e) => setCategoryName(e.target.value, "zH_CN")}
            />
          </div>

          <div className="form-group">
            <label className="required-field-label" htmlFor="TW">
              Chinese Original
            </label>
            <FormControl
              type="text"
              defaultValue={editedCategory.name["zH_TW"]}
              onChange={(e) => setCategoryName(e.target.value, "zH_TW")}
            />
          </div>
          <div
            className="btn-group"
            style={{
              marginTop: "10px",
              color: "red",
            }}
          >
            <div className="col-auto">
              <span>{message}</span>
            </div>
          </div>

          <div className="form-group text-right">
            {editedCategory["index"] >= 0 && (
              <button className="btn-gray m-1" onClick={removeCategory}>
                Remove
              </button>
            )}
            {!(editedCategory["index"] >= 0) && (
              <button className="btn-gray m-1" onClick={cancel}>
                Cancel
              </button>
            )}
            <button className="btn-submit m-1" onClick={saveCategory}>
              Save
            </button>
          </div>
        </div>
      </Modal>
    </>
  );
};

export default LobbyHeader;
