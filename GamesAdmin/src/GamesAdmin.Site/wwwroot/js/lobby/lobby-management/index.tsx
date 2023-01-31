import axios from "axios";
import React, { useState, useEffect } from "react";
import { Link, useHistory } from "react-router-dom";
import Modal from "react-modal";
import { bool } from "prop-types";

Modal.setAppElement("#root");

type Props = {};

type LobbyConfig = {
  name: string;
  id: string;
  currencyGroupIds: string[];
};

type LobbyManagement = {
  lobbyConfigs: LobbyConfig[];
  displayDeleteButton: boolean;
};

type CurrencyGroup = {
  value: string;
  text: string;
};

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

const LobbyManagement: React.FC<Props> = () => {
  const history = useHistory();
  const [lobbyManagement, setLobbyManagement] = useState<LobbyManagement>({
    lobbyConfigs: [],
    displayDeleteButton: true,
  });
  const [currencyGroups, setCurrencyGroups] = useState<CurrencyGroup[]>([]);
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
    const res = await axios("/lobby/getAll");
    setLobbyManagement({
      lobbyConfigs: res.data.lobbyConfigs,
      displayDeleteButton: res.data.displayDeleteButton,
    });
    setCurrencyGroups(res.data.currencyGroupItems);
    setIsSuperAdmin(res.data.isSuperAdmin);
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
    history.push(`/lobby/edit/${id}`);
  };

  const deleteLobbyConfig = async () => {
    if (idToDelete != "") {
      const result = await axios.post(`/lobby/delete?id=${idToDelete}`, {
        idToDelete,
      });
      if (result.data) {
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

  const openDeleteConfirm = (id) => {
    setIdToDelete(id);
  };

  const getLobbyConfigName = (id) => {
    return lobbyManagement.lobbyConfigs.find((config) => config.id === id)
      ?.name;
  };

  const addNewLobby = () => {
    const usedCurrencyGroups = lobbyManagement.lobbyConfigs.reduce<string[]>(
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
      history.push(`/lobby/add`);
    } else {
      openAddConfirm();
    }
  };

  const renderDeleteButton = (params: {
    displayDeleteButton: boolean;
    lobbyConfigId: string;
    isSuperAdmin: boolean;
  }) => {
    if (params.displayDeleteButton && isSuperAdmin) {
      return (
        <a
          className="btn btn-danger btn-lg"
          href="#"
          onClick={() => openDeleteConfirm(params.lobbyConfigId)}
        >
          Delete
        </a>
      );
    }

    return <></>;
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
          Add New Lobby
        </a>
      )}

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
          {lobbyManagement.lobbyConfigs.map((lobbyConfig, index) => (
            <tr key={index}>
              <td style={{ minWidth: "120px" }}>{lobbyConfig.name}</td>
              <td>{getCurrencyGroupNames(lobbyConfig.currencyGroupIds)}</td>
              <td className="text-center" style={{ minWidth: "150px" }}>
                <a
                  className="btn btn-success btn-lg"
                  href="#"
                  onClick={() => editLobbyConfig(lobbyConfig.id)}
                >
                  Edit
                </a>

                {renderDeleteButton({
                  displayDeleteButton: lobbyManagement.displayDeleteButton,
                  lobbyConfigId: lobbyConfig.id,
                  isSuperAdmin: isSuperAdmin,
                })}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

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

export default LobbyManagement;
