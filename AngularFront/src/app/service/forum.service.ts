// forum.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Theme } from '../interface/Theme';
import { ForumPagingDTO } from '../interface/ForumPagingDTO';

@Injectable({
  providedIn: 'root',
})
export class ForumService {
  private themeTagSubject = new BehaviorSubject<Theme[]>([]);
  themeTag$ = this.themeTagSubject.asObservable();

  constructor(private client: HttpClient) {
    this.loadThemeTags();
  }

  private loadThemeTags(): void {
    this.client
      .get<Theme[]>('https://localhost:7193/api/Articles')
      .subscribe({
        next: (data: Theme[]) => this.themeTagSubject.next(data),
        error: (error: any) =>
          console.error('Failed to load theme tags', error),
      });
  }

  getArticleView(data: {}): Observable<ForumPagingDTO> {
    return this.client.post<ForumPagingDTO>(
      'https://localhost:7193/api/Articles',
      data
    );
  }
}
