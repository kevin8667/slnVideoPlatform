import { Component, OnInit, ViewEncapsulation} from '@angular/core';
import { Video } from '../interfaces/video';
import { VideoDBService } from '../../video-db.service';
import ForumService from 'src/app/services/forumService/forum.service';
import { ArticleView } from 'src/app/interfaces/forumInterface/ArticleView';
import { Router } from '@angular/router';



@Component({
  selector: 'app-video-dbfront-page',
  templateUrl: './video-dbfront-page.component.html',
  styleUrls: ['./video-dbfront-page.component.css'],
  // encapsulation:ViewEncapsulation.None
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

  selectedVideoType:any;

  popularArticle:ArticleView[]=[]

  videoTypes:any[] = [
    {name: '電影', typeID:'1'},
    {name: '影集', typeID:'2'}
  ];

  responsiveOptions: any[] | undefined;

  selectedOption: number = 1; // 預設選擇 "今日"



  constructor(private videoDbService:VideoDBService, private forumService:ForumService, private router:Router){

  }

  onOptionSelect(option: number) {
    if (this.selectedVideoType !== option) {
      this.selectedVideoType = option;
      console.log('選中的選項:', this.selectedVideoType);
      this.videoDbService.getVideoApiWithTypeID(this.selectedVideoType.toString())
      .subscribe((videos) => {
        this.videos = videos;
      });
    }
  }

  ngOnInit() {
    this.defaultTypeID = 1;  // 設置預設的 typeID 為電影
    this.typeID = this.defaultTypeID;
    this.selectedVideoType = this.defaultTypeID;

    console.log(this.selectedVideoType);

    // 預設加載「電影」類型的影片
    this.videoDbService.getVideoApiWithTypeID(this.typeID.toString())
      .subscribe((videos) => {
        this.videos = videos;
      });
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

      this.forumService.getRecommendations().subscribe(data=>{
        this.popularArticle= data;
      })
  }

  truncateText(articleContent: string, maxLength: number) {
    if (!articleContent) {
      return '並無內容，請盡速修改'; // 或者其他預設值，例如 '無內容'
    }
    const parser = new DOMParser();
    const doc = parser.parseFromString(articleContent, 'text/html');
    const text = doc.body.innerText;

    const truncatedText =
      text.length <= maxLength ? text : text.substring(0, maxLength) + '...';
    return truncatedText;
  }
  getFirstImageSrc(htmlContent: string): string | null {
    const imgTagStart = htmlContent.indexOf('<img');
    if (imgTagStart === -1) {
      return null; // 沒有找到 img 元素
    }

    const srcStart = htmlContent.indexOf('src="', imgTagStart);
    if (srcStart === -1) {
      return null; // 沒有找到 src 屬性
    }

    const srcEnd = htmlContent.indexOf('"', srcStart + 5); // 5 是 `src="` 的長度
    const imgSrc = htmlContent.substring(srcStart + 5, srcEnd);
    return imgSrc;
  }
  navToArticle(id: any) {
    this.router.navigate(['forum', id]);
  }
}
