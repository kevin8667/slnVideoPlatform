import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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
}
