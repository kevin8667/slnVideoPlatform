import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PlaylistitemDTO } from 'src/app/interfaces/PlaylistitemDTO';

@Component({
  selector: 'app-playlistitem',
  templateUrl: './playlistitem.component.html',
  styleUrls: ['./playlistitem.component.css']
})
export class PlaylistitemComponent {
  @Input()
  item!: PlaylistitemDTO;

  @Output()
  remove: EventEmitter<void> = new EventEmitter<void>();

  getThumbnailUrl(thumbnailId: number | null): string {
    return thumbnailId ? `https://path-to-your-thumbnails/${thumbnailId}.jpg` : 'assets/img/movie.png';
  }

  removeItem(): void {
    const element = document.querySelector(`[data-id="${this.item.videoId}"]`);
    if (element) {
      element.classList.add('removing');
    }

    setTimeout(() => {
      this.remove.emit();
    }, 700);
  }
}
