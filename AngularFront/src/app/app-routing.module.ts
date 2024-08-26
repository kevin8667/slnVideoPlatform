import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoDBFrontPageComponent } from './video-dbfront-page/video-dbfront-page.component';
import { TestComponent } from './test/test.component';
import { AppComponent } from './app.component';

const routes: Routes = [
    { path: 'VDB', component: VideoDBFrontPageComponent },
    { path: 'test', component: TestComponent },
    {path:"**", component: AppComponent}
  ];
  
  @NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
export class AppRoutingModule { }