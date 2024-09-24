import { ArticleView } from "./ArticleView";

export interface ForumPagingDTO {
  totalCount: number;
  forumResult: ArticleView[];
}
