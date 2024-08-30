import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleComponent } from './article/article.component';
import { TableModule } from 'primeng/table';
import { HttpClientModule } from '@angular/common/http';
import { ForumServiceService } from '../service/forum-service.service';
import { forumDatePipe } from '../pipe/pipes/my-date.pipe';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { PaginatorModule } from 'primeng/paginator';
import { SliderModule } from 'primeng/slider';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { DataViewModule } from 'primeng/dataview';

const routes: Routes = [
  { path: '', component: ArticleListComponent },
  { path: ':id', component: ArticleComponent },
  { path: '**', component: ArticleListComponent }, // 使用路由參數
];

@NgModule({
  declarations: [ArticleListComponent, ArticleComponent, forumDatePipe],
  imports: [
    RouterModule.forChild(routes),
    TableModule,
    HttpClientModule,
    ButtonModule,
    CommonModule,
    PaginatorModule,
    SliderModule,
    AutoCompleteModule,
    DataViewModule,
  ],
  exports: [RouterModule],
  providers: [ForumServiceService],
})
export class ForumModule {}
