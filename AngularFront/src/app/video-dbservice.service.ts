import { Season } from './interfaces/season';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Video } from './interfaces/video';

@Injectable({
  providedIn: 'root'
})
export class VideoDBServiceService {

  constructor(private httpClient:HttpClient ) { }

  getVideoApi(){
    this.httpClient.get<Video[]>('https://localhost:7193/api/VideoList')
    .subscribe(videos => {
      console.log(videos); // 输出获取的数据，检查是否返回了预期的数据
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Video[]>('https://localhost:7193/api/VideoList')
  }
  getVideoApiWithID(id:string){

    const url = `https://localhost:7193/api/VideoList/${id}`;

    this.httpClient.get<Video>(url)
    .subscribe(video => {
      console.log(video); // 输出获取的数据，检查是否返回了预期的数据
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
      console.log(season); // 输出获取的数据，检查是否返回了预期的数据
    }, error => {
      console.error('Error fetching videos:', error);
    });
    return this.httpClient.get<Season>(url)
  }

  getVideoApiWithTypeID(id:string){

    const url = `https://localhost:7193/api/VideoList/type:${id}`;

    this.httpClient.get<Video[]>(url)
    .subscribe(videos => {
      console.log(videos); // 输出获取的数据，检查是否返回了预期的数据
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Video[]>(url)
  }
}
