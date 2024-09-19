export interface ArticleView {
  articleContent: string;
  articleId: number;
  articleImage: string;
  authorId: number;
  lock: boolean;
  nickName: string;
  postDate: Date;
  replyCount: number;
  themeId: number;
  themeName: string;
  title: string;
  updateDate: Date;
  likeCount: number;
  dislikeCount: number;
}
