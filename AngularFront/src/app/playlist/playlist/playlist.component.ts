import { Component, OnInit } from '@angular/core';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../../interfaces/PlaylistitemDTO';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ChangeDetectorRef } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css'],
  providers: [ConfirmationService, MessageService],
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
    private cdr: ChangeDetectorRef,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loadPlaylists();
  }

  isProcessing: boolean = false;

  checkAndConfirmAddToFavorites(event: Event, playlist: PlaylistDTO): void {
    event.stopPropagation();

    if (this.isProcessing) {
      console.log('操作正在進行中，無法再次點擊');
      return;
    }

    if (playlist.playListId !== undefined) {
      this.showConfirmation(event, playlist);
    } else {
      console.log('播放清單 ID 未定義');
      this.messageService.add({
        severity: 'error',
        summary: '錯誤',
        detail: '播放清單 ID 未定義',
      });
    }
  }

  showConfirmation(event: Event, playlist: PlaylistDTO): void {
    const target = event?.target as EventTarget | null;

    this.confirmationService.confirm({
      target: target || undefined,
      message: '確定要將這個播放清單加入收藏嗎？',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.isProcessing = true;
        this.finalizeAddToFavorites(playlist);
      },
      reject: () => {
        console.log('用戶取消操作');
        this.isProcessing = false;
      },
    });
  }

  finalizeAddToFavorites(playlist: PlaylistDTO): void {
    this.playlistService.addToFavorites(playlist.playListId!).subscribe(
      () => {
        playlist.addedCount = (playlist.addedCount ?? 0) + 1;
        this.messageService.add({
          severity: 'success',
          summary: '成功',
          detail: '成功將播放清單添加到收藏！',
        });
        this.isProcessing = false;
      },
      (error) => {
        console.log('最終添加失敗，進入錯誤處理：', error);
        this.handleError(error);
      }
    );
  }

  private handleError(error: any): void {
    console.log('錯誤回應訊息', error);

    this.isProcessing = false;

    let errorMessage =
      error?.error?.message || error?.message || '發生未知錯誤';

    if (error.status === 400) {
      if (errorMessage.includes('已經被添加')) {
        this.messageService.add({
          severity: 'warn',
          summary: '提示',
          detail: '該播放清單已經被添加到收藏中。',
        });
      } else if (errorMessage.includes('無法添加')) {
        this.messageService.add({
          severity: 'warn',
          summary: '提示',
          detail: '無法添加您自己創建的播放清單。',
        });
      } else {
        this.messageService.add({
          severity: 'error',
          summary: '錯誤',
          detail: errorMessage,
        });
      }
    } else {
      this.messageService.add({
        severity: 'error',
        summary: '錯誤',
        detail: '發生未知錯誤，請稍後再試。',
      });
    }
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

  incrementLike(playlist: PlaylistDTO) {
    const newLikeCount = (playlist.likeCount ?? 0) + 1;
    this.playlistService
      .updateLikeCount(playlist.playListId ?? 0, newLikeCount)
      .subscribe(
        (updatedPlaylist) => {
          playlist.likeCount = updatedPlaylist.likeCount;
          playlist.showLikeEffect = true;
          setTimeout(() => (playlist.showLikeEffect = false), 1000);
        },
        (error) => {
          console.error('Error updating like count', error);
        }
      );
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
