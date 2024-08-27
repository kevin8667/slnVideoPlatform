import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './forum/article-list/article-list.component';
import { AppComponent } from './app.component';

const routes: Routes = [
  { path: '', component: AppComponent },
  {
    path: 'forum',
    component: ArticleListComponent,
    loadChildren: () =>
      import('./forum/forum.module').then((m) => m.ForumModule),
  },
  // 其他路由配置
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
