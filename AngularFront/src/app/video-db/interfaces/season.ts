import { Video } from "./video";

export interface Season
{
  seasonId: number;
  seriesId: number;
  seasonName: string;
  seasonNumber: number;
  episodeCount: number;
  releaseDate: string;
  summary: string;
  videoLists: Video[]
}
