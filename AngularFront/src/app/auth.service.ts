import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of,BehaviorSubject } from 'rxjs';
import { catchError, tap, shareReplay } from 'rxjs/operators';
// 定義接口來處理返回的結果和錯誤狀態
interface MemberIdResponse {
  MemberId: number;
  error?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7193/api/Member/GetMemberId'; // 替換為您的實際 API URL
  private cachedMemberId: number | null = null; // 緩存會員 ID

  constructor(private http: HttpClient,private router:Router) {}

  private isLogin = new BehaviorSubject<boolean>(this.hasToken());

  removeCookie(name: string): void {
    // 设置一个过期时间为过去的时间
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/';
  }
  get isLoggedIn() {
    return this.isLogin.asObservable();
  }
  hasToken(): boolean {
    return this.getCookie('JwtToken') != null;
  }

  Logout(): void {
    debugger;
    this.isLogin.next(false);
    this.removeCookie('JwtToken');
    this.router.navigateByUrl('login');

  }
  SetLoginValue(): void {
    this.isLogin.next(true);
  }

  getCookie(name: string): string | null {
    const nameEQ = name + '=';
    const cookiesArray = document.cookie.split(';');

    for (let i = 0; i < cookiesArray.length; i++) {
      let cookie = cookiesArray[i].trim();

      if (cookie.indexOf(nameEQ) === 0) {
        return cookie.substring(nameEQ.length, cookie.length);
      }
    }

    return null; // 返回 null 如果没有找到该 cookie
  }
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
