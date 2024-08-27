import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDBFrontPageComponent } from './video-db/video-dbfront-page/video-dbfront-page.component';
import { AppComponent } from './app.component';


const routes: Routes = [
  { path: '',
    loadChildren: () =>
      import('./video-db/video-db.module').then((m) => m.VideoDBModule),
    component: VideoDBFrontPageComponent },
  {
    path: 'videos',
    loadChildren: () =>
      import('./video-db/video-db.module').then((m) => m.VideoDBModule),
    component: VideoDBFrontPageComponent,
  },
  { path: '**', component: AppComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
