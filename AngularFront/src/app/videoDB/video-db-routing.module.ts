import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDetailComponent } from '../video-detail/video-detail.component';
import { VideoDbSearchComponent } from '../video-db-search/video-db-search.component';
import { VideoDBFrontPageComponent } from '../video-dbfront-page/video-dbfront-page.component';

const routes: Routes = [
  { path: '', component: VideoDBFrontPageComponent },
  { path: 'details/:id', component: VideoDetailComponent },
  { path: 'search', component: VideoDbSearchComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)], // 正確地使用 forChild
  exports: [RouterModule]
})
export class VideoDBRoutingModule {}
