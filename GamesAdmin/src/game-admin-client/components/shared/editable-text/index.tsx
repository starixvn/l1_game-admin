import React from "react";

type Props = {
  value: string;
  name: string;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
};

type State = {
  type: string;
  edit: boolean;
};

export default class EditableText extends React.Component<Props, State> {
  constructor(props) {
    super(props);
    this.state = {
      type: props.type || "text",
      edit: false,
    };
    this.handleDisableEditMode = this.handleDisableEditMode.bind(this);
    this.handleEnableEditMode = this.handleEnableEditMode.bind(this);

  }
  handleDisableEditMode() {
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
        />
      );
    }

    return <span onClick={this.handleEnableEditMode}>{this.props.value}</span>;
  }
}
