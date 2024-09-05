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
        this.sc = data
        .filter(item => item.memberId === this.filterMemberId)
        .map(item => {
          // 根據 planId 設定價格
          switch(item.planId) {
            case 1:
              item.price = 50;
              break;
            case 2:
              item.price = 100;
              break;
            case 3:
              item.price = 200;
              break;
            case 4:
              item.price = 300;
              break;
            case 5:
              item.price = 350;
              break;
            default:
              item.price = 0; // 預設價格
          }

          // 根據 videoId 設定圖片路徑
          switch(item.videoId) {
            case 1:
              item.imagePath = 'assets/img/godfather.jpg';
              break;
            case 2:
              item.imagePath = 'assets/img/1995.jpg';
              break;
            case 3:
              item.imagePath = 'assets/img/Dark Knight.jpg';
              break;
            case 4:
              item.imagePath = 'assets/img/Gump.jpg';
              break;
            default:
              item.imagePath = 'assets/img/NoImage.jpg'; // 預設圖片
          }

          return item;
        }); // 將 API 返回的資料分配給 sc
        // 使用 console.log 查看傳送過來的資料
        console.log('CartPage data:', this.sc);
      },
      error => {
        console.error('Error fetching shopping carts:', error);
      }
    );

  }
}
