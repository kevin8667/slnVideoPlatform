import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { VideoDBService } from '../../video-db.service';
import { Video } from '../interfaces/video';
import { Genre } from '../interfaces/genre';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchStateService } from '../../search-state.service';
import { trigger, transition, style, animate } from '@angular/animations';
import { data } from 'jquery';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

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
  providers:[MessageService,ConfirmationService,DialogService],
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

  userID:number = 0;

  videos!: Video[];

  visible: boolean = false;

  videoIDForDelete:number=0;

  videoNameForDelete:string="";

  constructor(
    private route: ActivatedRoute,
    private videoDbService: VideoDBService,
    private searchStateService: SearchStateService,
    private router : Router,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private dialogService: DialogService,
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

    this.videoDbService.user$.subscribe((data) => (this.userID = data));

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

    this.router.navigate([], {
      queryParams: {
        'videoName': null,
      },
      queryParamsHandling: 'merge'
    })

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

  newVideo(){
    this.router.navigate(['/edit/new']);
  }

  deleteVideo(videoId:number){
    this.videoDbService.deleteVideo(videoId);
  }

  onConfirmDelete(videoId:number,videoName:string) {

    if (!this.visible) {
      this.messageService.add({ key: 'confirm', sticky: true, severity: 'warn', summary: '確定要刪除?', detail: '目標：'+videoName });
      this.visible = true;
      this.videoIDForDelete = videoId;
      this.videoNameForDelete = videoName;
    }
  }

  onConfirm() {
    this.messageService.clear('confirm');

    this.visible = false;

    if(this.videoDbService.deleteVideo(this.videoIDForDelete))
    {
      this.messageService.add({
        key: 'global',
        severity: 'success',
        summary: '已成功刪除!',
        detail: `刪除目標： ${this.videoNameForDelete}`
      });
    }

    // setInterval(() => {
    //   location. reload();
    // }, 400);

    this.searchVideos();
  }

  onReject() {
    this.messageService.clear('confirm');
    this.visible = false;
  }

  editVideo(videoId:number){
    this.router.navigate(["/edit"],{queryParams:{videoID:videoId}});
  }
}
