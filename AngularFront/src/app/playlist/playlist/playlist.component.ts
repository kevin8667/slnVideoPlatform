import { Component, OnInit } from '@angular/core';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent implements OnInit {
  playlists: PlaylistDTO[] = [];
  paginatedPlaylists: PlaylistDTO[] = [];
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
    playlist.likeCount += 1;
    playlist.showLikeEffect = true;
    setTimeout(() => {
      playlist.showLikeEffect = false;
    }, 1000);
  }
}

