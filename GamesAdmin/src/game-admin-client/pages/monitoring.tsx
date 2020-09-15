import Cookies from "js-cookie";
import { NextPageContext } from "next";
import { withRouter } from "next/router";

import { Home } from ".";
import GameName from "../components/shared/common/GameName";
import SocketIO from "../components/util/game-server-endpoint/socket-io";

class Monitor extends Home {
  static async getInitialProps(ctx: NextPageContext) {
    return { game: ctx.query.game }
  }

  initSocket() {
    let serverEndpoint = "";
    switch (this.props.router.query.game) {
      case GameName.VipSeDie:
        serverEndpoint =
          process.env.NEXT_PUBLIC_ENV === "local"
            ? process.env.NEXT_PUBLIC_VIP_SEDIE_SERVER_ENDPOINT
            : Cookies.get("vip-sedie-server-endpoint");
        break;
      case GameName.SeDie:
        serverEndpoint =
          process.env.NEXT_PUBLIC_ENV === "local"
            ? process.env.NEXT_PUBLIC_SEDIE_SERVER_ENDPOINT
            : Cookies.get("sedie-server-endpoint");
        break;
    }
    return new SocketIO(`${this.protocol}://${serverEndpoint}`);
  }
}


export default withRouter(Monitor);
