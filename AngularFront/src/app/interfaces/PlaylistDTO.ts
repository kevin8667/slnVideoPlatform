export interface PlaylistDTO {
  playListId?: number;
  playListName: string;
  playListDescription: string;
  viewCount?: number;
  likeCount?: number;
  addedCount?: number;
  sharedCount?: number;
  playListImage?: string | null;
  showImage?: string | null;
  playListCreatedAt?: Date;
  playListUpdatedAt?: Date;
  analysisTimestamp?: Date;
  showLikeEffect?: boolean;
}
