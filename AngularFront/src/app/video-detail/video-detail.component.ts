import { Season } from './../interfaces/season';
import { Component, OnInit, ViewEncapsulation} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VideoDBService } from '../video-db.service';
import { Video } from '../interfaces/video';
import { data } from 'jquery';

@Component({
  selector: 'app-video-detail',
  templateUrl: './video-detail.component.html',
  styleUrls: ['./video-detail.component.css'],
})

export class VideoDetailComponent implements OnInit{

  video:Video;

  season: Season | undefined;

  videos:Video[] = [];

  responsiveOptions: any[] | undefined;

  images:any[] | undefined;


  actors : any[]=[
    {
      name:"馬龍·白蘭度",
      imagePath:"/assets/img/Marlon.jpg"
    },
    {
      name:"艾爾·帕西諾",
      imagePath:"/assets/img/Alfredo.jpg"
    },
    {
      name:"詹姆士·肯恩",
      imagePath:"/assets/img/James.jpg"
    },
    {
      name:"勞勃·杜瓦",
      imagePath:"/assets/img/Robert.jpg"
    },
    {
      name:"黛安·基頓",
      imagePath:"/assets/img/Diane.jpg"
    },
    {
      name:"約翰·卡佐爾",
      imagePath:"/assets/img/John.jpg"
    }
  ]

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

  onValueChange(newValue: any) {
    this.images = newValue;
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

        this.videoService.getImagesByVideoID(videoID).subscribe(images=>{
          this.images=images;
          console.log(images);
        })
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
