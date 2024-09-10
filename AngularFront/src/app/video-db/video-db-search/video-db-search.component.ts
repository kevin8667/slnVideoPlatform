import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { VideoDBService } from '../../video-db.service';
import { Video } from '../interfaces/video';
import { Genre } from '../interfaces/genre';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchStateService } from '../../search-state.service';
import { trigger, transition, style, animate } from '@angular/animations';
import { data } from 'jquery';

interface VideoType
{
  typeName:string;
  typeId:number;
}

interface AutoCompleteCompleteEvent {
  originalEvent: Event;
  query: string;
}

@Component({
  selector: 'app-video-db-search',
  templateUrl: './video-db-search.component.html',
  styleUrls: ['./video-db-search.component.css'],
  // encapsulation:ViewEncapsulation.None,
  animations: [
    trigger('listAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(20px)' }),
        animate('0.5s ease-in-out',
                style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('0.5s ease-in-out',
                style({ opacity: 0, transform: 'translateY(20px)' }))
      ])
    ])
  ]
})

export class VideoDbSearchComponent implements OnInit {

  inputName: string = '';
  keyword: string[] | undefined;
  types: VideoType[] | undefined;
  selectedType: VideoType | undefined;

  genres :Genre[] =[];
  selectedGenres : Genre[] =[];
  urlGenre:string[] |null = null;

  videoName: string | null = null;
  typeId: number | null = null;
  summary: string | null = null;
  genreNames: string[]= [];
  seriesName: string | null = null;
  seasonName: string | null = null;

  pageSizeOptions:number[]=[5,10,15];
  pageSize: number = 15;

  totalRecords:number=0;


  videos!: Video[];

  constructor(
    private route: ActivatedRoute,
    private videoDbService: VideoDBService,
    private searchStateService: SearchStateService,
    private router : Router
  ) {}

  ngOnInit() {
    this.types = [
      { typeName: '電影', typeId: 1 },
      { typeName: '影集', typeId: 2 },
      { typeName: '其他', typeId: 3 }
    ];

    this.genreNames = [];

    this.videoDbService.getGenresApi().subscribe((genres)=>{
      this.genres=genres;
      console.log(genres)
    });

    this.route.queryParams.subscribe(params => {
      this.videoName = params['videoName'] || null;
      this.typeId = params['typeId'] ? +params['typeId'] : null;
      this.summary = params['summary'] || null;

      // 解析逗號分隔的 genreNames 字符串為數組
      this.genreNames = params['genreNames'] ? params['genreNames'].split(',') : null;
      console.log("類型："+this.genreNames);
      this.urlGenre = this.genreNames;

      this.seriesName = params['seriesName'] || null;
      this.seasonName = params['seasonName'] || null;


           // 如果有任何查詢參數存在，則執行搜索
      if (this.videoName || this.typeId || this.summary || (this.genreNames && this.genreNames.length > 0) || this.seriesName || this.seasonName) {
        // 執行搜索操作
        this.searchVideos();
      }
    });
  }




  searchVideoByFilters() {
    this.videoName = this.inputName;
    if (this.selectedType) {
      this.typeId = this.selectedType.typeId;
    } else {
      this.typeId = null;
    }
    this.searchVideos();
  }

  searchVideos(): void {

    const genreNames = this.selectedGenres?.map(genre => genre.genreName) || [];
    this.videoDbService.getSearchVideoApi(
      this.videoName,
      this.typeId,
      this.summary,
      this.urlGenre,
      this.seriesName,
      this.seasonName
    ).subscribe((response) => {
      this.videos = response;
      this.totalRecords = this.videos.length;
      console.log(this.videos);
    });
  }
}
