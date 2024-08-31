import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PlaylistComponent } from './playlist/playlist.component';
import { PlaylistmemberComponent } from './playlistmember/playlistmember.component';
import { ButtonModule } from 'primeng/button';
import { HttpClientModule } from '@angular/common/http';
import { CardModule } from 'primeng/card';
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
    HttpClientModule,
    CardModule,
    PaginatorModule
  ],
  exports: [RouterModule],
})
export class PlaylistModule { }

