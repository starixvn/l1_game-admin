import React, { useState, useEffect } from "react";
import { useHistory } from "react-router-dom";
import Modal from "react-modal";
import { LobbyApi } from "../lobby-api";
import { DirectLobbyConfig, SelectItem } from "../data-types";

Modal.setAppElement("#root");

const customStyles = {
  content: {
    top: "50%",
    left: "50%",
    right: "auto",
    bottom: "auto",
    marginRight: "-50%",
    transform: "translate(-50%, -50%)",
  },
};

const DirectLobbyManagement: React.FC = () => {
  const history = useHistory();
  const [lobbyManagement, setLobbyManagement] = useState<DirectLobbyConfig[]>(
    []
  );
  const [isDisplayDeleteButton, setIsDisplayDeleteButton] = useState(false);
  const [currencyGroups, setCurrencyGroups] = useState<SelectItem[]>([]);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [idToDelete, setIdToDelete] = useState("");
  const [modalConfig, setModalConfig] = useState({
    title: "",
    content: "",
    confirmHandler: () => {},
  });
  const [isSuperAdmin, setIsSuperAdmin] = useState(false);

  let subtitle;
  const afterOpenModal = () => {
    // references are now sync'd and can be accessed.
    subtitle.style.color = "#f00";
  };

  const closeModal = () => {
    setModalIsOpen(false);
    setIdToDelete("");
  };

  useEffect(() => {
    getAllLobbyConfigs();
  }, []);

  useEffect(() => {
    if (idToDelete != "") {
      setModalConfig({
        title: "Confirm",
        content: `Delete ${getLobbyConfigName(idToDelete)} ?`,
        confirmHandler: deleteLobbyConfig.bind(this),
      });
      setModalIsOpen(true);
    }
  }, [idToDelete]);

  const getAllLobbyConfigs = async () => {
    const res = await LobbyApi.getLobbies();
    setLobbyManagement(res.lobbyConfigs);
    setIsDisplayDeleteButton(res.displayDeleteButton);
    setCurrencyGroups(res.currencyGroupItems);
    setIsSuperAdmin(res.isSuperAdmin);
  };

  const getCurrencyGroupNames = (currencyGroupIds: string[]) => {
    const currencyGroupNames = currencyGroupIds
      .map((currencyGroupId) =>
        currencyGroups.find(
          (currencyGroup) => currencyGroup.value === currencyGroupId
        )
      )
      .map((currencyGroup) => currencyGroup?.text);
    return currencyGroupNames.join(", ");
  };

  const editLobbyConfig = (id) => {
    history.push(`/direct-lobby/edit/${id}`);
  };

  const deleteLobbyConfig = async () => {
    if (idToDelete != "") {
      const result = await LobbyApi.deleteLobbyConfig(idToDelete);
      if (result) {
        getAllLobbyConfigs();
      }
    }
    closeModal();
  };

  const openAddConfirm = () => {
    setModalConfig({
      title: "Confirm",
      content: `No available Group Currency`,
      confirmHandler: closeModal,
    });
    setModalIsOpen(true);
  };

  const openDeleteConfirm = (id: string | undefined) => {
    setIdToDelete(id ?? "");
  };

  const getLobbyConfigName = (id: string) => {
    return lobbyManagement.find((config) => config.id === id)?.name;
  };

  const addNewLobby = () => {
    const usedCurrencyGroups = lobbyManagement.reduce<string[]>(
      (accumulator, lobbyConfig) => {
        accumulator.push(...lobbyConfig.currencyGroupIds);
        return accumulator;
      },
      []
    );

    let allUsed = true;
    currencyGroups.forEach((currencyGroup) => {
      if (!usedCurrencyGroups.includes(currencyGroup.value)) {
        allUsed = false;
      }
    });

    if (!allUsed) {
      history.push(`/direct-lobby/add`);
    } else {
      openAddConfirm();
    }
  };

  const renderDeleteButton = (lobbyConfigId: string | undefined) => {
    return (
      isDisplayDeleteButton &&
      isSuperAdmin && (
        <a
          className="btn btn-danger btn-lg"
          href="#"
          onClick={() => openDeleteConfirm(lobbyConfigId)}
        >
          Delete
        </a>
      )
    );
  };

  return (
    <>
      {isSuperAdmin && (
        <a
          className="btn-add add-item"
          href="#"
          style={{ marginLeft: `0` }}
          onClick={() => addNewLobby()}
        >
          Add New Direct Lobby
        </a>
      )}

      {lobbyManagement?.length > 0 && (
        <table
          className="table table-bordered has-background table-medium"
          style={{ width: "auto" }}
        >
          <thead>
            <tr>
              <th>Name</th>
              <th>Group Currency</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {lobbyManagement.map((lobbyConfig, index) => (
              <tr key={index}>
                <td style={{ minWidth: "120px" }}>{lobbyConfig.name}</td>
                <td>{getCurrencyGroupNames(lobbyConfig.currencyGroupIds)}</td>
                <td className="text-center" style={{ minWidth: "150px" }}>
                  <a
                    className="btn btn-success btn-lg"
                    href="#"
                    onClick={() => editLobbyConfig(lobbyConfig.id)}
                  >
                    {isSuperAdmin ? <span>Edit</span> : <span>View</span>}
                  </a>

                  {renderDeleteButton(lobbyConfig.id)}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      <Modal
        isOpen={modalIsOpen}
        onAfterOpen={afterOpenModal}
        onRequestClose={closeModal}
        style={customStyles}
        contentLabel="Example Modal"
      >
        <h2 className="popup-title" ref={(_subtitle) => (subtitle = _subtitle)}>
          {modalConfig.title}
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
        <form className="p-3">
          <div className="text-center p-3">{modalConfig.content}</div>
          <div className="form-group text-right">
            <button
              className="btn-submit m-1"
              onClick={modalConfig.confirmHandler}
            >
              OK
            </button>
            <button className="btn-gray m-1" onClick={closeModal}>
              Cancel
            </button>
          </div>
        </form>
      </Modal>
    </>
  );
};

export default DirectLobbyManagement;
