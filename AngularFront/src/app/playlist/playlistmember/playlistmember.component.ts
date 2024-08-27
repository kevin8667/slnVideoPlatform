import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-playlistmember',
  templateUrl: './playlistmember.component.html',
  styleUrls: ['./playlistmember.component.css']
})
export class PlaylistmemberComponent {
  constructor(private router: Router) {}

  backToPlaylist() {
    this.router.navigate(['/playlist']);
  }

}
