import axios from "axios";
import {
  CreateOrUpdateViewModel,
  DirectLobbyConfig,
  DirectLobbyManagementViewModel,
} from "./data-types";

const urls = {
  createOrUpdate: "/direct-lobby",
  get: "/direct-lobby/get",
  delete: "/direct-lobby/delete",
  getLobbies: "/direct-lobby/GetAll",
};
export class LobbyApi {
  static async getLobbies() {
    const lobbyEntity = await axios.get<DirectLobbyManagementViewModel>(
      urls.getLobbies
    );
    return lobbyEntity.data;
  }

  static async createOrUpdateLobbyConfig(lobbyModel: DirectLobbyConfig) {
    const lobbyEntity = await axios.post<string>(
      urls.createOrUpdate,
      lobbyModel
    );
    return lobbyEntity.data;
  }

  static async getLobbyConfig(id: string) {
    const lobbyEntity = await axios.get<CreateOrUpdateViewModel>(
      `${urls.get}?id=${id}`
    );
    return lobbyEntity.data;
  }

  static async deleteLobbyConfig(id: string) {
    const lobbyEntity = await axios.post<boolean>(`${urls.delete}?id=${id}`);
    return lobbyEntity.data;
  }
}
