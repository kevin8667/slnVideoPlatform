import { ArticleView } from "./ArticleView";

export interface ForumPagingDTO {
  totalCount: number;
  totalPages: number;
  forumResult: ArticleView[];
}
