import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { cartPage } from './cart-page.model';

@Injectable({
  providedIn: 'root'
})
export class CartPageService {
  private apiUrl = 'https://localhost:7193/api/ShoppingCarts'; // 根據你的 API URL 進行調整
  navigate: any;

  constructor(private http: HttpClient) { }

  GetShoppingCarts(): Observable<cartPage[]> {
    return this.http.get<cartPage[]>(this.apiUrl);
  }

  // 新增購物車資料
  createShoppingCart(shoppingCart: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(this.apiUrl, shoppingCart, { headers });
  }

  //刪除指定的購物車
  deleteShoppingCart(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete(url);
  }

  //更新購物車
  updateShoppingCart(cartId: number, updatedData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${cartId}`, updatedData);
  }

  private PlanapiUrl = 'https://localhost:7193/api/PlanLists'; // 根據你的 API URL 進行調整

   //取得方案資料
  GetPlans(): Observable<any[]> {
    return this.http.get<any[]>(this.PlanapiUrl);  // 假設你的API路徑是 /api/plans
  }
}
