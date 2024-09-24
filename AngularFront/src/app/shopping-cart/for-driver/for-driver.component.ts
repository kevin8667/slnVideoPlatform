import { Component } from '@angular/core';
import { OrderPageService } from '../order-page/order-page.service';
import { OrderPage } from '../order-page/order-page.model';

@Component({
  selector: 'app-for-driver',
  templateUrl: './for-driver.component.html',
  styleUrls: ['./for-driver.component.css']
})
export class ForDriverComponent {
  orders: OrderPage[] = [];
  filterMemberId: number = 2;

  constructor(private OrderPageService: OrderPageService) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.OrderPageService.getOrders().subscribe(
      data => {
        this.orders = data
        .map(item => {
          // 根據 planId 設定價格
          switch(item.paymentStatus) {
            case 0:
              item.payments = "未付款";
              break;
            case 1:
                item.payments = "已付款";
                break;
            default:
              item.payments = "";
          }

          switch(item.deliveryStatus) {
            case 0:
              item.deliverys = "送貨中";
              break;
            case 1:
                item.deliverys = "已送達";
                break;
            default:
              item.deliverys = "";
          }

          return item;
        });
        console.log('OrderPage:', this.orders);
      },
      error => {
        console.error('Error fetching orders:', error);
      }
    );
  }

  newOrder={
    shoppingCartId: 5,
    couponId: 0,
    orderDate: new Date(),
    orderTotalPrice: 0,
    deliveryName: "小王",
    deliveryAddress: "資展國際",
    paymentStatus: 1,
    deliveryStatus: 0,
    driverId: 10,
    lastEditTime: new Date(),
  }

  addOrder(): void {
    this.OrderPageService.addOrder(this.newOrder).subscribe({
      next: (response) => {
        console.log('訂單新增成功:', response);
        this.loadOrders();
      },
      error: (error) => {
        console.error('新增訂單時發生錯誤:', error);
      },
    });
  }
}
