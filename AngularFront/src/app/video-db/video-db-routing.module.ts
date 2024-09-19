import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';
import { VideoDetailComponent } from './video-detail/video-detail.component';
import { VideoDbSearchComponent } from './video-db-search/video-db-search.component';
import { SeasonListComponent } from './season-list/season-list.component';
import { EpisodeListComponent } from './episode-list/episode-list.component';
import { PersonDetailComponent } from './person-detail/person-detail.component';
import { NewVideoComponent } from './new-video/new-video.component';
import { AddtoplaylistComponent } from './addtoplaylist/addtoplaylist.component';

const routes: Routes = [
  { path: '', component: VideoDBFrontPageComponent },
  { path: 'details/:id', component: VideoDetailComponent },
  { path: 'search', component: VideoDbSearchComponent },
  { path: 'seasons', component: SeasonListComponent },
  { path: 'episodes', component: EpisodeListComponent },
  { path: 'person/:id', component: PersonDetailComponent },
  { path: 'newvideo', component: NewVideoComponent },
  { path: 'addtoplaylist', component: AddtoplaylistComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VideoDbRoutingModule { }
