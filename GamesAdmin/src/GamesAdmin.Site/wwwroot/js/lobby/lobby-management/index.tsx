import axios from 'axios';
import React, { useState, createContext, useEffect } from "react";
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Button from 'react-bootstrap/Button';
import { BrowserRouter as Router, Route, Link, NavLink } from "react-router-dom";

type Props = {};

const LobbyManagement: React.FC<Props> = () => {
    const [name, setName] = useState("INH Lobby");
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

    return (
        <div className="container-fluid form">
            <Container>
                <Link to="/lobby/edit" >Add New Lobby</Link>

            </Container>

            <br />

            <Container>
                <Row>
                    <Col>Name</Col>
                    <Col>Group Currency</Col>
                    <Col>Action</Col>
                </Row>
                {
                    lobbyConfigs.map((lobbyConfig, index) => <Row key={index} >
                        <Col>{lobbyConfig.name}</Col>
                        <Col>{lobbyConfig.currencyGroupIds}</Col>
                        <Col>
                            <a href="#">Edit</a>
                            <a href="#">Delete</a>
                        </Col>
                    </Row>)
                }

            </Container>
        </div>
    );
};

export default LobbyManagement;
