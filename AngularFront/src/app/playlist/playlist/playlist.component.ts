import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
  rows: number = 20; // 默认显示20条记录
  first: number = 0;

  constructor(private playlistService: PlaylistService) {}

  ngOnInit(): void {
    this.playlistService.getPlaylists().subscribe(data => {
      this.playlists = data.map(playlist => {
        return { ...playlist, showLikeEffect: false }; // 初始化 showLikeEffect 属性为 false
      });
      this.paginate({ first: this.first, rows: this.rows }); // 初始化分页数据
    });
  }

  paginate(event: any): void {
    // 更新分页信息
    this.first = event.first;
    this.rows = event.rows;
    const start = this.first;
    const end = this.first + this.rows;
    this.paginatedPlaylists = this.playlists.slice(start, end);
  }

  incrementLike(playlist: PlaylistDTO): void {
    // 每次点击增加 likeCount 并显示 +1 动画
    playlist.likeCount += 1;
    playlist.showLikeEffect = true;

    setTimeout(() => {
      playlist.showLikeEffect = false; // 动画结束后隐藏
    }, 1000);
  }
}

