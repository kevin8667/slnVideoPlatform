import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private apiUrl = 'https://localhost:7193/api/Showtimes'; // 確保您後端的API路徑是正確的

  constructor(
    private http: HttpClient,
    private auth: AuthService) {}
    private userSubject = new BehaviorSubject<memberName>({
      memberId: 0,
      nickName: '',
    }); // 初始化
    public user$ = this.userSubject.asObservable();



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


}
