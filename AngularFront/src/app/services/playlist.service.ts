import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PlaylistDTO } from '../interfaces/PlaylistDTO';

@Injectable({
  providedIn: 'root'
})
export class PlaylistService {
  private apiUrl = 'https://localhost:7193/api/PlayList';

  constructor(private http: HttpClient) { }

  getPlaylists(): Observable<PlaylistDTO[]> {
    return this.http.get<PlaylistDTO[]>(this.apiUrl);
  }
}
