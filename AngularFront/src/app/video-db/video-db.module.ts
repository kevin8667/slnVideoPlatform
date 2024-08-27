import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VideoDBRoutingModule } from './video-db-routing.module';
import { VideoDBComponent } from './video-db.component';


@NgModule({
  declarations: [
    VideoDBComponent
  ],
  imports: [
    CommonModule,
    VideoDBRoutingModule
  ]
})
export class VideoDBModule { }
