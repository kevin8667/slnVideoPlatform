import { NgModule } from '@angular/core';
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
import { NewArticleComponent } from './new-article/new-article.component';
import { NewPostComponent } from './new-post/new-post.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MultiSelectModule } from 'primeng/multiselect';
import { ForumRoutingModule } from './forum-routing.module';

@NgModule({
  declarations: [
    ArticleListComponent,
    ArticleComponent,
    forumDatePipe,
    NewArticleComponent,
    NewPostComponent,
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
  ],
  exports: [],
  providers: [ForumServiceService],
})
export class ForumModule {}
