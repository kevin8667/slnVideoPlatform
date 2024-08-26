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
}
