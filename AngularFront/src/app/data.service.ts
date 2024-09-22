import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs'; // 引入 of
import { AuthService } from './auth.service';
import { switchMap } from 'rxjs/operators';
@Injectable({
  providedIn: 'root',
})
export class DataService {
  private apiUrl = 'https://localhost:7193/api/Showtimes'; // 確保您後端的API路徑是正確的

  constructor(private http: HttpClient, private auth: AuthService) {}

  // 根據電影 ID 取得上映中的影院
  getCinemas(videoId: number): Observable<any> {
    const url = `${this.apiUrl}/nowshowing/${videoId}`; // 根據 videoId 獲取影院
    return this.http.get<any>(url);
  }

  // 根據影院 ID 取得放映時間
  getShowtimesByCinema(cinemaId: number): Observable<any> {
    const url = `${this.apiUrl}/cinema/${cinemaId}`; // 根據 cinemaId 獲取放映時間
    return this.http.get<any>(url);
  }

  // 生成訂單的 API 調用
  createReservation(reservationData: any): Observable<any> {
    const url = `${this.apiUrl}`; // 調用 POST /api/Showtimes 生成訂單
    return this.http.post<any>(url, reservationData);
  }

  // 分配座位的 API 調用
  assignSeats(seatSelectionData: any): Observable<any> {
    const url = `${this.apiUrl}/reservation/seats`; // 調用 POST /api/Showtimes/reservation/seats 分配座位
    return this.http.post<any>(url, seatSelectionData);
  }
  // **新增: 根據會員ID取得所有訂單資料**
  getOrdersByMemberId(memberId: number): Observable<any[]> {
    const url = `${this.apiUrl}/member/${memberId}/reservation`; // 根據會員ID取得訂單
    return this.http.get<any[]>(url);
  }
   // 根據當前登入會員的ID取得訂單資料
  //  getOrdersForCurrentMember(): Observable<any[]> {
  //   return this.auth.getMemberId().pipe( // 調用 AuthService 的 getMemberId 方法
  //     switchMap((response) => {
  //       if (response.MemberId && !response.error) {
  //         const url = `${this.apiUrl}/member/${response.MemberId}/reservation`; // 使用會員ID進行API請求
  //         return this.http.get<any[]>(url);
  //       } else {
  //         // 如果沒有會員ID，返回一個空的結果
  //         return of([]); // 返回空數組
  //       }
  //     })
  //   );
  // }

}
