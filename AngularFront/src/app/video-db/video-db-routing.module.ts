import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';
import { VideoDetailComponent } from './video-detail/video-detail.component';
import { VideoDbSearchComponent } from './video-db-search/video-db-search.component';
import { SeasonListComponent } from './season-list/season-list.component';
import { EpisodeListComponent } from './episode-list/episode-list.component';
import { PersonDetailComponent } from './person-detail/person-detail.component';

const routes: Routes = [
  { path: '', component: VideoDBFrontPageComponent },
  { path: 'details/:id', component: VideoDetailComponent },
  { path: 'search', component: VideoDbSearchComponent },
  { path: 'seasons', component: SeasonListComponent },
  { path: 'episodes', component: EpisodeListComponent },
  { path: 'person/:id', component: PersonDetailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VideoDbRoutingModule { }
