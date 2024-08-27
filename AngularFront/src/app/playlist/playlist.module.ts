import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common'; // 如果需要使用 ngIf 或 ngFor 等指令

import { PlaylistComponent } from './playlist/playlist.component';
import { PlaylistmemberComponent } from './playlistmember/playlistmember.component';

const routes: Routes = [
  { path: '', component: PlaylistComponent },
  { path: 'member', component: PlaylistmemberComponent },
];

@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistmemberComponent // 添加到這裡
  ],
  imports: [
    CommonModule, // 引入 CommonModule
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule],
})
export class PlaylistModule { }
