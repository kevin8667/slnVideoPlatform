import { Component, OnInit, ViewEncapsulation} from '@angular/core';
import { Video } from '../interfaces/video';
import { VideoDBService } from '../video-db.service';


@Component({
  selector: 'app-video-dbfront-page',
  templateUrl: './video-dbfront-page.component.html',
  styleUrls: ['./video-dbfront-page.component.css'],
  //encapsulation:ViewEncapsulation.None
})
export class VideoDBFrontPageComponent implements OnInit{
  images:any[]=
  [
    {
      name:"刺激1995",
      imagePath:"/assets/img/1995.jpg"
    },
    {
      name:"教父",
      imagePath:"/assets/img/godfather.jpg"
    },
    {
      name:"黑暗騎士",
      imagePath:"/assets/img/Dark Knight.jpg"
    }
  ]

  defaultTypeID:number = 1;

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

  onSelect(event: any) {

    if (this.typeID === event.value.typeID) {
      return;
    }
    const selectedIndex = this.videoTypes.findIndex(type => type.name === event.value.name);
    const slider = document.querySelector('.slider-background') as HTMLElement;
    slider.style.left = `${selectedIndex * 33}%`; 

    if (event.value !== null && event.value !== undefined) {
      this.typeID = event.value.typeID;  // 更新選中的值
    } else {
      this.typeID = this.defaultTypeID;  // 如果沒有選擇，使用預設值
    }
  
    this.videoDbService.getVideoApiWithTypeID(this.typeID.toString())
      .subscribe((videos) => {
        this.videos = videos;
      });
  }

  ngOnInit() {
    this.defaultTypeID = 1;  // 設置預設的 typeID 為電影
    this.typeID = this.defaultTypeID;
      
    // 預設加載「電影」類型的影片
    this.videoDbService.getVideoApiWithTypeID(this.typeID.toString())
      .subscribe((videos) => {
        this.videos = videos;
      });
      this.responsiveOptions = [
        {
          breakpoint: '1024px',
          numVisible: 5,
          numScroll: 2
        },
        {
          breakpoint: '768px',
          numVisible: 3,
          numScroll: 1
        },
        {
          breakpoint: '560px',
          numVisible: 1,
          numScroll: 1
        }
      ];

  }
}
