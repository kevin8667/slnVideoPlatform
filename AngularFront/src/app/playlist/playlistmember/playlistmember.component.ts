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
  collaboratorPlaylists: PlaylistDTO[] = [];
  memberId: number = 5;

  constructor(private playlistService: PlaylistService) { }

  ngOnInit(): void {
    this.loadCreatedPlaylists();
    this.loadAddedPlaylists();
    this.loadCollaboratorPlaylists();
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

  loadCollaboratorPlaylists(): void {
    this.playlistService.getMemberCollaboratorPlaylists(this.memberId).subscribe(
      (data: PlaylistDTO[]) => {
        this.collaboratorPlaylists = data;
      },
      (error) => {
        console.error('Error loading collaborator playlists', error);
      }
    );
  }

  triggerDeleteAnimation(playlistId: number): void {
    const element = document.getElementById(`playlist-${playlistId}`);
    const trashIcon = document.getElementById('trashIcon');

    if (element && trashIcon) {
      trashIcon.classList.add('trash-spin');

      const trashIconRect = trashIcon.getBoundingClientRect();
      const elementRect = element.getBoundingClientRect();

      const xOffset = trashIconRect.left - elementRect.left;
      const yOffset = trashIconRect.top - elementRect.top;

      element.style.transform = `translate(${xOffset}px, ${yOffset}px) scale(0)`;
      element.style.transition = 'transform 1s ease-in-out';

      setTimeout(() => {
        this.deletePlaylist(playlistId);
        trashIcon.classList.remove('trash-spin');
      }, 1000);
    }
  }

  deletePlaylist(playlistId: number): void {
    this.playlistService.deletePlaylist(playlistId).subscribe(
      () => {
        console.log(`Playlist with ID ${playlistId} deleted`);
        this.loadCreatedPlaylists();
      },
      (error) => {
        console.error('Error deleting playlist', error);
      }
    );
  }

  addNewPlaylist(){

  }

  editPlaylist(playlistId: number): void {
    console.log('Editing playlist with ID:', playlistId);
  }
}
