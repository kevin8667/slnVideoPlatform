import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/data.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  orders: any[] = [];
  memberId: number = 1; // 假設當前的會員 ID 為 1

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.getOrders();
  }

  // 調用 DataService 的 API 取得訂單資料
  getOrders(): void {
    this.dataService.getOrdersByMemberId(this.memberId).subscribe(
      (response) => {
        this.orders = response; // 將 API 返回的資料賦值給 orders 陣列
      },
      (error) => {
        console.error('取得訂單失敗', error);
      }
    );
  }
}
