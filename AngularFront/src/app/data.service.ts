import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private apiUrl = 'https://localhost:7193/api/Showtimes'; // 確保您後端的API路徑是正確的

  constructor(private http: HttpClient) {}

  // 取得所有影院的資料
  // getCinemas(): Observable<any> {
  //   const url = `${this.apiUrl}/cinemas`; // 完整 API URL
  //   return this.http.get<any>(url);
  // }

  // 根據電影 ID 取得上映中的影院
  getCinemas(videoId: number): Observable<any> {
    const url = `${this.apiUrl}/nowshowing/${videoId}`; // 根據 videoId 獲取影院
    return this.http.get<any>(url);
  }
  // 根據影院 ID 取得放映時間
  // getShowtimesByCinema(cinemaId: number): Observable<any> {
  //   const url = `${this.apiUrl}/cinema/${cinemaId}/showtimes`; // 使用影院 ID 來取得放映時間
  //   return this.http.get<any>(url);
  // }
  // 根據影院 ID 取得放映時間
  getShowtimesByCinema(cinemaId: number): Observable<any> {
    const url = `${this.apiUrl}/cinema/${cinemaId}`; // 根據 cinemaId 獲取放映時間
    return this.http.get<any>(url);
  }
}
