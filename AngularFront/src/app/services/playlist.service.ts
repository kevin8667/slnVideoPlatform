import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PlaylistDTO } from '../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../interfaces/PlaylistitemDTO';
import { MemberInfoDTO } from '../interfaces/MemberInfoDTO';
import { VideoListDTO } from '../interfaces/VideoListDTO';

@Injectable({
  providedIn: 'root'
})
export class PlaylistService {
  private apiUrl = 'https://localhost:7193/api/PlayList';

  constructor(private http: HttpClient) { }

  getPlaylists(): Observable<PlaylistDTO[]> {
    return this.http.get<PlaylistDTO[]>(this.apiUrl);
  }

  addNewPlaylist(playlist: PlaylistDTO): Observable<PlaylistDTO> {
    return this.http.post<PlaylistDTO>(this.apiUrl, playlist);
  }

  editPlaylist(playlistId: number, playlist: PlaylistDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${playlistId}`, playlist);
  }

  deletePlaylist(id: number): Observable<void> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<void>(url);
  }

  getMemberCreatedPlaylists(memberId: number): Observable<PlaylistDTO[]> {
    return this.http.get<PlaylistDTO[]>(`${this.apiUrl}/created/${memberId}`);
  }

  getMemberAddedPlaylists(memberId: number): Observable<PlaylistDTO[]> {
    return this.http.get<PlaylistDTO[]>(`${this.apiUrl}/added/${memberId}`);
  }

  getMemberCollaboratorPlaylists(memberId: number): Observable<PlaylistDTO[]> {
    return this.http.get<PlaylistDTO[]>(`${this.apiUrl}/collaborator/${memberId}`);
  }

  getPlaylistItems(playlistId: number): Observable<PlaylistitemDTO[]> {
    return this.http.get<PlaylistitemDTO[]>(`${this.apiUrl}/${playlistId}/items`);
  }

  getAllVideos(): Observable<VideoListDTO[]> {
    return this.http.get<VideoListDTO[]>('https://localhost:7193/api/PlayList/videos');
  }

  addPlaylistItems(playListId: number, playlistItems: PlaylistitemDTO[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/playlists/${playListId}/items`, playlistItems);
  }

  addVideoToPlaylist(playlistId: number, video: PlaylistitemDTO): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${playlistId}/items`, video);
  }

  removePlaylistItem(playlistId: number, videoId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${playlistId}/items/${videoId}`);
  }

  updateVideoPosition(playlistId: number, videoId: number, newPosition: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${playlistId}/items/${videoId}/position`, { newPosition });
  }

  getCollaborators(playlistId?: number): Observable<MemberInfoDTO[]> {
    if (playlistId) {
      return this.http.get<MemberInfoDTO[]>(`${this.apiUrl}/collaborators/${playlistId}`);
    } else {
      return this.http.get<MemberInfoDTO[]>(`${this.apiUrl}/collaborators`);
    }
  }

  addCollaboratorToPlaylist(playlistId: number, memberId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${playlistId}/collaborators`, { memberId });
  }

  removeCollaboratorFromPlaylist(playlistId: number, memberId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${playlistId}/collaborators/${memberId}`);
  }
}
