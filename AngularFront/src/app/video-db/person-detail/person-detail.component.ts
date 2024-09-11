import { Component, OnInit } from '@angular/core';

import { VideoDBService } from '../../video-db.service';
import { ActivatedRoute } from '@angular/router';
import { Actor } from '../interfaces/actor';
import { Video } from '../interfaces/video';

@Component({
  selector: 'app-person-detail',
  templateUrl: './person-detail.component.html',
  styleUrls: ['./person-detail.component.css']
})
export class PersonDetailComponent implements OnInit {

  actor:Actor|undefined;
  videos:Video[] =[];

  responsiveOptions:any[]=[];

  constructor(private route: ActivatedRoute, private videoService: VideoDBService) {}

  ngOnInit(){
    var actorId:string|null;

    this.responsiveOptions = [
      {
        breakpoint: '1400px',
        numVisible: 5,
        numScroll: 2
      },
      {
        breakpoint: '1024px',
        numVisible: 3,
        numScroll: 1
      },
      {
        breakpoint: '768px',
        numVisible: 1,
        numScroll: 1
      }
    ];

    this.route.paramMap.subscribe(params => {
      actorId = params.get('id');

    if (actorId) {
      this.videoService.getActorWithId(actorId).subscribe(data => {
        this.actor = data;
        console.log(data);
      });

      this.videoService.getVideosWithActorId(actorId).subscribe(data => {
        this.videos = data;
        console.log(data);
      });

      // this.videoService.getVideoApi().subscribe(data=>{this.videos=data})
    }

    });
  }

}
