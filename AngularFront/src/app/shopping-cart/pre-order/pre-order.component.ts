import { Component, OnInit } from '@angular/core';
import { PreOrderService } from './pre-order.service';
import { preOrder } from './pre-order.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-pre-order',
  templateUrl: './pre-order.component.html',
  styleUrls: ['./pre-order.component.css']
})
export class PreOrderComponent {
  products: preOrder[] = [];
  displayedProducts: preOrder[] = [];  // 用於顯示的產品數組

  //從購物車傳資料
  shoppingCartId:number | undefined;
  videoName:string | undefined;
  planName:string | undefined;
  price: number =0;
  imagePath: string | undefined;

  //金額累加
  totalAmount: number = 0;


  constructor(
    private PreOrderService: PreOrderService,
    private ActivatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    //顯示左下方的商品列表
    this.PreOrderService.getProduct().subscribe(
      data => {
        this.products = data;
        // 為每個商品添加 isAdded 屬性，初始值設為 false
        this.products.forEach(product => {
          product.isAdded = false;
        });

        console.log('Fetched Products:', this.products);

        this.displayedProducts = this.products.slice(0, 5);  // 切割前 5 筆資料
        console.log('Displayed Products (first 5):', this.displayedProducts);
      },
      error => {
        console.error('Error fetching products:', error);
      }
    );

    //接收從Cart傳過來影片資訊
    this.ActivatedRoute.params.subscribe(params => {
      this.shoppingCartId = +params['shoppingCartId']; // 接收 id
      this.videoName = params['videoName'];
      this.planName = params['planName'];
      this.price = +params['price'];
      this.imagePath = params['imagePath'];
    });

    this.totalAmount = this.price;

  }

 // 加購或取消加購商品並更新小計
 addProductToCart(product: any) {
  if (product.isAdded) {
    // 取消加購：從小計中扣除商品價格
    this.totalAmount -= product.productPrice;
  } else {
    // 加購：將商品價格加到小計
    this.totalAmount += product.productPrice;
  }
  // 切換加購狀態
  product.isAdded = !product.isAdded;
}
}
