import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { VideoDBService } from '../video-db.service';
import { Video } from '../interfaces/video';
import { ActivatedRoute } from '@angular/router';
import { SearchStateService } from '../search-state.service';

interface VideoType
{
  typeName:string;
  typeId:number;
}

@Component({
  selector: 'app-video-db-search',
  templateUrl: './video-db-search.component.html',
  styleUrls: ['./video-db-search.component.css'],
  encapsulation:ViewEncapsulation.None
})

export class VideoDbSearchComponent implements OnInit {

  inputName: string = '';
  keyword: string[] | undefined;
  types: VideoType[] | undefined;
  selectedType: VideoType | undefined;

  videoName: string | null = null;
  typeId: number | null = null;
  summary: string | null = null;
  genreName: string | null = null;
  seriesName: string | null = null;
  seasonName: string | null = null;
  pageSize: number = 15;

  videos!: Video[];

  constructor(
    private route: ActivatedRoute,
    private videoDbService: VideoDBService,
    private searchStateService: SearchStateService
  ) {}

  ngOnInit() {
    this.types = [
      { typeName: '電影', typeId: 1 },
      { typeName: '影集', typeId: 2 },
      { typeName: '其他', typeId: 3 }
    ];

    // const savedParams = this.searchStateService.getSearchParams();
    // const savedResults = this.searchStateService.getSearchResults();

    // if (savedParams && savedResults) {
    //   this.videoName = savedParams.videoName;
    //   this.typeId = savedParams.typeId;
    //   this.summary = savedParams.summary;
    //   this.genreName = savedParams.genreName;
    //   this.seriesName = savedParams.seriesName;
    //   this.seasonName = savedParams.seasonName;

    //   this.videos = savedResults;
    // } else {
    //   this.searchVideoByFilters();
    // }
  }

  searchVideoByFilters() {
    this.videoName = this.inputName;
    if (this.selectedType != undefined) {
      this.typeId = this.selectedType.typeId;
    }else
    {
      this.typeId = null;
    }
    this.searchVideos();
  }

  searchVideos(): void {
    this.videoDbService.getSearchVideoApi(
      this.videoName,
      this.typeId,
      this.summary,
      this.genreName,
      this.seriesName,
      this.seasonName
    ).subscribe((response) => {
      console.log(this.typeId);
      this.videos = response;

    //   this.searchStateService.saveSearchParams({
    //     videoName: this.videoName,
    //     typeId: this.typeId,
    //     summary: this.summary,
    //     genreName: this.genreName,
    //     seriesName: this.seriesName,
    //     seasonName: this.seasonName
    //   });
    //   this.searchStateService.saveSearchResults(this.videos);
      });
  }
}