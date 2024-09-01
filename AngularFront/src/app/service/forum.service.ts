// forum.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Theme } from '../interface/Theme';
import { ForumPagingDTO } from '../interface/ForumPagingDTO';
import { ArticleView } from '../interface/ArticleView';

@Injectable({
  providedIn: 'root',
})
export class ForumService {
  private themeTagSubject = new BehaviorSubject<Theme[]>([]);
  themeTag$ = this.themeTagSubject.asObservable();

  constructor(private client: HttpClient) {
    this.loadThemeTags();
  }

  getArticle(id: number): Observable<ArticleView> {
    return this.client
      .get<ArticleView>(`https://localhost:7193/api/Articles/${id}`)
      .pipe(
        catchError((error) => {
          // 處理錯誤
          console.error('Error fetching article:', error);
          return throwError(() => new Error('Failed to fetch article'));
        })
      );
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
