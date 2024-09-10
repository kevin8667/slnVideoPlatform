import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { preOrder } from './pre-order.model';

@Injectable({
  providedIn: 'root'
})
export class PreOrderService {

  private apiUrl = 'https://localhost:7193/api/ProductLists'; // 替換為你的 API URL

  constructor(private http: HttpClient) { }

  getProduct(): Observable<preOrder[]> {
    return this.http.get<preOrder[]>(this.apiUrl);
  }
}
