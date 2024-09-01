export interface PlaylistitemDTO {
  playListId: number;
  videoId: number;
  videoPosition: number;
  videoName: string;
  thumbnailId: number | null;
  episode: number | null;
}
