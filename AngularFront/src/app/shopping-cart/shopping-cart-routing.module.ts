import { cartPage } from './cart-page/cart-page.model';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShoppingCartComponent } from './shopping-cart.component';
import { CartPageComponent } from './cart-page/cart-page.component';
import { OrderPageComponent } from './order-page/order-page.component';

const routes: Routes = [
  {path: 'cart', component: CartPageComponent},
  {path: 'order', component: OrderPageComponent},
  // { path: '', component: CartPageComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule]
})
export class ShoppingCartRoutingModule { }
