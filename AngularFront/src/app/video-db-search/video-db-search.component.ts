import { event } from 'jquery';
import { Component, OnInit } from '@angular/core';
import { VideoDBService } from '../video-db.service';
import { Video } from '../interfaces/video';
import { ActivatedRoute } from '@angular/router';
import { PagedResult } from '../interfaces/PagedResult';


interface VideoType
{
  typeName:string;
  typeId:number;
}

@Component({
  selector: 'app-video-db-search',
  templateUrl: './video-db-search.component.html',
  styleUrls: ['./video-db-search.component.css']
})

export class VideoDbSearchComponent implements OnInit {

  inputName:string ='';
  keyword:string[]|undefined;
  types:VideoType[]|undefined;
  selectedType:VideoType|undefined;

  videoName: string | null = null;
  typeId: number | null = null;
  summary: string | null = null;
  genreName: string | null = null;
  seriesName: string | null = null;
  seasonName: string | null = null;
  totalResults: number = 0;
  pageSize: number = 10;
  currentPage:number =1;
  first:number = 0;

  videos!: Video[];

  constructor(private route: ActivatedRoute,private videoDbService:VideoDBService){}


  ngOnInit() {
    this.types=
    [
      {typeName:'電影', typeId:1},
      {typeName:'影集', typeId:2},
      {typeName:'其他', typeId:3}
    ];


    // this.route.queryParams.subscribe(params => {
    //   this.videoName = params['videoName'] || null;
    //   this.typeId = params['typeId'] ? +params['typeId'] : null;
    //   this.summary = params['summary'] || null;
    //   this.genreName = params['genreName'] || null;
    //   this.seriesName = params['seriesName'] || null;
    //   this.seasonName = params['seasonName'] || null;})

    // this.searchVideos();

  }


  searchName()
  {
    this.videoName = this.inputName;
    const pageNumber = 0;  // 預設為第1頁
    const pageSize = this.pageSize || 10;  // 預設每頁顯示10條記錄
    //this.searchVideos();
    this.loadVideos({ page: pageNumber, rows: pageSize });
  }

  onPageChange(event: any) {
    const pageNumber = event.page + 1;
    const pageSize = event.rows;
    this.totalResults = 30;
    this.loadVideos({pageNumber, pageSize});
  }

  loadVideos(event: any) {
    console.log('Event:', event);
    const pageNumber = event.page + 1; // PrimeNG page index starts from 0
    const pageSize = event.rows || this.pageSize;
    console.log(pageNumber);
    this.videoDbService.getSearchVideoApi(this.videoName, this.typeId, this.summary, this.genreName, this.seriesName, this.seasonName, pageNumber, pageSize)
        .subscribe((result: PagedResult<Video>) => {
            this.videos = result.items;
            this.totalResults = result.totalResults;
        });
  }

  test(event:any){
    console.log('Event:', event);
  }


  // searchVideos(): void {
  //   this.videoDbService.getSearchVideoApi(this.videoName, this.typeId, this.summary, this.genreName, this.seriesName, this.seasonName,pageNumber, this.pageSize)
  //     .subscribe((response) => {
  //       console.log(response)
  //       this.videos=response;
  //       console.log(this.videos);
  //     });
  // }
}
