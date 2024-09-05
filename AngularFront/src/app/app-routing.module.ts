import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';
import { VideoDetailComponent } from './video-detail/video-detail.component';
import { VideoDbSearchComponent } from './video-db-search/video-db-search.component';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AppComponent } from './app.component';

const routes: Routes = [
  { path: '', component: AppComponent },
  {
    path: 'video',
    loadChildren: () =>
      import('./videoDB/video-db.module').then((m) => m.VideoDBModule),
  },
  { path: '**', redirectTo: '/' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)], // 正確地使用 forRoot
  exports: [RouterModule],
  providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
})
export class AppRoutingModule {}
