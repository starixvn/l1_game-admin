import React from "react";

type Props = {
  value: string;
  name: string;
  labelStyle: string;
  inputStyle: string;
  disabled: boolean;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onBlur: (event: React.ChangeEvent<HTMLInputElement>) => void;
};

type State = {
  type: string;
  edit: boolean;
};

export default class EditableText extends React.Component<Props, State> {
  constructor(props) {
    super(props);
    console.log(props.className);
    this.state = {
      type: props.type || "text",
      edit: false,
    };
    this.handleDisableEditMode = this.handleDisableEditMode.bind(this);
    this.handleEnableEditMode = this.handleEnableEditMode.bind(this);
  }
  handleDisableEditMode(e) {
    this.props.onBlur(e);
    this.setState({ edit: false });
  }

  handleEnableEditMode() {
    this.setState({ edit: true });
  }

  render() {
    if (this.state.edit) {
      return (
        <input
          name={this.props.name}
          type={this.state.type}
          value={this.props.value}
          autoFocus
          onChange={this.props.onChange}
          onBlur={this.handleDisableEditMode}
          className={this.props.inputStyle}
        />
      );
    }

    return (
      <span
        className={this.props.labelStyle}
        onClick={this.props.disabled ? null : this.handleEnableEditMode}
      >
        {this.props.value}
      </span>
    );
  }
}
