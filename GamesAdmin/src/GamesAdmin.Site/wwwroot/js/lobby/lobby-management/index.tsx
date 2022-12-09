import axios from 'axios';
import React, { useState, useEffect } from "react";
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { Link, useHistory } from "react-router-dom";


type Props = {};

const getHeaderStyle = () => ({
    background: `gray`,
    fontWeight: `bold`
}
)

const getColStyle = () => ({
    padding: `5px`,
    border: `solid 1px gray`

}
)

const getHeaderColStyle = () => ({
    textAlign: `center`,
    padding: `5px`,
}
)

const LobbyManagement: React.FC<Props> = () => {
    const history = useHistory();
    const [lobbyConfigs, setLobbyConfigs] = useState([]);
    const [currencyGroups, setCurrencyGroups] = useState([]);

    useEffect(() => {
        getAllLobbyConfigs();
    }, []);

    const getAllLobbyConfigs = async () => {
        const res = await axios('/lobby/getAll');
        console.log("res", res.data);
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

    const deleteLobbyConfig = async (id) => {
        const result = await axios.post(`/lobby/delete?id=${id}`, { id });
        if (result.data) {
            getAllLobbyConfigs();
            console.log("deleted ", id, result);
        }

    }

    return (
        <div className="container-fluid form">
            <br />
            <Link to="/lobby/add" style={{ marginLeft: `-14px` }} >Add New Lobby</Link>
            <br />
            <br />

            <div>
                <Row style={getHeaderStyle()} >
                    <Col md="3" style={getHeaderColStyle()}>Name</Col>
                    <Col md="6" style={getHeaderColStyle()}>Group Currency</Col>
                    <Col md="3" style={getHeaderColStyle()}>Action</Col>
                </Row>
                {
                    lobbyConfigs.map((lobbyConfig, index) => <Row key={index} >
                        <Col md="3" style={getColStyle()} >{lobbyConfig.name}</Col>
                        <Col md="6" style={getColStyle()}>{getCurrencyGroupNames(lobbyConfig.currencyGroupIds)}</Col>
                        <Col md="3" style={{ ...getColStyle(), textAlign: `center` }}>
                            <a href="#" onClick={() => editLobbyConfig(lobbyConfig.id)} >Edit</a>
                            &nbsp;
                            <a style={{ color: `red` }} href="#" onClick={() => deleteLobbyConfig(lobbyConfig.id)} >Delete</a>
                        </Col>
                    </Row>)
                }

            </div>
        </div>
    );
};

export default LobbyManagement;
