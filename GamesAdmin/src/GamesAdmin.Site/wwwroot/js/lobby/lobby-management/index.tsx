import axios from 'axios';
import React, { useState, useEffect } from "react";
import { Link, useHistory } from "react-router-dom";
import Modal from 'react-modal';

Modal.setAppElement('#root');


type Props = {};

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

const LobbyManagement: React.FC<Props> = () => {
    const history = useHistory();
    const [lobbyConfigs, setLobbyConfigs] = useState([]);
    const [currencyGroups, setCurrencyGroups] = useState([]);
    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [idToDelete, setIdToDelete] = useState("");

    let subtitle;
    const afterOpenModal = () => {
        // references are now sync'd and can be accessed.
        subtitle.style.color = '#f00';
    }

    const closeModal = () => {
        setModalIsOpen(false);
        //setEditedGameSetting({ index: null, value: 1 });
    }

    useEffect(() => {
        getAllLobbyConfigs();
    }, []);

    const getAllLobbyConfigs = async () => {
        const res = await axios('/lobby/getAll');
        setLobbyConfigs(res.data.lobbyConfigs);
        setCurrencyGroups(res.data.currencyGroupItems);
    }

    const getCurrencyGroupNames = (currencyGroupIds) => {
        const currencyGroupNames = currencyGroupIds
            .map(currencyGroupId => currencyGroups.find(currencyGroup => currencyGroup.value == currencyGroupId))
            .map(currencyGroup => currencyGroup?.text);
        return currencyGroupNames.join(", ");
    }

    const editLobbyConfig = (id) => {
        history.push(`/lobby/edit/${id}`);
    }

    const deleteLobbyConfig = async () => {
        if (idToDelete != "") {
            const result = await axios.post(`/lobby/delete?id=${idToDelete}`, { idToDelete });
            if (result.data) {
                getAllLobbyConfigs();
            }
        }
        closeModal();
    }
    const openConfirmPopup = (id) => {
        setIdToDelete(id);
        setModalIsOpen(true);
    }

    const getLobbyConfigName = (id) => {
        return lobbyConfigs.find(config => config.id === id)?.name;
    }

    return (
        <>           
            <Link to="/lobby/add" style={{ marginLeft: `0` }} className="btn-add add-item">Add New Lobby</Link>          

            <table className='table table-bordered has-background table-medium'>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Group Currency</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                {
                    lobbyConfigs.map((lobbyConfig, index) => <tr key={index} >
                        <td>{lobbyConfig.name}</td>
                        <td>{getCurrencyGroupNames(lobbyConfig.currencyGroupIds)}</td>
                        <td className='text-center'>
                            <a className='btn btn-success btn-lg' href="#" onClick={() => editLobbyConfig(lobbyConfig.id)}>Edit</a>                           
                            <a className='btn btn-danger btn-lg' href="#" onClick={() => openConfirmPopup(lobbyConfig.id)}>Delete</a>
                        </td>
                    </tr>)
                }
                </tbody>
            </table> 

            <Modal
                isOpen={modalIsOpen}
                onAfterOpen={afterOpenModal}
                onRequestClose={closeModal}
                style={customStyles}
                contentLabel="Example Modal"
            >
                <h2 ref={(_subtitle) => (subtitle = _subtitle)}>Confirm</h2>

                <form>
                    <div className="row">
                        <div className="col-12 bold pt-3">Delete {getLobbyConfigName(idToDelete)} ?</div>
                    </div>

                </form>

                <br />
                <button onClick={deleteLobbyConfig}>Ok</button>
                &nbsp;&nbsp;
                <button onClick={closeModal}>Cancel</button>
            </Modal>
        </>
    );
};

export default LobbyManagement;
