import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleComponent } from './article/article.component';
import { EditComponent } from './edit/edit.component';
import { unsavedChangesGuard } from './guards/unsaved-changes.guard';
import { forumGuard } from './guards/forum.guard';

const routes: Routes = [
  { path: '', component: ArticleListComponent },
  // 閱覽文章頁面
  { path: ':id', component: ArticleComponent, canActivate: [forumGuard] },
  // 發表回文
  {
    path: 'new/:type/:articleId', // 具有文章ID來加入該篇文章
    component: EditComponent,
    canActivate: [forumGuard], //利用路由守衛來檢查type是否為文章或是回文
    canDeactivate: [unsavedChangesGuard], //做儲存前檢查
    data: { requiresAuth: true }, //需登入才能進入
  },
  // 編輯文章或回文
  {
    path: 'ed/:type/:id', //利用type來分辨要向文章還是回文讀取資料
    component: EditComponent,
    canActivate: [forumGuard],
    canDeactivate: [unsavedChangesGuard],
    data: { requiresAuth: true },
  },
  //發表文章
  {
    path: 'new/:type',  //沒有文章的ID，認定為新發表，並在組件內透過沒有id來引導文章發表
    component: EditComponent,
    canActivate: [forumGuard],
    canDeactivate: [unsavedChangesGuard],
    data: { requiresAuth: true },
  },

  { path: '**', redirectTo: '/error' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ForumRoutingModule {}
