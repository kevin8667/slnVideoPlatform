import { QuillModule } from 'ngx-quill';
import { NgModule } from '@angular/core';

import { ArticleComponent } from './article/article.component';
import { TableModule } from 'primeng/table';
import { HttpClientModule } from '@angular/common/http';
import { ForumService } from '../service/forum.service';
import { forumDatePipe } from '../pipe/pipes/my-date.pipe';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { PaginatorModule } from 'primeng/paginator';
import { SliderModule } from 'primeng/slider';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { DataViewModule } from 'primeng/dataview';
import { NewArticleComponent } from './new-article/new-article.component';
import { NewPostComponent } from './new-post/new-post.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MultiSelectModule } from 'primeng/multiselect';
import { ForumRoutingModule } from './forum-routing.module';
import { ArticleListComponent } from './article-list/article-list.component';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SplitButtonModule } from 'primeng/splitbutton';
import { ToastModule } from 'primeng/toast'; // 確保引入 ToastModule
@NgModule({
  declarations: [
    ArticleComponent,
    forumDatePipe,
    NewArticleComponent,
    NewPostComponent,
    ArticleListComponent,
  ],
  imports: [
    TableModule,
    HttpClientModule,
    ButtonModule,
    CommonModule,
    PaginatorModule,
    SliderModule,
    AutoCompleteModule,
    DataViewModule,
    ReactiveFormsModule,
    MultiSelectModule,
    FormsModule,
    ForumRoutingModule,
    QuillModule,
    InputTextModule,
    MessageModule,
    SplitButtonModule,
    ToastModule,
  ],
  exports: [],
  providers: [ForumService],
})
export class ForumModule {}
