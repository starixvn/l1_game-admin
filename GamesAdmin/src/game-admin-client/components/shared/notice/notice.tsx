import React from "react";
import { Modal, ModalBody } from "reactstrap";
import Styles from "./notice.module.scss";

type Props = {
    isOpen: boolean,
    message: string
}

export const Notice = (props: Props) => (
    <div>
        <Modal isOpen={props.isOpen} className={Styles.notice}>
            <ModalBody className={Styles.body}>{props.message}</ModalBody>
        </Modal>
    </div>
);
