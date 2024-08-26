import { Component, OnInit } from '@angular/core';
import { Video } from '../interfaces/video';
import { VideoDBServiceService } from '../video-dbservice.service';


@Component({
  selector: 'app-video-dbfront-page',
  templateUrl: './video-dbfront-page.component.html',
  styleUrls: ['./video-dbfront-page.component.css']
})
export class VideoDBFrontPageComponent implements OnInit{

  videos:Video[] =[];

  responsiveOptions: any[] | undefined;

  constructor(private videoDbService:VideoDBServiceService){

  }

  ngOnInit() {
    console.log(this.videos.length)
    this.videoDbService.getVideoApi().subscribe((videos) => {
        this.videos = videos;
    });
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
      }
  ];

  }
}
