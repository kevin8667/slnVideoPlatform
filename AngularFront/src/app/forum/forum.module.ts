import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleComponent } from './article/article.component';
import { TableModule } from 'primeng/table';
import { HttpClientModule } from '@angular/common/http';
import { ForumServiceService } from '../service/forum-service.service';
import { forumDatePipe } from '../pipe/pipes/my-date.pipe';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PaginatorModule } from 'primeng/paginator';
import { DividerModule } from 'primeng/divider';
import { SliderModule } from 'primeng/slider';
import { InputTextModule } from 'primeng/inputtext';
import { AutoCompleteModule } from 'primeng/autocomplete';
const routes: Routes = [
  { path: '', component: ArticleListComponent },
  { path: 'article/:id?', component: ArticleComponent },
  { path: '**', component: ArticleListComponent }, // 使用路由參數
];

@NgModule({
  declarations: [ArticleListComponent, ArticleComponent, forumDatePipe],
  imports: [
    RouterModule.forChild(routes),
    TableModule,
    HttpClientModule,
    ButtonModule,
    FormsModule,
    CommonModule,
    PaginatorModule,
    DividerModule,
    SliderModule,
    InputTextModule,
    AutoCompleteModule
  ],
  exports: [RouterModule],
  providers: [ForumServiceService],
})
export class ForumModule {}
