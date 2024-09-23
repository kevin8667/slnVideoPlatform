import { Season } from '../interfaces/season';
import { Component, OnInit, ViewEncapsulation} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VideoDBService } from '../../video-db.service';
import { Video } from '../interfaces/video';
import { ConfirmationService, MessageService  } from 'primeng/api';
import { data } from 'jquery';
import { Rating, RatingRateEvent } from 'primeng/rating';
import { OverlayPanel } from 'primeng/overlaypanel';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AddtoplaylistComponent } from '../addtoplaylist/addtoplaylist.component';
import { RatingDTO } from '../interfaces/ratingDTO';
import { ChangeDetectorRef } from '@angular/core';


@Component({
  selector: 'app-video-detail',
  templateUrl: './video-detail.component.html',
  styleUrls: ['./video-detail.component.css'],
  providers: [ConfirmationService, MessageService, DialogService],
  //encapsulation:ViewEncapsulation.Emulated
})

export class VideoDetailComponent implements OnInit{

  userID = 0;

  video!:any ;

  videoIDForFunctions:number =0;

  season: Season | undefined;

  videos:Video[] = [];

  avgRating:number =0;

  responsiveOptions: any[] | undefined;

  images:any[] =[];

  userRating:number=0;

  isUserHaveRated = false;

  ratingInfo:RatingDTO|undefined;

  visible: boolean = false;

  trailerVisibility:boolean=false;

  videoUrl: SafeResourceUrl ="";

  ref: DynamicDialogRef | undefined;
  displayOverlay: boolean = false;

  isShowing:boolean = false;

  keywords:string[] = ["黑手黨","犯罪"]


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

  constructor(
    private route: ActivatedRoute,
    private videoService: VideoDBService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private dialogService: DialogService,
    private sanitizer: DomSanitizer,
    private cd: ChangeDetectorRef,
    private router: Router)
  {
  //   this.video = {
  //     videoId: 1,
  //     videoName: 'Sample Video',
  //     typeId: 2,
  //     seriesId: 3,
  //     mainGenreId: 4,
  //     seasonId: 1,
  //     episode: 1,
  //     runningTime: '01:30:00',
  //     isShowing: true,
  //     releaseDate: new Date('2024-01-01'),
  //     rating: 4.5,
  //     popularity: 100,
  //     thumbnailPath: '',
  //     lang: 'English',
  //     summary: 'This is a sample video summary.',
  //     views: 1000,
  //     ageRating: 'PG',
  //     trailerUrl: 'https://example.com/trailer',
  //     mainGenreName: ' ',
  //     seasonName: ' ',
  //     bgpath: '',
    // };
  }

  ngOnInit() {

    this.videoService.user$.subscribe((data) => (this.userID = data));

    this.avgRating =0;

    var videoID: string | null
      this.route.paramMap.subscribe(params => {
        videoID = params.get('id');

      if (videoID) {
        this.videoService.getVideoApiWithID(videoID).subscribe(data => {
          this.video = data;

          this.videoIDForFunctions = data.videoId;
          console.log(data);
          if(this.video.seasonId){
            this.videoService.getSeasonWithID(data.seasonId.toString()).subscribe((data)=>{this.season = data})
          }
          this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.video.trailerUrl);


          this.ratingInfo=
          {
            memberId : this.userID,
            videoId : this.videoIDForFunctions,
            rating : 0
          }
          console.log(this.ratingInfo);

        });

        this.videoService.getRatingsByVideo(videoID).subscribe(ratings=>{
          this.avgRating = 0;
          if(ratings.length>0)
          {
            let tempSum : number= ratings.reduce((partialSum, a) => partialSum + a.rating, 0);
            this.avgRating = tempSum/ratings.length;
            this.avgRating = Math.round(this.avgRating*10)/10
            console.log(this.avgRating);
          }
        })

        // this.videoService.getRatingByData(this.userID.toString(), videoID).subscribe(data=>{
        //   console.log(data);
        //   if(data)
        //     {
        //       this.userRating= data.rating;
        //     }
        // })

        this.videoService.getImagesByVideoID(videoID).subscribe(images=>{
          if(images){
            this.images=images;
            console.log(images);
          }

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

      this.route.queryParams.subscribe(params => {
        if (params['showOverlay'] === 'true') {
          this.displayOverlay = true;

          setTimeout(() => {
            const button = document.querySelector('.add-to-playlist-button') as HTMLElement;
            const arrowContainer = document.querySelector('.arrow-container') as HTMLElement;

            if (button && arrowContainer) {
              const buttonRect = button.getBoundingClientRect();

              // 動態設置箭頭位置，使其對準按鈕
              arrowContainer.style.position = 'absolute'; // 確保是絕對定位
              arrowContainer.style.top = (buttonRect.top - 80) + 'px'; // 讓箭頭位於按鈕上方
              arrowContainer.style.left = (buttonRect.left + (buttonRect.width / 2) - (arrowContainer.offsetWidth / 2) + 150) + 'px'; // 將箭頭水平居中對齊按鈕
            }
          }, 0); // 延遲執行，確保 DOM 完全加載
        }
      });
  }

  onValueChange(newValue: any) {
    this.images = newValue;
  }

  openLoginWarning(event:any, lop:OverlayPanel){
    if(this.userID ===0){
      console.log(this.userID)
      lop.toggle(event);
    }
    
  }


  openRatingPanel(event: any, op: OverlayPanel) {
    // 先檢查評分資料是否已經存在 (這部分邏輯應該已經存在)
    if (this.userID !== 0) {
      this.videoService.getRatingByData(this.userID.toString(), this.video.videoId).subscribe(data => {
        this.userRating = data.rating;
        this.isUserHaveRated = true;

        // 打開 OverlayPanel
        op.toggle(event);

        // 手動觸發變更檢測，確保評分顯示正確
        this.cd.detectChanges();
      },
      error => {
        // 如果返回 404，表示沒有評分
        if (error.status === 404) {
          this.userRating = 0; // 沒有找到評分，將評分設置為 0

          op.toggle(event);

        } else {
          console.error('Error:', error);
        }
      }
    );
    } else {
      // 用戶未登入，直接打開 Panel
      op.toggle(event);
    }

  }

  confirm(event: RatingRateEvent , op: OverlayPanel) {

    op.hide();

    event.originalEvent.stopPropagation();

    if (!this.visible) {
      this.messageService.add({ key: 'confirm', sticky: true, severity: 'warn', summary: '確定要留下評分?', detail: '評分：'+event.value });
      this.visible = true;
    }
  }
  onConfirm() {
    this.messageService.clear('confirm');

    this.visible = false;

    this.ratingInfo!.rating=this.userRating;

    console.log(this.videoIDForFunctions);

    this.postRating();

    this.messageService.add({
      key: 'global',
      severity: 'success',
      summary: '已成功留下評分!',
      detail: `您留下的評分為：${this.userRating}`
    });
  }

  onReject() {
    this.messageService.clear('confirm');
    this.visible = false;
  }

  overLayToggle(){
    this.trailerVisibility = !this.trailerVisibility;
    console.log(this.trailerVisibility)
  }

  onAddKeyword() {
    // 在這裡寫新增關鍵字的邏輯，比如打開一個彈出框
    console.log('Add new keyword clicked');
    // 可以調用對話框或者處理其他邏輯
  }


  postRating()
  {
    this.videoService.createNewRating(this.ratingInfo!).subscribe(ratingResult=>{
      console.log(ratingResult);
    })
  }

  getRating()
  {
    this.videoService.getRatingsByVideo(this.videoIDForFunctions!.toString()).subscribe(data=>{
        console.log(data);
      })
  }

  hideOverlay() {
    this.displayOverlay = false;
  }

  showAddToPlaylist() {
    const videoID = this.video.videoId; // 從當前的影片對象中獲取 videoId

    this.ref = this.dialogService.open(AddtoplaylistComponent, {
      header: '加入片單',
      width: '50%',
      contentStyle: { overflow: 'auto' },
      baseZIndex: 10000,
      maximizable: true,
      data: {
        videoId: videoID // 傳遞 videoId 給對話框
      }
    });

    this.ref.onClose.subscribe((selectedPlaylist) => {
      if (selectedPlaylist) {
        console.log('已選擇播放清單，將影片加入:', selectedPlaylist);
      }
    });
  }

  ngOnDestroy() {
    if (this.ref) {
      this.ref.close();
    }
  }

  onBookTicket(){
    this.router.navigate(['/ticket'], {queryParams:{videoID:this.video.videoId}});
  }
}
