import { Season } from './video-db/interfaces/season';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Video } from './video-db/interfaces/video';
import { HttpParams } from '@angular/common/http';
// import { PagedResult } from './video-db/interfaces/PagedResult';
import {Genre} from './video-db/interfaces/genre';
import { Actor } from './video-db/interfaces/actor';
import { CreateVideoDTO } from './video-db/interfaces/CreateVideoDTO';
import { Observable } from 'rxjs/internal/Observable';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from './auth.service';
import { RatingDTO } from './video-db/interfaces/ratingDTO';
import { data } from 'jquery';
import { KeywordDTO } from './video-db/interfaces/keyword';

@Injectable({
  providedIn: 'root'
})
export class VideoDBService {

  private userSubject = new BehaviorSubject<number>(0);

  public user$ = this.userSubject.asObservable();

  constructor(private httpClient:HttpClient , private auth: AuthService ) {
    this.loadMember();
   }

  // videoName: string | null = null;
  // typeId: number | null = null;
  // summary: string | null = null;
  // genreName:string | null = null;
  // seriesName:string | null = null;
  // seasonName:string | null = null;

  loadMember() {
    this.auth.MemberBehaviorData?.subscribe({
      next: (data) => {
        // console.log(data?.MemberId);
        // 檢查 data 是否為 null 並做防範性處理
        if (
          data &&
          typeof data.memberID === 'number'
        ) {
          // 更新 userSubject 的值
          this.userSubject.next(
            data.memberID);
        } else {
          // 如果資料無效，更新為 null
          this.userSubject.next(0);
        }
      },
      error(err) {
        console.error('獲取會員失敗', err);
      },
    });
  }


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
  getSearchVideoApi(
    videoName: string | null,
    typeId: number | null,
    summary: string | null,
    genreNames: string[] | null,
    keywords:string[]|null,
    seriesName: string | null,
    seasonName: string | null
  ) {
    const url = `https://localhost:7193/api/VideoList/search`;
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

    if (genreNames && genreNames.length > 0) {
      genreNames.forEach(genreName => {
        params = params.append('genreNames', genreName);
      });
    }

    if (keywords && keywords.length > 0) {
      keywords.forEach(keyword => {
        params = params.append('keywords', keyword);
      });
    }

    if (seriesName) {
      params = params.set('seriesName', seriesName);
    }
    if (seasonName) {
      params = params.set('seasonName', seasonName);
    }

    return this.httpClient.get<Video[]>(url, { params });
  }

  getImagesByVideoID(id:string)
  {
    const url = `https://localhost:7193/api/ImageForVideoList/video=${id}`;

    this.httpClient.get<string[]>(url)
    .subscribe(images => {
      console.log(images);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<string[]>(url)
  }

  getGenresApi(){
    const url = `https://localhost:7193/api/GenreList`;

    this.httpClient.get<Genre[]>(url)
    .subscribe(genres => {
      console.log(genres);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Genre[]>(url)
  }

  getGenreIdWithName(name:string)
  {
    const url = `https://localhost:7193/api/GenreList/name=${name}`;

    this.httpClient.get<number>(url)
    .subscribe(id => {
      console.log(id);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<number>(url)
  }

  getActorWithId(id:string)
  {
    const url = `https://localhost:7193/api/ActorList/${id}`;

    this.httpClient.get<Actor>(url)
    .subscribe(Actor => {
      console.log(Actor);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Actor>(url)
  }

  getVideosWithActorId(actorId:string)
  {
    const url = `https://localhost:7193/api/CastList/actor/${actorId}`;

    this.httpClient.get<Video[]>(url)
    .subscribe(videos => {
      console.log(videos);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Video[]>(url)
  }

  getActorsByVideo(videoId:string){
    const url = `https://localhost:7193/api/CastList/GetActorsByVideo/${videoId}`;

    this.httpClient.get<Actor[]>(url)
    .subscribe(actors => {
      console.log(actors);
    }, error => {
      console.error('Error fetching videos:', error);
    });

    return this.httpClient.get<Actor[]>(url)

  }

  createVideo(video:CreateVideoDTO): Observable<Video> {

    const apiUrl = 'https://your-api-url/api/videos';

    return this.httpClient.post<Video>(apiUrl, video);
  }

  createNewRating(rating:RatingDTO)
  {
    const apiUrl = 'https://localhost:7193/api/MemberRatings';

    return this.httpClient.post<RatingDTO>(apiUrl, rating);
  }

  getRatingsByVideo(videoId:string)
  {
    const apiUrl = `https://localhost:7193/api/MemberRatings/Video=${videoId}`;

    return this.httpClient.get<RatingDTO[]>(apiUrl);
  }

  getRatingByData(memberId:string,videoId:string)
  {
    const apiUrl = `https://localhost:7193/api/MemberRatings/Member=${memberId}&Video=${videoId}`;

    return this.httpClient.get<RatingDTO>(apiUrl);
  }

  deleteVideo(videoId:number){
    const url =`https://localhost:7193/api/VideoList/${videoId}`

    this.httpClient.delete<Video>(url).subscribe((data)=>{
    })

    return this.httpClient.delete<Video>(url);
  }

  addKeyword(videoId: string, keyword: string) {
    const url = `https://localhost:7193/api/KeywordForVideoList/AddKeywordToVideo/${videoId}`;
    
    const headers = { 'Content-Type': 'text/plain' };
  
    // 傳送單純的字串作為關鍵字
    return this.httpClient.post(url, keyword, { headers, responseType: 'text' });
  }

  getKeywordByVideo(videoId:string){
    const url= `https://localhost:7193/api/KeywordForVideoList/GetKeywordsByVideo/${videoId}`

    return this.httpClient.get<KeywordDTO[]>(url);
  }

  deleteKeywordFromVideo(videoId: string, keywordId: number) {
    const url = `https://localhost:7193/api/KeywordForVideoList/remove/${videoId}/${keywordId}`;
    return this.httpClient.delete(url, { responseType: 'text' });
  }

  searchAllKeywords() {
    const url = `https://localhost:7193/api/KeywordForVideoList/SearchAllKeywords`;
    return this.httpClient.get<KeywordDTO[]>(url);
  }

  searchKeywords(query:string){
    const url = `https://localhost:7193/api/KeywordForVideoList/search`;

    // 使用 HttpParams 傳遞查詢參數
    let params = new HttpParams().set('query', query);

    // 發送 GET 請求並返回關鍵字列表
    return this.httpClient.get<string[]>(url, { params });
  }

}
