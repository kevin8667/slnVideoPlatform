import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { TestComponent } from './test/test.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';


@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    VideoDBFrontPageComponent
  ],
  imports: [
    BrowserModule,
    NgbModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
