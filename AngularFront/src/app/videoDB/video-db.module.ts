import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoDBFrontPageComponent } from '../video-dbfront-page/video-dbfront-page.component';
import { CarouselModule } from 'primeng/carousel';
import { AppRoutingModule } from '../app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { VideoDetailComponent } from '../video-detail/video-detail.component';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { SelectButtonModule } from 'primeng/selectbutton';
import { VideoDbSearchComponent } from '../video-db-search/video-db-search.component';
import { InputTextModule } from 'primeng/inputtext';
import { ChipsModule } from 'primeng/chips';
import { DropdownModule } from 'primeng/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';
import { DataViewModule } from 'primeng/dataview';
import { ImageModule } from 'primeng/image';
import { SeasonListComponent } from '../season-list/season-list.component';
import { EpisodeListComponent } from '../episode-list/episode-list.component';
import { PersonDetailComponent } from '../person-detail/person-detail.component';
import { GalleriaModule } from 'primeng/galleria';
import { AutoCompleteModule } from 'primeng/autocomplete';


@NgModule({
  declarations: [VideoDetailComponent,
    VideoDBFrontPageComponent,
    VideoDbSearchComponent,
    SeasonListComponent,
    EpisodeListComponent,
    PersonDetailComponent
  ],
  imports: [
    CommonModule,
    CarouselModule,
    AppRoutingModule,
    HttpClientModule,
    CardModule,
    ButtonModule,
    RippleModule,
    SelectButtonModule,
    InputTextModule,
    ChipsModule,
    DropdownModule,
    BrowserAnimationsModule,
    PaginatorModule,
    TableModule,
    DataViewModule,
    ImageModule,
    GalleriaModule,
    AutoCompleteModule
  ]
})
export class VideoDBModule { }
