import { Component, OnInit } from '@angular/core';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { VideoAddToDTO } from '../../interfaces/VideoAddToDTO';
import { forkJoin, map } from 'rxjs';
import { MessageService } from 'primeng/api'; // 導入 MessageService 顯示提示訊息

@Component({
  selector: 'app-addtoplaylist',
  templateUrl: './addtoplaylist.component.html',
  styleUrls: ['./addtoplaylist.component.css'],
  providers: [MessageService] // 註冊 MessageService
})
export class AddtoplaylistComponent implements OnInit {
  playlists: PlaylistDTO[] = [];
  videoId: number;

  constructor(
    private playlistService: PlaylistService,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig, // 接收傳遞進來的 data
    private messageService: MessageService // 通過依賴注入 MessageService
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

    const payload: VideoAddToDTO = {
      playListId: playlist.playListId??0,
      videoId: this.videoId // 傳入選中的影片 ID
    };

    this.playlistService.addVideoToPlaylist(playlist.playListId??0, payload)
      .subscribe(response => {
        this.messageService.add({ severity: 'success', summary: '成功', detail: '影片成功加入播放清單！' });
      }, error => {
        if (error.status === 400) {
          this.messageService.add({ severity: 'warn', summary: '提醒', detail: '影片已經存在於播放清單中！' });
        } else {
          this.messageService.add({ severity: 'error', summary: '錯誤', detail: '添加影片到播放清單失敗！' });
        }
      });
    }
}

