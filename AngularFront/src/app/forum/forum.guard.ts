import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const forumGuard: CanActivateFn = (route, state) => {
  const router = inject(Router); // 使用 inject() 來獲取服務

  const idRoute = route.paramMap.get('id');
  const id = Number(idRoute);
  const type = route.paramMap.get('type');

  if (type && type !== 'post' && type !== 'article') {
    router.navigate(['/error']); // 導向錯誤頁面
    return false;
  }

  if (isNaN(id)) {
    router.navigate(['/error']); // 導向錯誤頁面
    return false; // 阻止導航
  }

  return true;
};
