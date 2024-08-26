import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VideoDBServiceService } from '../video-dbservice.service';
import { Video } from '../interfaces/video';

@Component({
  selector: 'app-video-detail',
  templateUrl: './video-detail.component.html',
  styleUrls: ['./video-detail.component.css']
})
export class VideoDetailComponent implements OnInit{

  video:Video;

  videos:Video[] = [];

  responsiveOptions: any[] | undefined;


  constructor(private route: ActivatedRoute, private videoService: VideoDBServiceService) {
    this.video = {
      videoID: 1,
      videoName: 'Sample Video',
      typeID: 2,
      seriesID: 3,
      mainGenreID: 4,
      seasonID: 1,
      episode: 1,
      runningTime: '01:30:00',
      isShowing: true,
      releaseDate: new Date('2024-01-01'),
      rating: 4.5,
      popularity: 100,
      thumbnailID: 5,
      lang: 'English',
      summary: 'This is a sample video summary.',
      views: 1000,
      ageRating: 'PG',
      trailerUrl: 'https://example.com/trailer'
  };
  }

  ngOnInit() {
    const vidoeID :string| null = this.route.snapshot.paramMap.get('id');
    this.videoService.getVideoApiWithID(vidoeID!).subscribe((data)=>{
      this.video = data
    })

    this.videoService.getVideoApi().subscribe((datas)=>{this.videos=datas})

    console.log(vidoeID);

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
