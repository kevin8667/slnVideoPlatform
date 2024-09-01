import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PlaylistDTO } from '../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../interfaces/PlaylistitemDTO';

@Injectable({
  providedIn: 'root'
})
export class PlaylistService {
  private apiUrl = 'https://localhost:7193/api/PlayList';

  constructor(private http: HttpClient) { }

  getPlaylists(): Observable<PlaylistDTO[]> {
    return this.http.get<PlaylistDTO[]>(this.apiUrl);
  }

  getPlaylistItems(playlistId: number): Observable<PlaylistitemDTO[]> {
    return this.http.get<PlaylistitemDTO[]>(`${this.apiUrl}/${playlistId}/items`);
  }

  removePlaylistItem(playlistId: number, videoId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${playlistId}/items/${videoId}`);
  }

  updateVideoPosition(playlistId: number, videoId: number, newPosition: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${playlistId}/items/${videoId}/position`, { newPosition });
  }
}
