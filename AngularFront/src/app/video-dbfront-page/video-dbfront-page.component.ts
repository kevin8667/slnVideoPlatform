import { Component, OnInit } from '@angular/core';
import { Video } from '../interfaces/video';
import { VideoDBService } from '../video-db.service';


@Component({
  selector: 'app-video-dbfront-page',
  templateUrl: './video-dbfront-page.component.html',
  styleUrls: ['./video-dbfront-page.component.css']
})
export class VideoDBFrontPageComponent implements OnInit{
  images:any[]=[1,2,3];


  videos:Video[] =[];

  name!: string;

  typeID:any;

  videoTypes:any[] = [

    {name: '電影', typeID:'1'},
    {name: '影集', typeID:'2'},
    {name: '其他', typeID:'3'}
  ];

  responsiveOptions: any[] | undefined;

  constructor(private videoDbService:VideoDBService){

  }

  onSelect(event:any){
    if (event.value !== null && event.value !== undefined) {
      this.typeID = event.value.typeID;  // 更新選中的值
      this.videoDbService.getVideoApiWithTypeID(this.typeID.toString())
    .subscribe((videos)=>{this.videos = videos});
    } else {
      // 處理取消選擇的情況，可能是將 selectedValue 設置為 null
      this.typeID = null;
      this.videoDbService.getVideoApi().subscribe((videos) => {
        this.videos = videos;
    });
      console.log('Selection cleared');
    }

  }

  ngOnInit() {
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
