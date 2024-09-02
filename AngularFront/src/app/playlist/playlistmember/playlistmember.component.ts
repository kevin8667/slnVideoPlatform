import { Component, OnInit } from '@angular/core';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';

@Component({
  selector: 'app-playlistmember',
  templateUrl: './playlistmember.component.html',
  styleUrls: ['./playlistmember.component.css']
})
export class PlaylistMemberComponent implements OnInit {
  createdPlaylists: PlaylistDTO[] = [];
  addedPlaylists: PlaylistDTO[] = [];
  memberId: number = 5;

  constructor(private playlistService: PlaylistService) { }

  ngOnInit(): void {
    this.loadCreatedPlaylists();
    this.loadAddedPlaylists();
  }

  loadCreatedPlaylists(): void {
    this.playlistService.getMemberCreatedPlaylists(this.memberId).subscribe(
      (data: PlaylistDTO[]) => {
        this.createdPlaylists = data;
      },
      (error) => {
        console.error('Error loading created playlists', error);
      }
    );
  }

  loadAddedPlaylists(): void {
    this.playlistService.getMemberAddedPlaylists(this.memberId).subscribe(
      (data: PlaylistDTO[]) => {
        this.addedPlaylists = data;
      },
      (error) => {
        console.error('Error loading added playlists', error);
      }
    );
  }

  editPlaylist(playlistId: number): void {
    console.log('Editing playlist with ID:', playlistId);
  }

  deletePlaylist(playlistId: number): void {
    console.log('Deleting playlist with ID:', playlistId);
  }
}
