import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { PlaylistitemDTO } from 'src/app/interfaces/PlaylistitemDTO';
import { PlaylistService } from 'src/app/services/playlist.service';
import videojs from 'video.js';
import 'videojs-youtube';

@Component({
  selector: 'app-playlistitem',
  templateUrl: './playlistitem.component.html',
  styleUrls: ['./playlistitem.component.css'],
})
export class PlaylistitemComponent {
  @Input()
  item!: PlaylistitemDTO;

  @Output()
  remove: EventEmitter<void> = new EventEmitter<void>();

  player: any;
  displayOverlay: boolean = false;
  displayButton: boolean = false; // 用來控制按鈕的顯示

  constructor(private service: PlaylistService, private router: Router) {
    service.loadVideoCss();
  }

  playItem() {
    this.displayOverlay = true;

    setTimeout(() => {
      this.initializePlayer();
    }, 100);
  }

  initializePlayer() {
    if (this.player) {
      this.player.dispose();
    }
    this.player = videojs('youtube-player', {
      controls: true,
      autoplay: true,
      techOrder: ['youtube'],
      sources: [
        {
          type: 'video/youtube',
          src: 'https://www.youtube.com/watch?v=FnCSee1k-ek',
        },
      ],
    });
    // 監聽 timeupdate 事件
    this.player.on('timeupdate', () => {
      if (this.player.currentTime() >= 10 && this.player.currentTime() < 11) {
        this.displayButton = true; // 在10秒時顯示按鈕
      }
    });
  }

  // 按鈕點擊方法
  goToVideo() {
    this.router.navigate(['/video-db/details/1'], { queryParams: { showOverlay: 'true' } });
  }

  closeOverlay() {
    this.displayOverlay = false;
    if (this.player) {
      this.player.dispose();
    }
  }

  ngOnDestroy() {
    if (this.player) {
      this.player.dispose();
    }
  }

  getThumbnailUrl(thumbnailPath: string | null): string {
    return thumbnailPath ?? '/assets/img/movie.png';
  }
}
