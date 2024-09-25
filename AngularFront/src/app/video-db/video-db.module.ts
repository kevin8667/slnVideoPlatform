import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoDbRoutingModule } from './video-db-routing.module';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';
import { VideoDetailComponent } from './video-detail/video-detail.component';
import { VideoDbSearchComponent } from './video-db-search/video-db-search.component';
import { SeasonListComponent } from './season-list/season-list.component';
import { EpisodeListComponent } from './episode-list/episode-list.component';
import { PersonDetailComponent } from './person-detail/person-detail.component';
import { HttpClientModule } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ChipsModule } from 'primeng/chips';
import { DropdownModule } from 'primeng/dropdown';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';
import { DataViewModule } from 'primeng/dataview';
import { ImageModule } from 'primeng/image';
import { GalleriaModule } from 'primeng/galleria';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { CarouselModule } from 'primeng/carousel';
import { MultiSelectModule } from 'primeng/multiselect';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { RatingModule } from 'primeng/rating';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { NewVideoComponent } from './new-video/new-video.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { FileUploadModule } from 'primeng/fileupload';
import { CheckboxModule } from 'primeng/checkbox';
import { PanelModule } from 'primeng/panel';
import { FieldsetModule } from 'primeng/fieldset';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { AddtoplaylistComponent } from './addtoplaylist/addtoplaylist.component';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';
import { TagModule } from 'primeng/tag';
import { DialogModule } from 'primeng/dialog';

@NgModule({
  declarations: [
    VideoDBFrontPageComponent,
    VideoDetailComponent,
    VideoDbSearchComponent,
    SeasonListComponent,
    EpisodeListComponent,
    PersonDetailComponent,
    NewVideoComponent,
    AddtoplaylistComponent,
  ],
  imports: [
    CommonModule,
    VideoDbRoutingModule,
    HttpClientModule,
    CardModule,
    ButtonModule,
    RippleModule,
    SelectButtonModule,
    FormsModule,
    InputTextModule,
    ChipsModule,
    DropdownModule,
    PaginatorModule,
    TableModule,
    DataViewModule,
    ImageModule,
    GalleriaModule,
    AutoCompleteModule,
    CarouselModule,
    MultiSelectModule,
    OverlayPanelModule,
    RatingModule,
    ToastModule,
    ConfirmDialogModule,
    DynamicDialogModule,
    ReactiveFormsModule,
    CalendarModule,
    FileUploadModule,
    CheckboxModule,
    PanelModule,
    FieldsetModule,
    InputTextareaModule,
    TagModule,
    DialogModule
  ],
  providers: [DialogService, MessageService],
})
export class VideoDbModule { }
