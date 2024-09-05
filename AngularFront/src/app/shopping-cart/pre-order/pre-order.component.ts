import { Component, OnInit } from '@angular/core';
import { PreOrderService } from './pre-order.service';
import { preOrder } from './pre-order.model';

@Component({
  selector: 'app-pre-order',
  templateUrl: './pre-order.component.html',
  styleUrls: ['./pre-order.component.css']
})
export class PreOrderComponent {
  products: preOrder[] = [];
  displayedProducts: preOrder[] = [];  // 用於顯示的產品數組

  constructor(private PreOrderService: PreOrderService) { }

  ngOnInit(): void {
    this.PreOrderService.getProduct().subscribe(
      data => {
        this.products = data;
        console.log('Fetched Products:', this.products);
        this.displayedProducts = this.products.slice(0, 5);  // 切割前 5 筆資料
        console.log('Displayed Products (first 5):', this.displayedProducts);
      },
      error => {
        console.error('Error fetching products:', error);
      }
    );
  }

}
