import React from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Row, Col } from 'reactstrap';
import styles from './result-updater.module.scss'

type Props = {
    modal: boolean,
    toggle: Function,
    submit: Function
};

type State = {
    modal: boolean,
    errors: string,
};

const COLUMNS = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

export default class RouletteResultUpdater extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        this.state = { modal: false, errors: "" }

        this.toggle = this.toggle.bind(this);
    }

    toggle() {
        this.props.toggle();
    }

    submit(number) {
        this.props.submit(number);
    }

    render() {
        let index = 0;
        return (
            <div>
                <Modal isOpen={this.props.modal} toggle={this.toggle} className={styles["result-updater"]}>
                    <ModalHeader toggle={this.toggle} className={styles["modal-header"]}></ModalHeader>
                    <ModalBody>
                        <Row>
                            <Col className={styles.number} onClick={this.submit.bind(this, 0)}>0</Col>
                            {COLUMNS.map((col) => <Col key={`col-${col}`} className={styles.column}>
                                {[1, 2, 3].map(row => {
                                    index++;
                                    return <Row key={`row-${col}-${row}`} className={styles.number} onClick={this.submit.bind(this, index)}>{index}</Row>
                                })}
                            </Col>)}
                        </Row>
                    </ModalBody>
                    <ModalFooter className={styles["modal-footer"]}>
                    </ModalFooter>
                </Modal>
            </div>
        )
    }
}