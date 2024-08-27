import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleComponent } from './article/article.component';
import { NotFoundError } from 'rxjs';

const routes: Routes = [
  { path: '', component: ArticleListComponent },
  { path: 'article/:id?', component: ArticleComponent },
  {path:'**',component:ArticleListComponent} // 使用路由參數
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ForumModule {}
