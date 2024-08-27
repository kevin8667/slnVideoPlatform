import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDBComponent } from './video-db.component';
import { VideoDetailComponent } from './video-detail/video-detail.component';

const routes: Routes = [
  { path: '', component: VideoDBComponent },
  { path:'details/:id', component: VideoDetailComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VideoDBRoutingModule { }
