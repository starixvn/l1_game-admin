import React, { useState, createContext, useEffect, } from "react";
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import { FormControl } from "react-bootstrap";
import Modal from 'react-modal';


type Props = {};

type GameCategory = {
    id: string,
    content: string
}

type GameSetting = {
    id: number;
    bigSize: boolean;
    newTag: boolean;
}

const getGameCategorySetting = (categoryId): GameSetting[] => {
    const count = 4;
    return Array.from({ length: count }, (v, k) => k).map((k, index) => ({
        id: index + 1,
        bigSize: true,
        newTag: false
    }));
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

const getItemStyle = (isDragging, draggableStyle) => ({
    // some basic styles to make the items look a bit nicer
    //userSelect: 'none',
    padding: grid * 2,
    margin: `0 ${grid}px 0 0`,

    // change background colour if dragging
    background: isDragging ? 'lightgreen' : 'grey',

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

const LobbyContent: React.FC<Props> = ({ lobbyCategory, onChange }) => {
    console.log("lobbyCategory ", lobbyCategory);
    const [items, setItems] = useState(getItems(6));
    const [category, setCategory] = useState("");
    const [lobbyCategoryState, setLobbyCategoryState] = useState(lobbyCategory);
    const [editedGameSetting, setEditedGameSetting] = useState({ gameId: 5, bigSize: true, newTag: true });

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
        setEditedGameSetting({ gameId: 5, bigSize: true, newTag: true });
    }

    const onDragEnd = (result) => {
        // dropped outside the list
        if (!result.destination) {
            return;
        }

        const itemToSet = reorder(
            lobbyCategory.gameSettings,
            result.source.index,
            result.destination.index
        );
        lobbyCategory.gameSettings = itemToSet;
        // setLobbyCategoryState(lobbyCategoryState);
        onChange(lobbyCategory);
        //setItems(itemToSet);
    }

    const addHandler = (e) => {
        console.log("add category", e);
        openModal();
    }

    const saveGameSetting = () => {
        const existing = lobbyCategory.gameSettings.find(s => s.gameId === editedGameSetting.gameId);
        if (!existing) {
            lobbyCategory.gameSettings.push(editedGameSetting);
        }
        onChange(lobbyCategory);
        console.log("save game settings ", editedGameSetting);
        closeModal();
    }

    const setGameId = (value) => {
        console.log("set game id", value);
        editedGameSetting.gameId = value;
        setEditedGameSetting(editedGameSetting);
    }

    const setBooleanValue = (value, name) => {
        editedGameSetting[name] = value;
        setEditedGameSetting(editedGameSetting);
        console.log("set editedGameSetting ", editedGameSetting);
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
                            {lobbyCategory.gameSettings.map((item, index) => (
                                <Draggable key={item.gameId.toString()} draggableId={item.gameId.toString()} index={index}>
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
                                                <span>{item.gameId}</span>
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
                <h2 ref={(_subtitle) => (subtitle = _subtitle)}>Game</h2>

                <form>
                    <div className="row">
                        <div className="col-4 bold pt-3">Game Id: </div>
                        <div className="col-8">
                            <FormControl
                                type="text"
                                defaultValue={`${editedGameSetting.gameId} 1`}
                                onChange={(e) => setGameId(e.target.value)}
                            />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-4">
                            <input
                                type="checkbox"
                                defaultChecked={false}
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
                <button onClick={closeModal}>Remove</button>
            </Modal>
        </div>
    );
};

export default LobbyContent;
