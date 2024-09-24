import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShoppingCartRoutingModule } from './shopping-cart-routing.module';
import { CartPageComponent } from './cart-page/cart-page.component';
import { OrderPageComponent } from './order-page/order-page.component';
import { PreOrderComponent } from './pre-order/pre-order.component';
import { FinishPaymentComponent } from './finish-payment/finish-payment.component';
import { FormsModule } from '@angular/forms';  // 用來處理表單
import { HttpClientModule } from '@angular/common/http';  // 用來處理 HTTP 請求

import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { ForDriverComponent } from './for-driver/for-driver.component';



@NgModule({
  declarations: [
    CartPageComponent,
    OrderPageComponent,
    PreOrderComponent,
    FinishPaymentComponent,
    ForDriverComponent

  ],
  imports: [
    CommonModule,
    ShoppingCartRoutingModule,
    FormsModule,       // 表單模組
    HttpClientModule,   // HttpClient 模組
    SidebarModule,
    ButtonModule
  ]
})
export class ShoppingCartModule {


}
