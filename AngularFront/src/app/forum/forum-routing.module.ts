import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { NewArticleComponent } from './new-article/new-article.component';
import { NewPostComponent } from './new-post/new-post.component';
import { ArticleComponent } from './article/article.component';

const routes: Routes = [
  { path: '', component: ArticleListComponent },
  { path: 'newA', component: NewArticleComponent },
  { path: 'newP', component: NewPostComponent },
  { path: ':id', component: ArticleComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ForumRoutingModule {}
