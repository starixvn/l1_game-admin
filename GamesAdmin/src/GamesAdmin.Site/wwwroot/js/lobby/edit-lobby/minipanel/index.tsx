import React, { useState, createContext, useEffect, } from "react";
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import { FormControl } from "react-bootstrap";
import Modal from 'react-modal';
import './index.css';

type Props = {};

type GameCategory = {
    id: string,
    content: string
}


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

const getCircleStyle = () => ({
    // background: `#f00`,
    width: `400px`,
    height: `400px`,
    borderRadius: `50%`,
    border: `solid 1px gray`,
    margin: `15px 0`,
    position: 'relative'
})

const getItemInCircleStyle = (total, index) => {
    const delta = 360 / total;
    return {
        transform: `rotate(${delta * index}deg) translateX(200px)`,
        position: `absolute`,

    }
}

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

const MiniPanel: React.FC<Props> = ({ miniPanelGameIds, onChange }) => {
    console.log("miniPanelGameIds ", miniPanelGameIds);
    let [editedGameSetting, setEditedGameSetting] = useState({ index: null, value: 1 });

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
        setEditedGameSetting({ index: null, value: 1 });
    }

    const onDragEnd = (result) => {
        // dropped outside the list
        if (!result.destination) {
            return;
        }

        const itemToSet = reorder(
            miniPanelGameIds,
            result.source.index,
            result.destination.index
        );
        miniPanelGameIds = itemToSet;
        onChange(miniPanelGameIds);
    }

    const addHandler = (e) => {
        console.log("add game id", e);
        openModal();
    }

    const saveGameSetting = () => {
        if (editedGameSetting.index == null) {
            miniPanelGameIds.push(editedGameSetting.value);
        }
        else {
            miniPanelGameIds[editedGameSetting.index] = editedGameSetting.value;
        }
        onChange(miniPanelGameIds);
        console.log("save game settings ", editedGameSetting);
        closeModal();
    }

    const setGameId = (value) => {
        console.log("set game id", value);
        editedGameSetting.value = value;
        setEditedGameSetting(editedGameSetting);
    }

    const clickEditMiniPanel = (e, index) => {
        e.stopPropagation();

        editedGameSetting = { index, value: miniPanelGameIds[index] };
        setEditedGameSetting(editedGameSetting);
        openModal();
    }

    return (
        <div>
            <div className="bold">MiniPanel Layout:</div>

            <div style={getCircleStyle()} className="circle" >
                {
                    miniPanelGameIds.map((item, index) => (
                        <div className="item" style={getItemInCircleStyle(miniPanelGameIds.length, index)} >
                            <span>
                                <span>{item}</span> &nbsp;
                                <a href="#" onClick={(e) => clickEditMiniPanel(e, index)} >edit</a>
                            </span>
                        </div>
                    ))
                }

            </div>

            <DragDropContext onDragEnd={onDragEnd}>
                <Droppable droppableId="droppable" direction="horizontal">
                    {(provided, snapshot) => (
                        <div
                            ref={provided.innerRef}
                            style={getListStyle(snapshot.isDraggingOver)}
                            {...provided.droppableProps}
                        >
                            {miniPanelGameIds.map((item, index) => (
                                <Draggable key={item.toString()} draggableId={item.toString()} index={index}>
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
                                                <span>{item}</span>
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
                                defaultValue={`${editedGameSetting.value}`}
                                onChange={(e) => setGameId(e.target.value)}
                            />
                        </div>
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

export default MiniPanel;
