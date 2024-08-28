import { Season } from './../interfaces/season';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VideoDBService } from '../video-db.service';
import { Video } from '../interfaces/video';

@Component({
  selector: 'app-video-detail',
  templateUrl: './video-detail.component.html',
  styleUrls: ['./video-detail.component.css']
})
export class VideoDetailComponent implements OnInit{

  video:Video;

  season: Season | undefined;

  videos:Video[] = [];

  responsiveOptions: any[] | undefined;


  constructor(private route: ActivatedRoute, private videoService: VideoDBService) {
    this.video = {
      videoId: 1,
      videoName: 'Sample Video',
      typeId: 2,
      seriesId: 3,
      mainGenreId: 4,
      seasonId: 1,
      episode: 1,
      runningTime: '01:30:00',
      isShowing: true,
      releaseDate: new Date('2024-01-01'),
      rating: 4.5,
      popularity: 100,
      thumbnailId: 5,
      lang: 'English',
      summary: 'This is a sample video summary.',
      views: 1000,
      ageRating: 'PG',
      trailerUrl: 'https://example.com/trailer',
      mainGenreName:' ',
      seasonName: ' '
  };
  }

  ngOnInit() {
    var videoID: string | null
      this.route.paramMap.subscribe(params => {
        videoID = params.get('id');

      if (videoID) {
        this.videoService.getVideoApiWithID(videoID).subscribe(data => {
          this.video = data;
          console.log(data);
          if(this.video.seasonId){
            this.videoService.getSeasonWithID(data.seasonId.toString()).subscribe((data)=>{this.season = data})
          }
        });
      }
    });


    this.videoService.getVideoApi().subscribe((datas)=>{this.videos=datas})

    this.responsiveOptions = [
      {
          breakpoint: '1199px',
          numVisible: 1,
          numScroll: 1
      },
      {
          breakpoint: '991px',
          numVisible: 2,
          numScroll: 1
      },
      {
          breakpoint: '767px',
          numVisible: 1,
          numScroll: 1
      }]
  }

}
