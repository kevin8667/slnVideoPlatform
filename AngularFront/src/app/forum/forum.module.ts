import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleComponent } from './article/article.component';
import { NotFoundError, pipe } from 'rxjs';
import { TableModule } from 'primeng/table';
import { HttpClientModule } from '@angular/common/http';
import { ForumServiceService } from '../service/forum-service.service';
import { forumDatePipe } from '../pipe/pipes/my-date.pipe';

const routes: Routes = [
  { path: '', component: ArticleListComponent },
  { path: 'article/:id?', component: ArticleComponent },
  { path: '**', component: NotFoundError }, // 使用路由參數
];

@NgModule({
  declarations: [ArticleListComponent, ArticleComponent,forumDatePipe],
  imports: [RouterModule.forChild(routes), TableModule, HttpClientModule],
  exports: [RouterModule],
  providers: [ForumServiceService],
})
export class ForumModule {}
