export interface PlaylistDTO {
  playListName: string;
  playListDescription: string;
  viewCount: number;
  likeCount: number;
  addedCount: number;
  sharedCount: number;
  showImage: string | null;
  showLikeEffect?: boolean;
}
