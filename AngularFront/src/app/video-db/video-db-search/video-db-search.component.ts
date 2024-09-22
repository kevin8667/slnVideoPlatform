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
  selectedGenres : string[] =[];
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
      { typeName: '影集', typeId: 2 }
    ];

    this.genreNames = [];

    this.selectedGenres = [];


    this.videoDbService.getGenresApi().subscribe((genres)=>{
      this.genres=genres;
    });



    this.route.queryParams.subscribe(params => {
      this.videoName = params['videoName'] || null;
      this.typeId = params['typeId'] ? +params['typeId'] : null;
      this.summary = params['summary'] || null;

      // 解析逗號分隔的 genreNames 字符串為數組
      this.genreNames = params['genreNames'] ? params['genreNames'].split(',') : [];
      console.log("類型："+this.genreNames);
      this.urlGenre = this.genreNames;
      this.selectedGenres = this.genreNames;
      console.log(this.selectedGenres);

      this.seriesName = params['seriesName'] || null;
      this.seasonName = params['seasonName'] || null;


           // 如果有任何查詢參數存在，則執行搜索
      if (this.videoName || this.typeId || this.summary || (this.genreNames && this.genreNames.length > 0) || this.seriesName || this.seasonName) {
        // 執行搜索操作
        this.searchVideos();
      }
    });
  }

  toggleGenreSelection(genreName: string) {
    if (this.selectedGenres.length!==null &&this.selectedGenres.includes(genreName)) {
      // 如果已經選中，則從選中列表中移除
      this.selectedGenres = this.selectedGenres.filter(name => name !== genreName);
    } else {
      // 如果未選中，則添加到選中列表
      this.selectedGenres.push(genreName);
    }
  }


  isSelected(genreName: string): boolean {
    return this.selectedGenres.includes(genreName);
  }




  searchVideoByFilters() {
    this.videoName = this.inputName;
    if (this.selectedType) {
      this.typeId = this.selectedType.typeId;
    } else {
      this.typeId = null;
    }
    //this.searchVideos();
  }

  searchVideos(): void {

    this.videoDbService.getSearchVideoApi(
      this.videoName,
      this.typeId,
      this.summary,
      this.selectedGenres,
      this.seriesName,
      this.seasonName
    ).subscribe((response) => {
      this.videos = response;
      this.totalRecords = this.videos.length;
      console.log(this.videos);
    });
  }
}
