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
    background: isDragging ? 'lightgreen' : '#f3f7fa',    
    ...draggableStyle,
});

const getItemInCircleStyle = (total, index) => {
    const delta = 360 / total;
    return {
        transform: `rotate(${delta * index - 90}deg) translateX(170px)`,
        position: `absolute`,
        marginTop: `-40px`,
        marginLeft: `-40px`

    }
}

const getInnerItemInCircleStyle = (total, index) => {
    const delta = 360 / total;
    return {
        transform: `rotate(${360 - delta * index + 90}deg)`,              
    }
}

const getListStyle = isDraggingOver => ({
    background: isDraggingOver ? 'lightblue' : 'lightgrey',
    display: 'flex',
    padding: grid,
});

const customStyles = {
    content: {
        top: '40%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',        
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

            <div className="circle-layout" >
                <div><strong>Preview Layout</strong></div>
                {
                    miniPanelGameIds.map((item, index) => (
                        <div className="item" style={getItemInCircleStyle(miniPanelGameIds.length, index)} key={index} >
                            <span className="item-inner" style={getInnerItemInCircleStyle(miniPanelGameIds.length, index)} >
                                <span>{getGameName(item)}</span>
                                <a className="edit-button" href="#" onClick={(e) => clickEditMiniPanel(e, index)}><i className="icon-edit"></i></a>
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
                                            className="droppable-icon"
                                        >
                                            <span className="game-name">{getGameName(item)}</span>                                            
                                        </div>
                                    )}
                                </Draggable>
                            ))}
                            <div>
                                <a className="add-button" href="#" onClick={(e) => addHandler(e)}>Add Icon</a>                               
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
                <h2 className="popup-title" ref={(_subtitle) => (subtitle = _subtitle)}>Game Icon
                    <button type="button" className="close" data-dismiss="modal" aria-label="Close" onClick={closeModal}>
                        <span aria-hidden="true">×</span>
                    </button>
                </h2>

                <form className="popup-body form">
                    <div className="form-group">
                        <label className="required-field-label" htmlFor="GameID">Game Id</label>
                        <select className="form-control" defaultValue={editedGameSetting.value} aria-label="Default select example" onChange={(e) => setGameId(e.target.value)}>
                            {availableGameIds.map((gameId, index) => (
                                <option key={index} value={gameId.value} >{gameId.text}</option>
                            )
                            )}
                        </select>
                    </div>
                    <div className="form-group text-right">
                        {isEditing() && <button className="btn-gray m-1" onClick={deleteGame}>Remove</button>}
                        {!isEditing() && <button className="btn-gray m-1" onClick={closeModal}>Cancel</button>}
                        <button className="btn-submit m-1" onClick={saveGameSetting}>Save</button>
                    </div>
                </form>
            </Modal>
        </div>
    );
};

export default MiniPanel;
