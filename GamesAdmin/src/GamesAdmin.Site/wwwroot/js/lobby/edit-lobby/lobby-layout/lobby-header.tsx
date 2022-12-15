import React, { useState, useContext } from "react";
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import Modal from 'react-modal';
import { FormControl } from "react-bootstrap";
import LobbyConfigContext from '../lobby-config-context';


type Props = {};

type GameCategory = {
    id: string,
    content: string
}

const reorder = (list: GameCategory[], startIndex, endIndex): GameCategory[] => {
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

    background: isEditing ? "#afd7f3" : isDragging ? 'white' : 'white',

    ...draggableStyle,
});

const getListStyle = isDraggingOver => ({
    background: isDraggingOver ? 'lightblue' : 'lightgrey',
    display: 'flex',
    padding: grid,
});


const customStyles = {
    content: {
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        marginRight: '-50%',
        transform: 'translate(-50%, -50%)',
    },
};

Modal.setAppElement('#root');


const LobbyHeader: React.FC<Props> = ({ switchCategory, editingIndex }) => {
    const lobbyConfigContext = useContext(LobbyConfigContext);
    const [editedCategory, setEditedCategory] = useState({ name: {}, gameSettings: [] });
    const categories = lobbyConfigContext.lobbyConfig.categories;

    const [modalIsOpen, setModalIsOpen] = useState(false);

    const openModal = () => {
        setModalIsOpen(true);
    }

    let subtitle;
    const afterOpenModal = () => {
        subtitle.style.color = '#f00';
    }

    const closeModal = () => {
        setModalIsOpen(false);
        setEditedCategory({ name: {}, gameSettings: [] });
    }

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
    }

    const clickEditCategory = (e, index) => {
        e.stopPropagation();
        const category = categories[index];
        category["index"] = index;
        setEditedCategory(category);
        openModal();
    }

    const switchTab = (index) => {
        switchCategory(index);
    }

    const addHandler = (e) => {
        setEditedCategory({ name: {}, gameSettings: [] });
        openModal();
    }

    const setCategoryName = (value, name) => {
        editedCategory.name[name] = value;
        setEditedCategory(editedCategory);
    }

    const saveCategory = () => {
        if (editedCategory.name["eN_US"]) {
            if (!(editedCategory["index"] >= 0)) {
                categories.push(editedCategory);
            }
            closeModal();
            console.log("saveCategory ", editedCategory);
            lobbyConfigContext.onChangeCategories(categories);
        }
    }

    const removeCategory = () => {
        if (editedCategory["index"] >= 0) {

            categories.splice(editedCategory["index"], 1);
            lobbyConfigContext.onChangeCategories(categories);
            console.log("remove ",)
        }
        closeModal();

    }

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
                            {categories.map(l => l.name).map((item, index) => (
                                <Draggable key={`${item.eN_US}-${index}`} draggableId={item.eN_US} index={index}>
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
                                                <a className="edit-button" href="#" onClick={(e) => clickEditCategory(e, index)} ><i className="icon-edit"></i></a>
                                            </span>
                                        </div>
                                    )}
                                </Draggable>
                            ))}
                            <div>                               
                                <a className="add-button" href="#" onClick={(e) => addHandler(e)}>Add Category</a>
                                
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
                <h2 ref={(_subtitle) => (subtitle = _subtitle)}>Category</h2>

                <form>
                    <div className="row">
                        <div className="col-4 bold pt-3">English: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={editedCategory.name["eN_US"]}
                                onChange={(e) => setCategoryName(e.target.value, "eN_US")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Vietnamese: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={editedCategory.name["vI_VN"]}
                                onChange={(e) => setCategoryName(e.target.value, "vI_VN")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Thai: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={editedCategory.name["tH_TH"]}
                                onChange={(e) => setCategoryName(e.target.value, "tH_TH")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Indo: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={editedCategory.name["iD_ID"]}
                                onChange={(e) => setCategoryName(e.target.value, "iD_ID")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Chinese Simplified: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={editedCategory.name["zH_CN"]}
                                onChange={(e) => setCategoryName(e.target.value, "zH_CN")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Chinese Classical: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={editedCategory.name["zH_TW"]}
                                onChange={(e) => setCategoryName(e.target.value, "zH_TW")}
                            />
                        </div>
                    </div>

                </form>

                <br />
                <button onClick={saveCategory}>Save</button>
                &nbsp; &nbsp;
                {editedCategory["index"] >= 0 && <button onClick={removeCategory}>Remove</button>}
                {!(editedCategory["index"] >= 0) && < button onClick={closeModal}>Cancel</button>}
            </Modal>
        </>
    );
};

export default LobbyHeader;
