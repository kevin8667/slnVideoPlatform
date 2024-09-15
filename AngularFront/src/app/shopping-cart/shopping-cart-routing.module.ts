import { cartPage } from './cart-page/cart-page.model';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShoppingCartComponent } from './shopping-cart.component';

import { CartPageComponent } from './cart-page/cart-page.component';
import { OrderPageComponent } from './order-page/order-page.component';
import { PreOrderComponent } from './pre-order/pre-order.component';
import { FinishPaymentComponent } from './finish-payment/finish-payment.component';

const routes: Routes = [
  {path: 'cart', component: CartPageComponent},
  {path: 'preOrder/:shoppingCartId/:videoName/:planName/:price/:imagePath', component: PreOrderComponent},
  {path: 'order', component: OrderPageComponent},
  {path: 'finish', component: FinishPaymentComponent},

  // { path: '', component: CartPageComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule]
})
export class ShoppingCartRoutingModule { }
