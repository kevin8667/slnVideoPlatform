import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PlaylistComponent } from './playlist/playlist.component';
import { ButtonModule } from 'primeng/button';
import { HttpClientModule } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { PaginatorModule } from 'primeng/paginator';
import { DialogModule } from 'primeng/dialog';
import { PlaylistitemComponent } from './playlistitem/playlistitem.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PanelModule } from 'primeng/panel';
import { MultiSelectModule } from 'primeng/multiselect';
import { PlaylistMemberComponent } from './playlistmember/playlistmember.component';
import { PlaylistpostAputComponent } from './playlistpost-aput/playlistpost-aput.component';

const routes: Routes = [
  { path: '', component: PlaylistComponent },
  { path: 'member', component: PlaylistMemberComponent },
  { path: 'item', component: PlaylistitemComponent},
  { path: 'add', component: PlaylistpostAputComponent},
];

@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistitemComponent,
    PlaylistMemberComponent,
    PlaylistpostAputComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ButtonModule,
    HttpClientModule,
    CardModule,
    PaginatorModule,
    DialogModule,
    DragDropModule,
    PanelModule,
    MultiSelectModule
  ],
  exports: [RouterModule],
})
export class PlaylistModule { }

