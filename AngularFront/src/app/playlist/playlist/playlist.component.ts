import { Component, OnInit } from '@angular/core';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../../interfaces/PlaylistitemDTO';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent implements OnInit {
  playlists: PlaylistDTO[] = [];
  paginatedPlaylists: PlaylistDTO[] = [];
  selectedPlaylistItems: PlaylistitemDTO[] = [];
  displayModal: boolean = false;
  rows: number = 20;
  first: number = 0;

  constructor(private playlistService: PlaylistService) {}

  ngOnInit(): void {
    this.playlistService.getPlaylists().subscribe(data => {
      this.playlists = data.map(playlist => {
        return { ...playlist, showLikeEffect: false };
      });
      this.paginate({ first: this.first, rows: this.rows });
    });
  }

  paginate(event: any): void {
    this.first = event.first;
    this.rows = event.rows;
    const start = this.first;
    const end = this.first + this.rows;
    this.paginatedPlaylists = this.playlists.slice(start, end);
  }

  incrementLike(playlist: PlaylistDTO): void {
    playlist.likeCount = (playlist.likeCount ?? 0) + 1;
    playlist.showLikeEffect = true;
    setTimeout(() => {
      playlist.showLikeEffect = false;
    }, 1000);
  }

  onCardClick(playlistId: number): void {
    this.playlistService.getPlaylistItems(playlistId).subscribe(items => {
      this.selectedPlaylistItems = items;
      this.displayModal = true;
    });
  }

  closeModal(): void {
    this.displayModal = false;
  }

  drop(event: CdkDragDrop<PlaylistitemDTO[]>): void {
    moveItemInArray(this.selectedPlaylistItems, event.previousIndex, event.currentIndex);
    const movedItem = this.selectedPlaylistItems[event.currentIndex];
    this.playlistService.updateVideoPosition(movedItem.playListId, movedItem.videoId, event.currentIndex).subscribe();
  }

  removeItem(item: PlaylistitemDTO): void {
    this.selectedPlaylistItems = this.selectedPlaylistItems.filter(i => i !== item);
    this.playlistService.removePlaylistItem(item.playListId, item.videoId).subscribe();
  }
}

