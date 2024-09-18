export interface Post {
  articleId: number;
  postId: number;
  posterId: number;
  postContent: string;
  postDate: Date;
  lock: boolean;
  postImage: '';
  nickName: string;
  likeCount: number;
  dislikeCount: number;
}
