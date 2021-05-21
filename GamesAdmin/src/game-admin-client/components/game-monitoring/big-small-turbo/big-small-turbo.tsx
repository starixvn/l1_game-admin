import React from "react";
import BigSmallBase from "../big-small-base/big-small-base";

type BigSmallProps = {
  onMessage: Function;
};

export default class BigSmallTurbo extends React.Component<
  BigSmallProps,
  { data: object }
> {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <BigSmallBase
        type="taixiu"
        turbo={true}
        onMessage={this.props.onMessage}
      />
    );
  }
}
