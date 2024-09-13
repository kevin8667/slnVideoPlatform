import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private apiUrl = 'https://localhost:7193/api/Member/GetMemberId'; // 替換為您的實際 API URL
  private cachedMemberId: number | null = null; // 緩存會員 ID

  constructor(private http: HttpClient) {}

  getMemberId(): Observable<{ MemberId: number }> {
    // 如果已經緩存了會員 ID，直接返回緩存的值
    if (this.cachedMemberId !== null) {
      return of({ MemberId: this.cachedMemberId });
    }

    // 否則發送請求獲取會員 ID
    return this.http.get<{ MemberId: number }>(this.apiUrl).pipe(
      catchError(err => {
        console.error('獲取會員 ID 失敗', err);
        return of({ MemberId: -1 }); // 返回一個默認值或錯誤處理
      }),
      tap(data => {
        this.cachedMemberId = data.MemberId; // 緩存會員 ID
      })
    );
  }
}
