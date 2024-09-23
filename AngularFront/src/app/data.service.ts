import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs'; // 引入 of
import { AuthService } from './auth.service';
import { memberName } from './interfaces/forumInterface/memberIName';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private apiUrl = 'https://localhost:7193/api/Showtimes'; // 確保您後端的API路徑是正確的

  private userSubject = new BehaviorSubject<memberName>({
    memberId: 0,
    nickName: '',
  }); // 初始化
  public user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient, private auth: AuthService) {
    this.loadMember();
  }

  loadMember() {
    this.auth.MemberBehaviorData?.subscribe({
      next: (data) => {
        // console.log(data?.MemberId);
        // 檢查 data 是否為 null 並做防範性處理
        if (data && typeof data.memberID === 'number') {
          // 更新 userSubject 的值
          this.userSubject.next({
            memberId: data.memberID,
            nickName: data.nickName,
          });
        } else {
          // 如果資料無效，更新為 null
          this.userSubject.next({ memberId: 0, nickName: '' });
        }
      },
      error(err) {
        console.error('獲取會員失敗', err);
      },
    });
  }

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

  // 根據影院和影片 ID 取得放映時間
  getShowtimesByCinemaAndMovie(
    cinemaId: number,
    videoId: number
  ): Observable<any> {
    const url = `${this.apiUrl}/cinema/${cinemaId}/movie/${videoId}`;
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
}
