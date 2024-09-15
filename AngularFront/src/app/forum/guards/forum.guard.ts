import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import ForumService from 'src/app/services/forumService/forum.service';

export const forumGuard: CanActivateFn = (route, state) => {
  const router = inject(Router); // 使用 inject() 來獲取服務
  const forumService = inject(ForumService);
  const type = route.paramMap.get('type');
  const requiresAuth = route.data['requiresAuth'];
  if (requiresAuth) {
    const userId = forumService.getCurrentUser().id;
    if (userId === null) {
      router.navigate(['/login']);
      return false;
    }
  }
  if (type) {
    // 檢查 type 是否為 'post' 或 'article'
    if (type !== 'post' && type !== 'article') {
      router.navigate(['/error']); // 導向錯誤頁面
      return false;
    }
  }

  const idRoute = route.paramMap.get('id');
  if (idRoute) {
    const id = Number(idRoute);

    //id填非數字
    if (isNaN(id)) {
      router.navigate(['/error']); // 導向錯誤頁面
      return false;
    }
  }


  return true;
};
