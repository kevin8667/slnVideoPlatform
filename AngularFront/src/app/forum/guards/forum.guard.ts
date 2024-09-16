import { memberName } from 'src/app/interfaces/forumInterface/memberIName';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import ForumService from 'src/app/services/forumService/forum.service';
import { AuthService } from 'src/app/auth.service';

export const forumGuard: CanActivateFn = (route, state) => {
  const router = inject(Router); // 使用 inject() 來獲取服務
  const type = route.paramMap.get('type');
  const requiresAuth = route.data['requiresAuth'];
  const auth = inject(AuthService);
  const forumService = inject(ForumService);
  if (requiresAuth) {
    let userId = 0;
    forumService.user$.subscribe((data) => {
      if (data) userId = data.memberId;
    });
    if (userId < 1) {
      router.navigate(['/login'], { queryParams: { returnUrl: state.url } });

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
