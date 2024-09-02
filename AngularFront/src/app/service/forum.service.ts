// forum.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Theme } from '../interface/Theme';
import { ForumPagingDTO } from '../interface/ForumPagingDTO';
import { ArticleView } from '../interface/ArticleView';
import { Router } from '@angular/router';
import { Post } from '../interface/Post';

@Injectable({
  providedIn: 'root',
})
export class ForumService {
  private themeTagSubject = new BehaviorSubject<Theme[]>([]);
  themeTag$ = this.themeTagSubject.asObservable();

  constructor(private client: HttpClient, private route: Router) {
    this.loadThemeTags();
  }

  getArticle(id: number): Observable<ArticleView> {
    return this.client
      .get<ArticleView>(`https://localhost:7193/api/Articles/${id}`)
      .pipe(
        catchError(() => {
          // 處理錯誤
          history.back();
          return throwError(() => new Error('服務異常：Failed to fetch article'));
        })
      );
  }
  getPosts(id:number):Observable<Post[]>{
    return this.client.get<Post[]>(`https://localhost:7193/api/Posts/${id}`)
    .pipe(catchError(()=>{
      return throwError(() => new Error('服務異常：Failed to fetch post'));
    }))
  }
  private loadThemeTags(): void {
    this.client.get<Theme[]>('https://localhost:7193/api/Articles').subscribe({
      next: (data: Theme[]) => this.themeTagSubject.next(data),
      error: (error: any) => console.error('Failed to load theme tags', error),
    });
  }

  getArticleView(data: {}): Observable<ForumPagingDTO> {
    return this.client.post<ForumPagingDTO>(
      'https://localhost:7193/api/Articles',
      data
    );
  }
}
