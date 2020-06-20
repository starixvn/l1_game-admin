import React from 'react';
import styles from './baccarat-result-updater.module.scss'
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Row } from 'reactstrap';

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
  displayName: string,
  value: boolean
}

const TYPES = {
  group1: ["player", "banker", "tie"],
  group2: ["big", "small"]
};

const TYPES_MAP = {
  player: 0,
  banker: 1,
  tie: 2,
  playerPair: 3,
  bankerPair: 4,
  big: 5,
  small: 6
};

const CHOICES = [
  { name: "player", displayName: "Player" },
  { name: "banker", displayName: "Banker" },
  { name: "big", displayName: "Big" },
  { name: "small", displayName: "small" },
  { name: "tie", displayName: "Tie" },
  { name: "playerPair", displayName: "Player Pair" },
  { name: "bankerPair", displayName: "Banker Pair" }
];

const CHOICE_GROUPS = [
  ["player", "banker", "tie"],
  ["big", "small"]
];

export default class BaccaratResultUpdater extends React.Component<Props, State> {
  constructor(props: any) {
    super(props);

    this.state = {
      modal: false,
      errors: "",
      types: CHOICES.map((choice) => {
        return { name: choice.name, displayName: choice.displayName, value: false }
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
    // const result = this.getCheckedTypes();
    // if (result.length < 3) {
    //   this.setState({
    //     errors: "You must choose three values."
    //   })
    // } else {
    //   const isEven = result.includes("even");
    //   const isBig = result.includes("big");
    //   const bigSmallTypes = [...TYPES.group2, ...TYPES.group3]
    //   const element = TYPES_MAP[result.filter(x => !bigSmallTypes.includes(x))[0]]
    //   const list = pickNumbers(isEven, isBig, element, this.props.isMini);
    //   const fieldNames = [
    //     Object.keys(TYPES_MAP)[element],
    //     isBig ? 'big' : 'small',
    //     isEven ? 'even' : 'odd',
    //   ];

    //   this.props.submit(list, fieldNames);
    // }
  }

  handleCheck(type, e) {
    const dependingTypes = this.finDependingChoices(type.name);
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

  updateCheck(dependingTypes: any[], typeName: string, typeValue: boolean) {
    const types = this.state.types;

    types.forEach(type => {
      if (type.name == typeName) {
        type.value = typeValue;
      }

      if (dependingTypes.indexOf(type.name) > -1) {
        type.value = false;
      }
    });
    console.log(types);
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

  finDependingChoices(typeName) {
    const choices = [];
    CHOICE_GROUPS.forEach(group => {
      const index = group.indexOf(typeName);

      if (index >= 0) {
        choices.push(...group.splice(index, 1));
      }
    });
    console.log(choices);
    return choices;
  }

  renderType(type: OptionType, index) {
    return <div className={styles['check-block']} key={index}>
      <div>{type.displayName}</div>
      <div><input type="checkbox" checked={type.value} onChange={(e) => this.handleCheck(type, e)} /></div>
    </div>
  }

  render() {
    return (
      <div>
        <Modal isOpen={this.props.modal} toggle={this.toggle} className={styles['result-updater']}>
          <ModalHeader className={styles['custom-modal-header']} toggle={this.toggle}></ModalHeader>
          <ModalBody className={styles['modal-content']}>
            <Row className={styles['row']}>
              {
                this.state.types.map((type, index) => this.renderType(type, index))
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
