import { Component, OnInit } from '@angular/core';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../../interfaces/PlaylistitemDTO';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css'],
})
export class PlaylistComponent implements OnInit {
  playlists: PlaylistDTO[] = [];
  paginatedPlaylists: PlaylistDTO[] = [];
  filteredVideos: PlaylistitemDTO[] = [];
  selectedVideos: PlaylistitemDTO[] = [];
  selectedPlaylistItems: PlaylistitemDTO[] = [];
  displayModal: boolean = false;
  rows: number = 20;
  first: number = 0;
  videoName: string = '';

  constructor(private playlistService: PlaylistService) {}

  ngOnInit(): void {
    this.playlistService.getPlaylists().subscribe((data) => {
      console.log('API Response:', data); // 檢查 API 返回數據
      this.playlists = data.map((playlist) => {
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

  handleKeydown(event: KeyboardEvent) {
    if (event.key === 'Enter' && this.videoName.trim()) {
      const firstSuggestion = this.filteredVideos[0];

      if (firstSuggestion) {
        this.addSearchChip({ value: firstSuggestion });
      } else {
        const newVideo: PlaylistitemDTO = {
          playListId: 0,
          videoId: Date.now(),
          videoPosition: 0,
          videoName: this.videoName.trim(),
          thumbnailPath: '',
          episode: null,
        };

        this.addSearchChip({ value: newVideo });
      }

      this.videoName = '';
    }
  }

  addSearchChip(event: any): void {
    const video: PlaylistitemDTO = event.value;
    if (!this.selectedVideos.some((v) => v.videoId === video.videoId)) {
      this.selectedVideos.push(video);
    }
    console.log('Selected Videos:', this.selectedVideos);
  }

  searchVideos(event: any): void {
    let query = event.query;
    this.filteredVideos = [];

    this.playlists.forEach((playlist) => {
      if (playlist.videos) {
        const filtered =
          playlist.videos.filter((video) => {
            console.log('Video being checked:', video);
            return video.videoName && video.videoName.includes(query);
          }) || [];
        this.filteredVideos = [...this.filteredVideos, ...filtered];
      }
    });

    console.log('Filtered Videos:', this.filteredVideos);
  }

  searchPlaylists(): void {
    console.log('Searching with: ', this.selectedVideos);

    const filteredPlaylists = this.playlists.filter((playlist) => {
      console.log('Checking playlist: ', playlist);
      if (!playlist.videos || playlist.videos.length === 0) {
        console.log('No videos in this playlist');
        return false;
      }

      return this.selectedVideos.every((video) =>
        playlist.videos?.some(
          (v) =>
            v.videoName &&
            v.videoName
              .trim()
              .toLowerCase()
              .includes(video.videoName.trim().toLowerCase())
        )
      );
    });

    this.paginatedPlaylists = filteredPlaylists.slice(0, this.rows);
    console.log('Filtered Playlists: ', this.paginatedPlaylists);
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
