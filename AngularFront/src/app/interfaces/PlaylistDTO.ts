export interface PlaylistDTO {
  playListId: number;
  playListName: string;
  playListDescription: string;
  viewCount: number;
  likeCount: number;
  addedCount: number;
  sharedCount: number;
  showImage: string | null;
  showLikeEffect?: boolean;
}
