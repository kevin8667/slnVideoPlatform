import { CartPageService } from './cart-page.service';
import { Component, OnInit } from '@angular/core';
import { cartPage } from './cart-page.model';



@Component({
  selector: 'app-cart-page',
  templateUrl: './cart-page.component.html',
  styleUrls: ['./cart-page.component.css']
})
export class CartPageComponent {
  sc: cartPage[] = []; //用model定義介面數據
  filterMemberId: number = 2;

  constructor(private CartPageService: CartPageService){}

  ngOnInit(): void {
    // 訂閱服務返回的數據
    this.CartPageService.GetShoppingCarts().subscribe(
      data => {
        this.sc = data.filter(item => item.memberId === this.filterMemberId); // 將 API 返回的資料分配給 sc
        // 使用 console.log 查看傳送過來的資料
        console.log('CartPage data:', this.sc);
      },
      error => {
        console.error('Error fetching shopping carts:', error);
      }
    );
  }
}
