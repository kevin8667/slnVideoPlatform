import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { TestComponent } from './test/test.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';
import { CarouselModule } from 'primeng/carousel';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { VideoDetailComponent } from './video-detail/video-detail.component';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { SelectButtonModule } from 'primeng/selectbutton';
import { VideoDbSearchComponent } from './video-db-search/video-db-search.component';

@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    VideoDBFrontPageComponent,
    VideoDetailComponent,
    VideoDbSearchComponent
  ],
  imports: [
    BrowserModule,
    NgbModule,
    CarouselModule,
    AppRoutingModule,
    HttpClientModule,
    CardModule,
    ButtonModule,
    RippleModule,
    SelectButtonModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
