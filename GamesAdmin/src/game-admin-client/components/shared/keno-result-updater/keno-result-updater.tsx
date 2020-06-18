import React from 'react';
import styles from './keno-result-updater.module.scss'
import {Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, Col} from 'reactstrap';
import {upperFirstLetter} from '../../util/util';
import {pickNumbers} from './pick-numbers';

type Props = {
  modal: boolean,
  toggle: Function,
  submit: Function
  isMini?: boolean
};

type State = {
  modal: boolean,
  errors: string,
  types: Array<OptionType>
};

type OptionType = {
  name: string,
  value: boolean
}

const TYPES = {
  group1: ["gold", "wood", "water", "fire", "earth"],
  group2: ["big", "small"],
  group3: ["even", "odd"],
  group4: ["big", "gold", "wood"],
  group5: ["small", "fire", "earth"]
}

const TYPES_MAP = {
  big: 0,
  small: 1,
  even: 2,
  odd: 3,
  earth: 4,
  fire: 5,
  gold: 6,
  water: 7,
  wood: 8
};

export default class KenoResultUpdater extends React.Component<Props, State> {
  constructor(props: any) {
    super(props);

    this.state = {
      modal: false,
      errors: "",
      types: [...TYPES.group1, ...TYPES.group2, ...TYPES.group3].map((type) => {
        return {name: type, value: false}
      })
    }

    this.toggle = this.toggle.bind(this);
    this.handleCheck = this.handleCheck.bind(this);
    this.submit = this.submit.bind(this);
  }

  toggle() {
    this.props.toggle();
  }

  submit() {
    const result = this.getCheckedTypes();
    if (result.length < 3) {
      this.setState({
        errors: "You must choose three values."
      })
    } else {
      const isEven = result.includes("even");
      const isBig = result.includes("big");
      const bigSmallTypes = [...TYPES.group2, ...TYPES.group3]
      const element = TYPES_MAP[result.filter(x => !bigSmallTypes.includes(x))[0]]
      const list = pickNumbers(isEven, isBig, element, this.props.isMini);
      const fieldNames = [
        Object.keys(TYPES_MAP)[element],
        isBig ? 'big' : 'small',
        isEven ? 'even' : 'odd',
      ];

      this.props.submit(list, fieldNames);
    }
  }

  handleCheck(type, e) {
    const dependingTypes = this.findDependingTypes(type.name);
    const types = this.updateCheck(dependingTypes, type.name, e.target.checked);

    this.setState({
      types: types,
      errors: ""
    })
  }

  getCheckedTypes(): Array<string> {
    const result = this.state.types.reduce<Array<string>>((arr: Array<string>, type: OptionType) => {
      if (type.value) {
        arr.push(type.name)
      }
      return arr;
    }, [])

    return result;
  }

  updateCheck(dependingTypes, typeName, typeValue) {
    const types = this.state.types;
    dependingTypes.forEach(dt => {
      let type = types.find(o => o.name === dt);
      if (dt !== typeName) {
        type.value = false;
      } else {
        type.value = typeValue;
      }
    });

    return types;
  }

  findDependingTypes(typeName) {
    const dependingTypes = [];

    Object.keys(TYPES).forEach(function (key) {
      const group: string[] = TYPES[key];
      if (group.indexOf(typeName) >= 0) {
        dependingTypes.push(...group)
      }
    });

    return dependingTypes;
  }

  renderType(type, index) {
    return <div className={styles['check-block']} key={index}>
      <div>{upperFirstLetter(type.name)}</div>
      <div><input type="checkbox" checked={type.value} onChange={(e) => this.handleCheck(type, e)}/></div>
    </div>
  }

  render() {
    return (
      <div>
        <Modal isOpen={this.props.modal} toggle={this.toggle} className={styles['keno-result-updater']}>
          <ModalHeader className={styles['custom-modal-header']} toggle={this.toggle}></ModalHeader>
          <ModalBody className={styles['modal-content']}>
            <Row className={styles['row']}>
              {
                this.state.types.slice(0, 5).map((type, index) => this.renderType(type, index))
              }
            </Row>
            <Row className={styles['row']}>
              {
                this.state.types.slice(5, 9).map((type, index) => this.renderType(type, index))
              }
            </Row>
            <Row className={styles['errors']}>
              <div>{this.state.errors}</div>
            </Row>
          </ModalBody>
          <ModalFooter className={styles['custom-modal-footer']}>
            <Button color="primary" onClick={this.submit} className={styles['custom-button']}>Submit</Button>
          </ModalFooter>
        </Modal>
      </div>
    );
  }
}
