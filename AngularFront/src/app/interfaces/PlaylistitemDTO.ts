export interface PlaylistitemDTO {
  playListId: number;
  videoId: number;
  videoPosition: number;
  videoName: string;
  thumbnailPath: string | null;
  episode: number | null;
}
