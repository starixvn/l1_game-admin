import React, { useState, createContext, useEffect, } from "react";
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import { FormControl } from "react-bootstrap";
import Modal from 'react-modal';
import LobbyMenu from "./lobby-menu";


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

const getStyleNew = (isNew) => ({
    display: isNew ? `` : `none`,
    color: `red`
})

const getGameIconStyle = (gameSetting) => ({
    width: `80px`,
    height: gameSetting.bigSize ? `160px` : `80px`,
    border: `solid 1px`,
    display: `block`,

})

const getBlockStyle = () => ({
    height: `160px`,
    marginRight: `10px`,
    display: 'table-cell'
}
)


Modal.setAppElement('#root');

const LobbyContent: React.FC<Props> = ({ lobbyCategory, onChange, gameIds }) => {
    console.log("lobbyCategory ", lobbyCategory, gameIds);
    let [editedGameSetting, setEditedGameSetting] = useState({ gameId: 5, bigSize: true, newTag: true });

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

        console.log("before ", lobbyCategory.gameSettings);
        const itemToSet = reorder(
            lobbyCategory.gameSettings,
            result.source.index,
            result.destination.index
        );
        lobbyCategory.gameSettings = [...itemToSet];
        console.log("after ", lobbyCategory.gameSettings);
        onChange(lobbyCategory);

        setEditedGameSetting({ gameId: 5, bigSize: true, newTag: true });
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
        //console.log("save game settings ", editedGameSetting);
        closeModal();
    }

    const setGameId = (value) => {
        console.log("set game id", value);
        editedGameSetting.gameId = +value;
        setEditedGameSetting(editedGameSetting);
    }

    const setBooleanValue = (value, name) => {
        editedGameSetting[name] = value;
        setEditedGameSetting(editedGameSetting);
        console.log("set editedGameSetting ", editedGameSetting);
    }

    const clickEditGameSetting = (e, gameSetting) => {
        setEditedGameSetting(gameSetting);
        openModal();
    }

    const getListToRender = () => {
        const lobbyMenu = new LobbyMenu(lobbyCategory.gameSettings);
        return lobbyMenu.getListToRender();
    }

    const getGameSettingById = (id) => {
        return lobbyCategory.gameSettings.find(gameSetting => gameSetting.gameId === id);
    }

    const getGameName = (id) => {
        return gameIds.find(gameId => gameId.value == id)?.text
    }

    const RenderIcons = (props) => {
        const { item, index } = props;
        if (item.bigSize) {
            const gameSetting = getGameSettingById(item.ids[0]);
            return <span style={getBlockStyle()} >
                <span style={getGameIconStyle(gameSetting)} key={index} >
                    <span>{gameSetting.gameId}</span>
                    <span style={getStyleNew(gameSetting.newTag)} >[new]</span>
                    <br />
                    <a href="#" onClick={(e) => clickEditGameSetting(e, gameSetting)} >edit</a>
                    &nbsp;
                    &nbsp;
                </span>
            </span>;
        }
        else {
            const gameSetting1 = getGameSettingById(item.ids[0]);
            const gameSetting2 = getGameSettingById(item.ids[1]);

            if (gameSetting2) {
                return <span style={getBlockStyle()} key={index}>
                    <span style={getGameIconStyle(gameSetting1)} key={0} >
                        <span>{getGameName(gameSetting1.gameId)}</span>
                        <span style={getStyleNew(gameSetting1.newTag)} >[new]</span>
                        <br />
                        <a href="#" onClick={(e) => clickEditGameSetting(e, gameSetting1)} >edit</a>
                        &nbsp;
                        &nbsp;
                    </span>
                    <span style={getGameIconStyle(gameSetting2)} key={1} >
                        <span>{getGameName(gameSetting2.gameId)}</span>
                        <span style={getStyleNew(gameSetting2.newTag)} >[new]</span>
                        <br />
                        <a href="#" onClick={(e) => clickEditGameSetting(e, gameSetting2)} >edit</a>
                        &nbsp;
                        &nbsp;
                    </span>
                </span>
            }
            else {
                return <span style={getBlockStyle()} key={index}>
                    <span style={getGameIconStyle(gameSetting1)} key={0} >
                        <span>{getGameName(gameSetting1.gameId)}</span>
                        <span style={getStyleNew(gameSetting1.newTag)} >[new]</span>
                        <br />
                        <a href="#" onClick={(e) => clickEditGameSetting(e, gameSetting1)} >edit</a>
                        &nbsp;
                        &nbsp;
                    </span>
                </span>
            }
        }
    }

    return (
        <div style={{ border: `solid 1px gray`, padding: `10px` }} >
            <br />

            <div style={{ display: "table" }} >{getListToRender().map((item, index) => (
                <RenderIcons item={item} index={index} />
            ))}</div>

            <br />

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
                                                <span>{getGameName(item.gameId)}</span>
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
                            <select defaultValue={editedGameSetting.gameId} aria-label="Default select example" onChange={(e) => setGameId(e.target.value)}>
                                {gameIds.map((gameId, index) => (
                                    <option key={index} value={gameId.value} >{gameId.text}</option>
                                )
                                )}

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
                <button onClick={closeModal}>Remove</button>
            </Modal>
        </div>
    );
};

export default LobbyContent;
