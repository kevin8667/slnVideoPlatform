// forum.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  BehaviorSubject,
  firstValueFrom,
  lastValueFrom,
  Observable,
  throwError,
} from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Theme } from '../interfaces/forumnterface/Theme';
import { ForumPagingDTO } from '../interfaces/forumnterface/ForumPagingDTO';
import { ArticleView } from '../interfaces/forumnterface/ArticleView';
import { Post } from '../interfaces/forumnterface/Post';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root',
})
export default class ForumService {
  private themeTagSubject = new BehaviorSubject<Theme[]>([]);
  themeTag$ = this.themeTagSubject.asObservable();

  constructor(private client: HttpClient, private sanitizer: DomSanitizer) {
    this.loadThemeTags();
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
    return this.client
      .delete(api)
      .pipe(
        catchError(() =>
          throwError(() => new Error('服務異常:刪除單筆文章發生例外'))
        )
      );
  }
  deletePost(id: number) {
    const api = `https://localhost:7193/api/Posts/${id}`;
    return this.client
      .delete<Post>(api)
      .pipe(
        catchError(() =>
          throwError(() => new Error('服務異常:獲取單筆回文發生例外'))
        )
      );
  }
  getSafe(data: string): SafeHtml {
    if (!data) return '此文章並無內容，請盡速修改!';

    return this.sanitizer.bypassSecurityTrustHtml(data);
  }
  async getPicture(img: any) {
    const api = 'https://localhost:7193/api/FourmImg';
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
}
