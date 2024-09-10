import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { VideoDBFrontPageComponent } from './video-db/video-dbfront-page/video-dbfront-page.component';
import { AppComponent } from './app.component';
// import { VideoDetailComponent } from './video-db/video-detail/video-detail.component';
// import { VideoDbSearchComponent } from './video-db/video-db-search/video-db-search.component';

const routes: Routes = [
  { path: 'video-db', loadChildren: () => import('./video-db/video-db.module').then(m => m.VideoDbModule) },
    
    { path:"**", component: AppComponent}
  ];

  @NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
export class AppRoutingModule { }
