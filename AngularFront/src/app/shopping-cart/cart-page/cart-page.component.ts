import { CartPageService } from './cart-page.service';
import { Component, OnInit } from '@angular/core';
import { cartPage } from './cart-page.model';
import { Router } from '@angular/router';



@Component({
  selector: 'app-cart-page',
  templateUrl: './cart-page.component.html',
  styleUrls: ['./cart-page.component.css']
})
export class CartPageComponent {
  sc: cartPage[] = []; //用model定義介面數據
  plans: any[] = [];
  filterMemberId: number = 0;


  constructor(
    private cartPageService: CartPageService,
    private router: Router  // 注入 Angular 的 Router
  ){}

  ngOnInit(): void {
    this.loadshoppingCart();
    this.loadPlans();
    this.cartPageService.userId$.subscribe(user => this.filterMemberId=user.memberId);
  }

  loadshoppingCart(){
    // 訂閱服務返回的數據
    this.cartPageService.GetShoppingCarts().subscribe(
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


  // 選擇購物車資料，傳到結帳頁面
  selectedItem: any;
  // 選擇某一行資料
  selectItem(item: any) {
    this.selectedItem = item;
  }
  checkout() {
    if (this.selectedItem) {
      console.log('selectedItem',this.selectedItem);
      console.log('videoName',this.selectedItem.videoName)
      try{
        this.router.navigate([
          '/shoppingCart/preOrder',
          this.selectedItem.shoppingCartId,
          this.selectedItem.videoName,
          this.selectedItem.planName,
          this.selectedItem.price,
          this.selectedItem.imagePath,
        ]);
      } catch (error) {
        console.error('Error navigating:', error);
      }
    }
  }

  //購物車model
  newShoppingCart = {
    memberId : 0,
    planId: 1,
    videoId: 1
  };

  //新增購物車(提交表單)
  onSubmit(): void {
    this.newShoppingCart.memberId=this.filterMemberId
    this.cartPageService.createShoppingCart(this.newShoppingCart)
      .subscribe(
        response => {
          console.log('Shopping cart added:', response);
          this.loadshoppingCart();
          // 可以在這裡加入跳轉或成功訊息提示
        },
        error => {
          console.error('Error adding shopping cart:', error);
        }
      );
  }

  //開關表單
  showForm = false;  // 控制表單顯示狀態的布林變數
  // 按鈕點擊事件處理方法，用於切換表單顯示狀態
  toggleForm(): void {
    this.showForm = !this.showForm;
  }

  //刪除購物車
  deleteShoppingCart(id: number): void {


    // 顯示確認訊息
    const confirmed = window.confirm('確定要刪除嗎？');

    if (confirmed) {
      this.cartPageService.deleteShoppingCart(id).subscribe(() => {
        console.log(`購物車 ${id} 已刪除`);
        this.loadshoppingCart(); // 刪除後重新載入資料
      }, error => {
        console.error('刪除失敗:', error);
      });
    }
  }

  // 加載方案資料
  loadPlans() {
    this.cartPageService.GetPlans().subscribe(data => {
      this.plans = data;  // 從API獲取的方案賦值給 plans
      console.log(data)
    });
  }

  //更新購物車
  updatePlan(item: any) {

    console.log('已更新項目的 planId:', item);
    this.cartPageService.updateShoppingCart(Number(item.shoppingCartId), { planId: Number(item.planId), videoId: Number(item.videoId), memberId: this.filterMemberId})
      .subscribe(response => {
        console.log('購物車已更新:', response);
        this.loadshoppingCart();
      }, error => {
        console.error('更新購物車時發生錯誤:', error);
      });
  }
}
