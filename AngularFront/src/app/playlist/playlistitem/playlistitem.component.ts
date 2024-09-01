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
  remove: EventEmitter<PlaylistitemDTO> = new EventEmitter<PlaylistitemDTO>();

  getThumbnailUrl(thumbnailId: number | null): string {
    return thumbnailId ? `https://path-to-your-thumbnails/${thumbnailId}.jpg` : 'assets/img/movie.png';
  }

  removeItem(item: PlaylistitemDTO): void {
    this.remove.emit(item);
  }
}
