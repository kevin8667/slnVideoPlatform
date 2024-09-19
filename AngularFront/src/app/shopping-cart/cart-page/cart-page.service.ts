import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { cartPage } from './cart-page.model';
import { AuthService } from 'src/app/auth.service';
import { memberName } from 'src/app/interfaces/forumInterface/memberIName';

@Injectable({
  providedIn: 'root',
})
export class CartPageService {
  private userSubject = new BehaviorSubject<memberName>({
    memberId: 0,
    nickName: '',
  });
  public userId$ = this.userSubject.asObservable();

  private apiUrl = 'https://localhost:7193/api/ShoppingCarts'; // 根據你的 API URL 進行調整
  navigate: any;

  constructor(private http: HttpClient, private auth: AuthService, private router:Router) {
    this.getUserId();
  }

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
    return this.http.get<any[]>(this.PlanapiUrl); // 假設你的API路徑是 /api/plans
  }

  getUserId() {
    this.auth.MemberBehaviorData?.subscribe({
      next: (data) => {
        if (typeof data?.memberID==="number") {
          this.userSubject.next({
            memberId: data?.memberID || 0,
            nickName: data.nickName,
          });
        }
        else{
          this.userSubject.next({
            memberId: 0,
            nickName: ""
          })
        }
        console.log("memberId", data?.memberID)
      },
    });
  }

  noLogin(){
    this.router.navigate(["login"])
  }

}
