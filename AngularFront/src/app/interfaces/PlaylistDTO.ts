import { PlaylistitemDTO } from "./PlaylistitemDTO";

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
  isCreatedByUser?: boolean; // 新增此屬性來區分自建或協作
  videos?: PlaylistitemDTO[];
}
