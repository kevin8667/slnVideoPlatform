import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShoppingCartRoutingModule } from './shopping-cart-routing.module';
import { CartPageComponent } from './cart-page/cart-page.component';
import { OrderPageComponent } from './order-page/order-page.component';
import { PreOrderComponent } from './pre-order/pre-order.component';



@NgModule({
  declarations: [
    CartPageComponent,
    OrderPageComponent,
    PreOrderComponent

  ],
  imports: [
    CommonModule,
    ShoppingCartRoutingModule
  ]
})
export class ShoppingCartModule {


}
