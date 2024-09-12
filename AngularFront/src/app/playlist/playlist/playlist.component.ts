import { Component, OnInit } from '@angular/core';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../../interfaces/PlaylistitemDTO';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css'],
})
export class PlaylistComponent implements OnInit {
  playlists: PlaylistDTO[] = [];
  paginatedPlaylists: PlaylistDTO[] = [];
  allPlaylists: PlaylistDTO[] = [];
  filteredVideos: PlaylistitemDTO[] = [];
  selectedVideos: PlaylistitemDTO[] = [];
  selectedPlaylistItems: PlaylistitemDTO[] = [];
  displayModal: boolean = false;
  rows: number = 20;
  first: number = 0;
  videoName: string = '';

  constructor(
    private playlistService: PlaylistService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadPlaylists();
  }

  loadPlaylists(): void {
    this.playlistService.getPlaylists().subscribe((data) => {
      this.allPlaylists = data;
      this.playlists = [...this.allPlaylists];
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

  addVideoToSelected(video: PlaylistitemDTO): void {
    if (!this.selectedVideos.some((v) => v.videoId === video.videoId)) {
      this.selectedVideos.push(video);
    }
  }

  searchVideos(event: any): void {
    const query = event.query.toLowerCase();
    this.filteredVideos = [];

    this.playlists.forEach((playlist) => {
      if (playlist.videos) {
        const filtered = playlist.videos.filter((video) =>
          video.videoName.toLowerCase().includes(query)
        );
        this.filteredVideos = [...this.filteredVideos, ...filtered];
      }
    });
  }

  searchPlaylists(): void {
    if (!this.selectedVideos || this.selectedVideos.length === 0) {
      console.log('No selected videos, nothing to search.');
      return;
    }

    const filteredPlaylists = this.allPlaylists.filter((playlist) => {
      if (!playlist.videos || playlist.videos.length === 0) {
        return false;
      }

      return this.selectedVideos.every((video) =>
        playlist.videos!.some(
          (v) =>
            v.videoName &&
            v.videoName
              .trim()
              .toLowerCase()
              .includes(video.videoName.trim().toLowerCase())
        )
      );
    });
    this.playlists = filteredPlaylists;
    this.paginate({ first: 0, rows: this.rows });
    this.cdr.detectChanges();
  }

  incrementLike(playlist: PlaylistDTO): void {
    playlist.likeCount = (playlist.likeCount ?? 0) + 1;
    playlist.showLikeEffect = true;
    setTimeout(() => {
      playlist.showLikeEffect = false;
    }, 1000);
  }

  onCardClick(playlistId: number): void {
    this.playlistService.getPlaylistItems(playlistId).subscribe((items) => {
      this.selectedPlaylistItems = items;
      this.displayModal = true;
    });
  }

  closeModal(): void {
    this.displayModal = false;
  }

  drop(event: CdkDragDrop<PlaylistitemDTO[]>): void {
    moveItemInArray(
      this.selectedPlaylistItems,
      event.previousIndex,
      event.currentIndex
    );
    const movedItem = this.selectedPlaylistItems[event.currentIndex];
    this.playlistService
      .updateVideoPosition(
        movedItem.playListId,
        movedItem.videoId,
        event.currentIndex
      )
      .subscribe();
  }

  removeItem(item: PlaylistitemDTO): void {
    this.selectedPlaylistItems = this.selectedPlaylistItems.filter(
      (i) => i !== item
    );
    this.playlistService
      .removePlaylistItem(item.playListId, item.videoId)
      .subscribe();
  }
}
