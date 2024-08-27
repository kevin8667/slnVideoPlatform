import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent {

  constructor(private router: Router) {}

  showplay() {
    this.router.navigate(['member'], { relativeTo: this.router.routerState.root.firstChild });
  }

}
