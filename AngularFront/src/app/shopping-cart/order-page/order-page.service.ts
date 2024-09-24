import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderPage } from './order-page.model';

@Injectable({
  providedIn: 'root'
})
export class OrderPageService {

  private apiUrl = 'https://localhost:7193/api/Orders'; // 替換為你的 API URL

  constructor(private http: HttpClient) { }

  getOrders(): Observable<OrderPage[]> {
    return this.http.get<OrderPage[]>(this.apiUrl);
  }

   // 新增訂單的方法
  addOrder(order: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(this.apiUrl, order, { headers });
  }
}
