import { withRouter } from "next/router";
import { Home } from ".";
import SocketIO from "../components/util/game-server-endpoint/socket-io";
import Cookies from "js-cookie";

class Monitor extends Home {
  constructor(props) {
    super(props);
  }

  initSocket() {
    const sedieServerEndpoint = process.env.NEXT_PUBLIC_ENV === "local" ? process.env.NEXT_PUBLIC_SEDIE_SERVER_ENDPOINT : Cookies.get("sedie-server-endpoint");
    return new SocketIO(`${this.protocol}://${sedieServerEndpoint}`)
  }
}

export default withRouter(Monitor);
