import { OrderPageService } from './order-page.service';
import { Component, OnInit } from '@angular/core';
import { OrderPage } from './order-page.model';
import { CartPageService } from '../cart-page/cart-page.service';

@Component({
  selector: 'app-order-page',
  templateUrl: './order-page.component.html',
  styleUrls: ['./order-page.component.css']
})
export class OrderPageComponent {
  orders: OrderPage[] = [];
  filterMemberId: number = 2;
  cartPageService: any;

  constructor(private OrderPageService: OrderPageService,
    private cartPageServices: CartPageService,
  ) { }

  ngOnInit(): void {
    this.loadOrders();
    this.cartPageServices.userId$.subscribe((user: { memberId: number; }) => this.filterMemberId=user.memberId);
  }

  loadOrders(): void {
    this.OrderPageService.getOrders().subscribe(
      data => {
        this.orders = data
        .filter(item => item.memberId === this.filterMemberId)
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

  sidebarVisible: boolean = false;

  //選擇訂單資料
  selectedOrder: any;

  selectOrder(order: any){
    this.selectedOrder = order;
    this.sidebarVisible = true;
  }
}
