import { Component, OnInit } from '@angular/core';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { PlaylistService } from '../../services/playlist.service'; // 確保 PlaylistService 正確導入
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { forkJoin, map } from 'rxjs';

@Component({
  selector: 'app-addtoplaylist',
  templateUrl: './addtoplaylist.component.html',
  styleUrls: ['./addtoplaylist.component.css']
})
export class AddtoplaylistComponent implements OnInit {
  playlists: PlaylistDTO[] = [];
  videoId: number;

  constructor(
    private playlistService: PlaylistService, // 通過依賴注入獲取 PlaylistService
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig // 接收傳遞進來的 data
  ) {
    this.videoId = config.data.videoId; // 接收 videoId
  }

  ngOnInit(): void {
    const memberId = 5; // 假設這裡是默認的 memberId

    forkJoin({
      createdPlaylists: this.playlistService.getMemberCreatedPlaylists(memberId),
      collaboratorPlaylists: this.playlistService.getMemberCollaboratorPlaylists(memberId)
    })
    .pipe(
      map(({ createdPlaylists, collaboratorPlaylists }) => {
        // 標示 createdPlaylists 為 "自建"
        const markedCreatedPlaylists = createdPlaylists.map(playlist => ({
          ...playlist,
          isCreatedByUser: true // 自建的播放清單
        }));

        // 標示 collaboratorPlaylists 為 "協作"
        const markedCollaboratorPlaylists = collaboratorPlaylists.map(playlist => ({
          ...playlist,
          isCreatedByUser: false // 協作的播放清單
        }));

        return [...markedCreatedPlaylists, ...markedCollaboratorPlaylists].sort((a, b) => {
          const aId = a.playListId ?? 0;
          const bId = b.playListId ?? 0;
          return bId - aId;
        });
      })
    )
    .subscribe((sortedPlaylists) => {
      this.playlists = sortedPlaylists;
    });
  }

  selectPlaylist(playlist: PlaylistDTO) {
    console.log('影片ID:', this.videoId, '被加入到播放清單:', playlist);
    // 處理將影片加入到播放清單的邏輯
    this.ref.close(playlist); // 關閉對話框，返回選擇的播放清單
  }
}
