import { inject } from '@angular/core';
import { CartPageService } from '../cart-page/cart-page.service';
import { cartPage } from './../cart-page/cart-page.model';
import { CanActivateFn } from '@angular/router';

export const scGuardGuard: CanActivateFn = (route, state) => {
  const cartPage= inject(CartPageService)
  let Id = 0;
  cartPage.userId$.subscribe(data=>{
    // console.log("data.memberId",data.memberId)
    Id=data.memberId
  })
  console.log("Id",Id)

  if(Id < 1){
    alert("請先登入");
    cartPage.noLogin();
    return false;
  }

  return true;
};
