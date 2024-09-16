import { EditComponent } from './edit/edit.component';
import { QuillModule } from 'ngx-quill';
import { NgModule } from '@angular/core';

import { ArticleComponent } from './article/article.component';
import { TableModule } from 'primeng/table';
import ForumService from '../services/forumService/forum.service';
import { forumDatePipe } from '../pipe/pipes/my-date.pipe';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { PaginatorModule } from 'primeng/paginator';
import { SliderModule } from 'primeng/slider';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { DataViewModule } from 'primeng/dataview';
import { ReactiveFormsModule } from '@angular/forms';
import { ForumRoutingModule } from './forum-routing.module';
import { ArticleListComponent } from './article-list/article-list.component';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SplitButtonModule } from 'primeng/splitbutton';
import { ToastModule } from 'primeng/toast';
import { AutoFocusModule } from 'primeng/autofocus';
import { GalleriaModule } from 'primeng/galleria';
import { EditorComponent } from './share/editor/editor.component';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { SidebarModule } from 'primeng/sidebar';
@NgModule({
  declarations: [
    ArticleComponent,
    forumDatePipe,
    ArticleListComponent,
    EditorComponent,
    EditComponent
  ],
  imports: [
    TableModule,
    ButtonModule,
    CommonModule,
    PaginatorModule,
    SliderModule,
    AutoCompleteModule,
    DataViewModule,
    ReactiveFormsModule,
    ForumRoutingModule,
    QuillModule,
    InputTextModule,
    MessageModule,
    SplitButtonModule,
    ToastModule,
    AutoFocusModule,
    GalleriaModule,
    TagModule,
    ConfirmDialogModule,
    MenuModule,
    ToggleButtonModule,
    SidebarModule
  ],
  exports: [],
  providers: [ForumService, ],
})
export class ForumModule {}
