import { OrderPageService } from './order-page.service';
import { Component, OnInit } from '@angular/core';
import { OrderPage } from './order-page.model';

@Component({
  selector: 'app-order-page',
  templateUrl: './order-page.component.html',
  styleUrls: ['./order-page.component.css']
})
export class OrderPageComponent {
  orders: OrderPage[] = [];
  filterMemberId: number = 2;

  constructor(private OrderPageService: OrderPageService) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.OrderPageService.getOrders().subscribe(
      data => {
        this.orders = data.filter(item => item.memberId === this.filterMemberId);
        console.log('OrderPage:', this.orders);
      },
      error => {
        console.error('Error fetching orders:', error);
      }
    );
  }
}
