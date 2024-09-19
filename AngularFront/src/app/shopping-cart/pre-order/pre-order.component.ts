import { Component, OnInit } from '@angular/core';
import { PreOrderService } from './pre-order.service';
import { preOrder } from './pre-order.model';
import { ActivatedRoute } from '@angular/router';
import { linePay } from './line-pay.model';

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

  //折扣影響finalPrice
  discountCode = '-10%';
  finalPrice: number = 99999;



  constructor(
    private PreOrderService: PreOrderService,
    private ActivatedRoute: ActivatedRoute,
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
    this.finalPrice = this.calculateDiscountedPrice(this.totalAmount, this.discountCode);

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

  //調整總價
  this.finalPrice = this.calculateDiscountedPrice(this.totalAmount, this.discountCode);
  }

  //給linepay的資料
  linePay: linePay ={
    orderId : "5",
    amount: 99999
  }

  toLinePay(){
    //調整給linepay變數
    this.linePay.orderId= this.generateUniqueId();
    this.linePay.amount=this.finalPrice;

    this.PreOrderService.linePayService(this.linePay).subscribe(
      response => {
        if (response.paymentUrl) {
          window.location.href = response.paymentUrl;  // 導向 LINE Pay 支付頁面
        }
      },
      error => {
        console.error('Error in payment request', error);
      });
  }

  // 給LinePay隨機產生唯一ID的方法
  generateUniqueId(): string {
    const currentDateTime = new Date();
    const year = currentDateTime.getFullYear();
    const month = ('0' + (currentDateTime.getMonth() + 1)).slice(-2);
    const day = ('0' + currentDateTime.getDate()).slice(-2);
    const hours = ('0' + currentDateTime.getHours()).slice(-2);
    const minutes = ('0' + currentDateTime.getMinutes()).slice(-2);
    const seconds = ('0' + currentDateTime.getSeconds()).slice(-2);

    return `${year}${month}${day}${hours}${minutes}${seconds}`;
  }

  //折扣影響總價
  calculateDiscountedPrice(price: number, discountCode: string): number {
    let discount: number;

    // 根據字串判別折扣
    switch (discountCode.toLowerCase()) {
      case '-20%':
        discount = 0.8;  // 20% 折扣
        break;
      case '-10%':
        discount = 0.9;  // 10% 折扣
        break;
      case '-5%':
        discount = 0.95; // 5% 折扣
        break;
      default:
        discount = 1;    // 無折扣
        break;
    }

    // 計算總價
    const totalPrice = price * discount;
    return totalPrice;
  }

}
