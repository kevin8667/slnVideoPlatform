import { Season } from './interfaces/season';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Video } from './interfaces/video';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class VideoDBService {

  constructor(private httpClient:HttpClient ) { }

  // videoName: string | null = null;
  // typeId: number | null = null;
  // summary: string | null = null;
  // genreName:string | null = null;
  // seriesName:string | null = null;
  // seasonName:string | null = null;

  getVideoApi(){
    this.httpClient.get<Video[]>('https://localhost:7193/api/VideoList')
    .subscribe(videos => {
      console.log(videos);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Video[]>('https://localhost:7193/api/VideoList')
  }
  getVideoApiWithID(id:string){

    const url = `https://localhost:7193/api/VideoList/${id}`;

    this.httpClient.get<Video>(url)
    .subscribe(video => {
      console.log(video);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Video>(url)
  }

  getSeasonWithID(id:string)
  {
    const url=`https://localhost:7193/api/SeasonList/${id}`
    this.httpClient.get<Season>(url)
    .subscribe(season => {
      console.log(season);
    }, error => {
      console.error('Error fetching videos:', error);
    });
    return this.httpClient.get<Season>(url)
  }

  getVideoApiWithTypeID(id:string){

    const url = `https://localhost:7193/api/VideoList/type=${id}`;

    this.httpClient.get<Video[]>(url)
    .subscribe(videos => {
      console.log(videos);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Video[]>(url)
  }
  getSearchVideoApi(videoName: string | null, typeId: number | null, summary: string | null, genreName: string | null, seriesName: string | null, seasonName: string | null){

    const url = `https://localhost:7193/api/VideoList/search`

    let params = new HttpParams();
    if (videoName) {
      params = params.set('videoName', videoName);
    }
    if (typeId) {
      params = params.set('typeId', typeId.toString());
    }
    if (summary) {
      params = params.set('summary', summary);
    }
    if (genreName) {
      params = params.set('genreName', genreName);
    }
    if (seriesName) {
      params = params.set('seriesName', seriesName);
    }
    if (seasonName) {
      params = params.set('seasonName', seasonName);
    }

    this.httpClient.get<Video>(url, { params })
    .subscribe(video => {
      //console.log(video);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Video>(url, { params })
  }
}
