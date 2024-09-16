// forum.service.ts
import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  BehaviorSubject,
  firstValueFrom,
  Observable,
  ReplaySubject,
  throwError,
} from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Theme } from '../../interfaces/forumInterface/Theme';
import { ForumPagingDTO } from '../../interfaces/forumInterface/ForumPagingDTO';
import { ArticleView } from '../../interfaces/forumInterface/ArticleView';
import { Post } from '../../interfaces/forumInterface/Post';
import { LikeDTO } from '../../interfaces/forumInterface/LikeDTO';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { AllReactionsDTO } from 'src/app/interfaces/forumInterface/AllReactionsDTO';
import { AuthService } from 'src/app/auth.service';
import { memberName } from 'src/app/interfaces/forumInterface/memberIName';
@Injectable({
  providedIn: 'root',
})
export default class ForumService {
  private renderer: Renderer2;
  private themeTagSubject = new BehaviorSubject<Theme[]>([]);
  themeTag$ = this.themeTagSubject.asObservable();
  private userSubject = new ReplaySubject<memberName>(1); // 初始化
  public user$ = this.userSubject.asObservable()
  constructor(
    private client: HttpClient,
    private sanitizer: DomSanitizer,
    private rendererFactory: RendererFactory2,
    private auth: AuthService
  ) {
    this.renderer = this.rendererFactory.createRenderer(null, null);
    this.loadThemeTags();
    this.loadAllCss();
    this.loadMember();
  }


  loadMember() {
    this.auth.getMemberId().subscribe({
      next: (data) => {
        if (data && data.memberId) {
          this.userSubject.next({
            memberId: data.memberId,
            nickName: '豬頭',
          });
        }
      },
      error(err) {
        console.error('獲取會員失敗', err);
      },
    });
  }
  loadThemeTags(): void {
    const api = 'https://localhost:7193/api/Articles/Theme';
    this.client.get<Theme[]>(api).subscribe({
      next: (data: Theme[]) => {
        this.themeTagSubject.next(data);
      },
      error: (error: any) => {
        console.error('Failed to load theme tags', error);
      },
    });
  }
  getArticleView(data: {}): Observable<ForumPagingDTO> {
    return this.client.post<ForumPagingDTO>(
      'https://localhost:7193/api/Articles/LoadIndex',
      data
    );
  }
  getArticle(id: number): Observable<ArticleView> {
    return this.client
      .get<ArticleView>(`https://localhost:7193/api/Articles/${id}`)
      .pipe(
        catchError(() => {
          // 處理錯誤
          history.back();
          return throwError(
            () => new Error('服務異常：Failed to fetch article')
          );
        })
      );
  }
  getPosts(id: number): Observable<Post[]> {
    return this.client
      .get<Post[]>(`https://localhost:7193/api/Posts/all/${id}`)
      .pipe(
        catchError(() => {
          return throwError(() => new Error('服務異常:獲取多筆回文發生例外'));
        })
      );
  }
  getPost(id: number): Observable<Post> {
    return this.client.get<Post>(`https://localhost:7193/api/Posts/${id}`).pipe(
      catchError(() => {
        return throwError(() => new Error('服務異常:獲取單筆回文發生例外'));
      })
    );
  }
  updateArticle(id: number, articleData: Partial<ArticleView>) {
    const api = `https://localhost:7193/api/Articles/${id}`;
    return this.client.patch(api, articleData);
  }
  updatePost(id: number, data: Partial<Post>) {
    const api = `https://localhost:7193/api/Posts/${id}`;
    return this.client.patch<ArticleView>(api, data);
  }
  deleteArticle(id: number) {
    const api = `https://localhost:7193/api/Articles/${id}`;
    return this.client.delete(api);
  }
  deletePost(id: number) {
    const api = `https://localhost:7193/api/Posts/${id}`;
    return this.client.delete(api);
  }
  getSafe(data: string): SafeHtml {
    if (!data) return '此文章並無內容，請盡速修改!';

    return this.sanitizer.bypassSecurityTrustHtml(data);
  }
  async getPicture(img: FormData) {
    const api = 'https://localhost:7193/api/ForumImg';
    const data = await firstValueFrom(this.client.post(api, img));
    return data;
  }
  createArticle(data: ArticleView) {
    const api = 'https://localhost:7193/api/Articles';
    return this.client.post(api, data);
  }
  createPost(data: Post) {
    const api = 'https://localhost:7193/api/Posts';
    return this.client.post(api, data);
  }
  loadCss(fontUrl: string) {
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = fontUrl;
    this.renderer.appendChild(document.head, link);
  }
  ArticleCount(data: LikeDTO) {
    const api = 'https://localhost:7193/api/Articles/React';
    return this.client.post(api, data);
  }
  PostCount(data: LikeDTO) {
    const api = 'https://localhost:7193/api/Posts/React';
    return this.client.post(api, data);
  }
  getUserReaction(memberId: number, articleId: number) {
    const api = 'https://localhost:7193/api/Articles/UserReactions';
    const ArticleReactionDTO = { memberId: memberId, articleId: articleId };
    return this.client.post<AllReactionsDTO>(api, ArticleReactionDTO);
  }
  private loadAllCss() {
    this.loadCss(
      'https://fonts.googleapis.com/css2?family=Noto+Sans+TC:wght@400;500&display=swap'
    );
    this.loadCss(
      'https://fonts.googleapis.com/css2?family=Noto+Sans+TC:wght@700&display=swap'
    );
  }
}
