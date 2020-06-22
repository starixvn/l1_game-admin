import React from "react";
import { Modal, ModalHeader, ModalBody, Row } from "reactstrap";
import Card from "../../shared/card/card";
import Styles from "./desk-of-cards.module.scss";

type Props = {
    modal: boolean,
    toggle: () => void,
    onPickCard: (card: number, suit: number) => void
};
export default function DeskOfCards(props: Props) {
    const { modal, toggle, onPickCard } = props;
    const TYPES = [0, 1, 2, 3];
    const CARDS = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

    return (
        <div>
            <Modal isOpen={modal} toggle={toggle} className={Styles["desk-of-cards"]}>
                <ModalHeader toggle={toggle} className={Styles["modal-header"]} ></ModalHeader>
                <ModalBody>
                    {
                        TYPES.map((type, index) => (
                            <Row className={Styles["card-row"]} key={index}>{
                                CARDS.map((card, index) => (
                                    <div key={index} className={Styles.card} onClick={() => onPickCard(card, type)}>
                                        <Card value={card} suit={type} />
                                    </div>
                                ))}
                            </Row>
                        ))
                    }
                </ModalBody>
            </Modal>
        </div>
    );
}