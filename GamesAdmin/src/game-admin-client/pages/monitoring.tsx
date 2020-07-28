import { NextRouter, withRouter } from "next/router";
import { Home } from ".";

class Monitor extends Home {
  constructor(props) {
    super(props, true);
  }
}

export default withRouter(Monitor);
