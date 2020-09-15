import Cookies from "js-cookie";
import { GetServerSideProps } from "next";
import { withRouter } from "next/router";

import { Home } from ".";
import GameName from "../components/shared/common/GameName";
import SocketIO from "../components/util/game-server-endpoint/socket-io";

class Monitor extends Home {
  constructor(props) {
    super(props);
  }

  initSocket() {
    debugger;
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

export const getServerSideProps: GetServerSideProps = async (context) => {
  return {
    props: { game: context.query.game }
  };
};

export default withRouter(Monitor);
