import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartPageService {
  private apiUrl = 'https://localhost:7193/api/ShoppingCarts'; // 根據你的 API URL 進行調整

  constructor(private http: HttpClient) { }

  GetShoppingCarts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
