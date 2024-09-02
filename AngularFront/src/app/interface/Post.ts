export interface Post {
  articleId: number;
  postId: number;
  posterId: number;
  postContent: string;
  postDate: Date;
  lock: boolean;
  postImage: null;
  nickName:string;
}
