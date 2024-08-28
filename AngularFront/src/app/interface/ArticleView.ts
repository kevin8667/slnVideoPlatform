export interface ArticleView {
  articleId: number;
  themeId: number;
  authorId: number;
  title: string;
  articleContent: string;
  postDate: Date;
  updateDate: Date;
  replyCount: number;
  lock: false;
  articleImage: null;
  posts: [];
  themeName: string;
  memberName: string;
}
