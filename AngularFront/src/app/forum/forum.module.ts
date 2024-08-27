import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleComponent } from './article/article.component';

const routes: Routes = [
  { path: '', component: ArticleListComponent }, // 當訪問 /article 時，載入 ArticleComponent
  { path: 'article/:id', component: ArticleComponent }, // 當訪問 /article/post/:id 時，載入 PostComponent
  { path: '**', component: ArticleListComponent }, // 當訪問 /article 時，載入 ArticleComponent
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
})
export class ForumModule {}
