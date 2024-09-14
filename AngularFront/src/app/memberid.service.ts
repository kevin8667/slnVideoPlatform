import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap, shareReplay } from 'rxjs/operators';

// 定義接口來處理返回的結果和錯誤狀態
interface MemberIdResponse {
  MemberId: number;
  error?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private apiUrl = 'https://localhost:7193/api/Member/GetMemberId'; // 替換為您的實際 API URL
  private cachedMemberId: number | null = null; // 緩存會員 ID

  constructor(private http: HttpClient) {}

  getMemberId(): Observable<MemberIdResponse> {
    // 如果已經緩存了會員 ID，直接返回緩存的值
    if (this.cachedMemberId !== null) {
      return of({ MemberId: this.cachedMemberId });
    }

    // 發送請求獲取會員 ID
    return this.http.get<MemberIdResponse>(this.apiUrl).pipe(
      tap(data => this.cachedMemberId = data.MemberId), // 緩存數據
      shareReplay(1), // 緩存最後一次的結果，避免重複 HTTP 請求
      catchError(err => {
        console.error('獲取會員 ID 失敗', err);
        return of({ MemberId: -1, error: true }); // 返回錯誤標記
      })
    );
  }
}
