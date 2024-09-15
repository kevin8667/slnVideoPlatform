import { LikeDTO } from './LikeDTO';

export interface AllReactionsDTO {
  articleReaction: LikeDTO | null;
  postReactions: LikeDTO[];
}
