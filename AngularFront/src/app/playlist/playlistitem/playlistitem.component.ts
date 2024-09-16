import { Component, Input, Output, EventEmitter } from '@angular/core';
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

  playItem() {
    this.displayOverlay = true;

    setTimeout(() => {
      this.initializePlayer();
    }, 100);
  }
  constructor(private service: PlaylistService) {
    service.loadVideoCss();
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
