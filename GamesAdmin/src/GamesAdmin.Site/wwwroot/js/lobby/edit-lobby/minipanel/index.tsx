import React, { useState, createContext, useEffect, } from "react";
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
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
    padding: `3px`,
    margin: `0 ${grid}px 0 0`,
    fontSize: `8`,
    width: `60px`,

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
        transform: `rotate(${delta * index - 90}deg) translateX(150px)`,
        position: `absolute`,
        marginTop: `-30px`,
        marginLeft: `-40px`

    }
}

const getInnerItemInCircleStyle = (total, index) => {
    const delta = 360 / total;
    return {
        transform: `rotate(${360 - delta * index + 90}deg)`,
        width: `70px`,
        height: `70px`,
        display: `block`,
        padding: `4px`,
        border: `solid 1px gray`
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

const MiniPanel: React.FC<Props> = ({ miniPanelGameIds, onChange, gameIds }) => {
    const getAvailableGames = (editingId = null) => {
        let result = [];
        if (miniPanelGameIds) {
            const filter = (gameId) => {
                return editingId === +gameId.value || !miniPanelGameIds.includes(+gameId.value);
            }

            console.log("miniPanelGameIds ", miniPanelGameIds);
            console.log("gameIds ", gameIds);

            result = gameIds.filter(filter);
            console.log("result ", result);
        }

        return result;
    }

    const getDefaultGameIdToAdd = () => {
        const availableGameIds = getAvailableGames();
        return availableGameIds.length > 0 ? +availableGameIds[0].value : 0;
    }

    let [availableGameIds, setAvailableGameIds] = useState(getAvailableGames());

    let [editedGameSetting, setEditedGameSetting] = useState({ index: -1, value: getDefaultGameIdToAdd() });

    const [modalIsOpen, setModalIsOpen] = useState(false);

    const isEditing = () => {
        return miniPanelGameIds.includes(editedGameSetting.value);
    }

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
        //setEditedGameSetting({ index: null, value: 1 });
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
        setAvailableGameIds(getAvailableGames());
        setEditedGameSetting({ index: -1, value: getDefaultGameIdToAdd() });
        openModal();
    }

    const saveGameSetting = () => {
        console.log("save ", editedGameSetting, editedGameSetting.value, miniPanelGameIds);

        if (!miniPanelGameIds.includes(editedGameSetting.value)) {
            if (editedGameSetting.index >= 0) {
                miniPanelGameIds[editedGameSetting.index] = editedGameSetting.value;
            }
            else {
                miniPanelGameIds.push(editedGameSetting.value);
            }
            console.log("change", miniPanelGameIds);
            onChange(miniPanelGameIds);
        }

        closeModal();
    }

    const deleteGame = () => {
        const index = miniPanelGameIds.findIndex(id => id == editedGameSetting.value);
        if (index >= 0) {
            miniPanelGameIds.splice(index, 1);
            onChange(miniPanelGameIds);
        }
        closeModal();
    }

    const setGameId = (value) => {
        if (!miniPanelGameIds.includes(+value)) {
            editedGameSetting.value = +value;
            setEditedGameSetting(editedGameSetting);
        }
    }

    const clickEditMiniPanel = (e, index) => {
        e.stopPropagation();

        setAvailableGameIds(getAvailableGames(miniPanelGameIds[index]));
        setEditedGameSetting({ index, value: miniPanelGameIds[index] });
        openModal();
    }

    const getGameName = (id) => {
        return gameIds.find(gameId => gameId.value == id)?.text
    }


    return (
        <div className="lobby-panel">
            <div className="title">MiniPanel Layout</div>

            <div style={getCircleStyle()} className="circle" >
                {
                    miniPanelGameIds.map((item, index) => (
                        <div className="item" style={getItemInCircleStyle(miniPanelGameIds.length, index)} key={index} >
                            <span style={getInnerItemInCircleStyle(miniPanelGameIds.length, index)} >
                                <span>{getGameName(item)}</span> &nbsp;
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
                                                <span>{getGameName(item)}</span>
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
                            <select defaultValue={editedGameSetting.value} aria-label="Default select example" onChange={(e) => setGameId(e.target.value)}>
                                {availableGameIds.map((gameId, index) => (
                                    <option key={index} value={gameId.value} >{gameId.text}</option>
                                )
                                )}

                            </select>
                        </div>
                    </div>

                </form>

                <br />
                <button onClick={saveGameSetting}>Save</button>
                &nbsp;&nbsp;
                {isEditing() && <button onClick={deleteGame}>Remove</button>}
                {!isEditing() && <button onClick={closeModal}>Cancel</button>}
            </Modal>
        </div>
    );
};

export default MiniPanel;
