import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PlaylistComponent } from './playlist/playlist.component';
import { PlaylistmemberComponent } from './playlistmember/playlistmember.component';
import { ButtonModule } from 'primeng/button';
import { HttpClientModule } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { PaginatorModule } from 'primeng/paginator';
import { DialogModule } from 'primeng/dialog';
import { PlaylistitemComponent } from './playlistitem/playlistitem.component';
import { DragDropModule } from '@angular/cdk/drag-drop';

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
    PaginatorModule,
    DialogModule,
    DragDropModule
  ],
  exports: [RouterModule],
})
export class PlaylistModule { }

