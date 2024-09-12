import { PlaylistDTO } from "./PlaylistDTO";

export interface PlayListCreateDTO {
  PlayList: PlaylistDTO;
  videoIds: number[];
  collaboratorIds: number[];
}
