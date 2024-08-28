import { Component } from '@angular/core';
import { VideoDBService } from '../video-db.service';
import { Video } from '../interfaces/video';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-video-db-search',
  templateUrl: './video-db-search.component.html',
  styleUrls: ['./video-db-search.component.css']
})
export class VideoDbSearchComponent {

  inputName:string ='';

  videoName: string | null = null;
  typeId: number | null = null;
  summary: string | null = null;
  genreName: string | null = null;
  seriesName: string | null = null;
  seasonName: string | null = null;

  video!: Video;

  constructor(private route: ActivatedRoute,private videoDbService:VideoDBService){}


  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.videoName = params['videoName'] || null;
      this.typeId = params['typeId'] ? +params['typeId'] : null;
      this.summary = params['summary'] || null;
      this.genreName = params['genreName'] || null;
      this.seriesName = params['seriesName'] || null;
      this.seasonName = params['seasonName'] || null;})

    this.searchVideos();

  }

  searchVideos(): void {
    this.videoDbService.getSearchVideoApi(this.videoName, this.typeId, this.summary, this.genreName, this.seriesName, this.seasonName)
      .subscribe((response) => {
        console.log(response);
        this.video=response[2];
        console.log(this.video);
      });
  }
}
