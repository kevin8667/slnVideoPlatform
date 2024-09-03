import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';
import { AppComponent } from './app.component';
import { VideoDetailComponent } from './video-detail/video-detail.component';
import { VideoDbSearchComponent } from './video-db-search/video-db-search.component';

const routes: Routes = [
    { path: 'video', component: VideoDBFrontPageComponent },
    { path:'details/:id', component: VideoDetailComponent},
    { path: 'search', component: VideoDbSearchComponent },
    { path:"**", component: AppComponent}
  ];

  @NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
export class AppRoutingModule { }
