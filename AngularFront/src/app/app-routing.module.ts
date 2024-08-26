import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';
import { TestComponent } from './test/test.component';
import { AppComponent } from './app.component';
import { VideoDetailComponent } from './video-detail/video-detail.component';

const routes: Routes = [
    { path: 'VDB', component: VideoDBFrontPageComponent },
    { path: 'test', component: TestComponent },
    { path:'details/:id', component: VideoDetailComponent},
    { path:"**", component: AppComponent}
  ];
  
  @NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
export class AppRoutingModule { }