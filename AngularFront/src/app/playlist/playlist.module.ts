import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PlaylistComponent } from './playlist/playlist.component';
import { PlaylistmemberComponent } from './playlistmember/playlistmember.component';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { HttpClientModule } from '@angular/common/http';
import { CardModule } from 'primeng/card'; // 导入 CardModule
import { PaginatorModule } from 'primeng/paginator';
import { PlaylistitemComponent } from './playlistitem/playlistitem.component';

const routes: Routes = [
  { path: '', component: PlaylistComponent },
  { path: 'member', component: PlaylistmemberComponent },
  { path: 'item', component: PlaylistitemComponent},
];

@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistmemberComponent,
    PlaylistitemComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ButtonModule,
    TableModule,
    HttpClientModule,
    CardModule, // 确保导入 CardModule
    PaginatorModule // 确保导入 PaginatorModule
  ],
  exports: [RouterModule],
})
export class PlaylistModule { }

