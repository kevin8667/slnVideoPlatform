import { cartPage } from './cart-page/cart-page.model';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShoppingCartComponent } from './shopping-cart.component';

import { CartPageComponent } from './cart-page/cart-page.component';
import { OrderPageComponent } from './order-page/order-page.component';
import { PreOrderComponent } from './pre-order/pre-order.component';
import { FinishPaymentComponent } from './finish-payment/finish-payment.component';
import { scGuardGuard } from './guards/sc-guard.guard';
import { ErrorPageComponent } from '../error-page/error-page.component';
import { ForDriverComponent } from './for-driver/for-driver.component';

const routes: Routes = [
  {path: 'cart', component: CartPageComponent, canActivate: [scGuardGuard]},
  {path: 'preOrder/:shoppingCartId/:videoName/:planName/:price/:imagePath', component: PreOrderComponent, canActivate: [scGuardGuard]},
  {path: 'order', component: OrderPageComponent, canActivate: [scGuardGuard]},
  {path: 'finish', component: FinishPaymentComponent},
  {path: 'driver', component: ForDriverComponent},
  // { path: "**", component: ErrorPageComponent},
  {path: '', component: CartPageComponent, canActivate: [scGuardGuard]},
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule]
})
export class ShoppingCartRoutingModule { }
