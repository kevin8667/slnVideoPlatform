import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { TestComponent } from './test/test.component';
import { FormsModule } from '@angular/forms';
import { CarouselModule } from 'primeng/carousel';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { SelectButtonModule } from 'primeng/selectbutton';
import { InputTextModule } from 'primeng/inputtext';
import { ChipsModule } from 'primeng/chips';
import { DropdownModule } from 'primeng/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';
import { DataViewModule } from 'primeng/dataview';
import { ImageModule } from 'primeng/image';
import { GalleriaModule } from 'primeng/galleria';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { FooterComponent } from './footer/footer.component';


@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    NavBarComponent,
    FooterComponent,
    // 這些組件將被移除並轉移到 VideoDBModule
    // VideoDBFrontPageComponent,
    // VideoDetailComponent,
    // VideoDbSearchComponent,
    // SeasonListComponent,
    // EpisodeListComponent,
    // PersonDetailComponent,
  ],
  imports: [
    BrowserModule,
    CarouselModule,
    AppRoutingModule,
    HttpClientModule,
    CardModule,
    ButtonModule,
    RippleModule,
    SelectButtonModule,
    FormsModule,
    InputTextModule,
    ChipsModule,
    DropdownModule,
    BrowserAnimationsModule,
    PaginatorModule,
    TableModule,
    DataViewModule,
    ImageModule,
    GalleriaModule,
    AutoCompleteModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
