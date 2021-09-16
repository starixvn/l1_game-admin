import React from "react";
import BigSmallBase from "../big-small-base/big-small-base";

type Props = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
};

export default class BigSmallTurbo extends React.Component<
  Props,
  { data: object }
> {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <BigSmallBase
        type="chanle"
        turbo={true}
        onMessage={this.props.onMessage}
        sendMessage={this.props.sendMessage}
        isProduction={this.props.isProduction}
      />
    );
  }
}
