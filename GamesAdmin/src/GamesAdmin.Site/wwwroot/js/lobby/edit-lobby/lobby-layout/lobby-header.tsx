import React, { useState, createContext, useEffect, } from "react";
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import Modal from 'react-modal';
import { FormControl } from "react-bootstrap";


type Props = {};

type GameCategory = {
    id: string,
    content: string
}

// fake data generator
const getItems = (count): GameCategory[] =>
    Array.from({ length: count }, (v, k) => k).map(k => ({
        id: `item-${k}`,
        content: `item ${k}`,
    }));

// a little function to help us with reordering the result
const reorder = (list: GameCategory[], startIndex, endIndex): GameCategory[] => {
    const result = Array.from(list);
    const [removed] = result.splice(startIndex, 1);
    result.splice(endIndex, 0, removed);

    return result;
};

const grid = 8;

const getItemStyle = (isDragging, draggableStyle, isEditing) => ({
    // some basic styles to make the items look a bit nicer
    //userSelect: 'none',
    padding: grid * 2,
    margin: `0 ${grid}px 0 0`,

    // change background colour if dragging
    background: isEditing ? "yellow" : isDragging ? 'lightgreen' : 'grey',

    // styles we need to apply on draggables
    ...draggableStyle,
});

const getListStyle = isDraggingOver => ({
    background: isDraggingOver ? 'lightblue' : 'lightgrey',
    display: 'flex',
    padding: grid,
    //overflow: 'auto',
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


const LobbyHeader: React.FC<Props> = ({ lobbyCategories, onChange, switchCategory, editingIndex }) => {
    console.log("lobbyCategories ", lobbyCategories);
    const [items, setItems] = useState(getItems(6));
    const [lobbyCategoriesState, setLobbyCategoriesState] = useState(lobbyCategories);
    const [editedCategory, setEditedCategory] = useState({ name: {}, gameSettings: [] });

    const [modalIsOpen, setModalIsOpen] = useState(false);

    const openModal = () => {
        setModalIsOpen(true);
    }

    let subtitle;
    const afterOpenModal = () => {
        // references are now sync'd and can be accessed.
        subtitle.style.color = '#f00';
    }

    const closeModal = () => {
        setModalIsOpen(false);
        setEditedCategory({ name: {}, gameSettings: [] });
    }


    const onDragEnd = (result) => {
        // dropped outside the list
        if (!result.destination) {
            return;
        }

        const itemToSet = reorder(
            lobbyCategoriesState,
            result.source.index,
            result.destination.index
        );
        //setItems(itemToSet);
        onChange(itemToSet);
        setLobbyCategoriesState(itemToSet);
    }

    const clickEditCategory = (e, index) => {
        e.stopPropagation();
        console.log("edit", index, lobbyCategoriesState[index]);
        const category = lobbyCategoriesState[index];
        category["index"] = index;
        setEditedCategory(category);
        openModal();
    }

    const switchTab = (index) => {
        switchCategory(index);
    }

    const addHandler = (e) => {
        console.log("add category", e);
        openModal();
    }

    const setCategoryName = (value, name) => {
        console.log("update category name ", name, value);
        editedCategory.name[name] = value;
        setEditedCategory(editedCategory);
    }

    const saveCategory = () => {
        console.log("save category ", editedCategory);
        if (!editedCategory["index"]) {
            lobbyCategoriesState.push(editedCategory);
            setLobbyCategoriesState(lobbyCategoriesState);
        }
        closeModal();
        onChange(lobbyCategoriesState);
    }

    return (
        <div>
            <DragDropContext onDragEnd={onDragEnd}>
                <Droppable droppableId="droppable" direction="horizontal">
                    {(provided, snapshot) => (
                        <div
                            ref={provided.innerRef}
                            style={getListStyle(snapshot.isDraggingOver)}
                            {...provided.droppableProps}
                        >
                            {lobbyCategoriesState.map(l => l.name).map((item, index) => (
                                <Draggable key={`${item.EN_US}-${index}`} draggableId={item.EN_US} index={index}>
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
                                                <span>{item.EN_US}</span> &nbsp;
                                                <a href="#" onClick={(e) => clickEditCategory(e, index)} >edit</a>
                                            </span>
                                        </div>
                                    )}
                                </Draggable>
                            ))}
                            <div>
                                <span>
                                    <a href="#" onClick={(e) => addHandler(e)} >Add</a>
                                </span>
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
                                defaultValue={editedCategory.name["EN_US"]}
                                onChange={(e) => setCategoryName(e.target.value, "EN_US")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Vietnamese: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={editedCategory.name["VI_VI"]}
                                onChange={(e) => setCategoryName(e.target.value, "VI_VI")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Thai: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={name}
                                onChange={(e) => setCategoryName(e.target.value, "TH_TH")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Indo: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={name}
                                onChange={(e) => setCategoryName(e.target.value, "ID_ID")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Chinese Simplified: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={name}
                                onChange={(e) => setCategoryName(e.target.value, "ZH_CN")}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4 bold pt-3">Chinese Classical: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={name}
                                onChange={(e) => setCategoryName(e.target.value, "ZH_TW")}
                            />
                        </div>
                    </div>

                </form>

                <br />
                <button onClick={saveCategory}>Save</button>
                &nbsp;&nbsp;
                <button onClick={closeModal}>Remove</button>
            </Modal>
        </div>
    );
};

export default LobbyHeader;
