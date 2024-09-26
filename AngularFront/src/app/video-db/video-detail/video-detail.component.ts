import { CartPageService } from './../../shopping-cart/cart-page/cart-page.service';
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
import { KeywordDTO } from '../interfaces/keyword';


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

  keywords:KeywordDTO[] = []

  newShoppingCart = {
    memberId : 0,
    planId: 1,
    videoId: 1
  };

  actors : any[]=[]
  directors :any[]=[]

  mainActorName:string='';
  mainDirectorName:string='';

  displayKeywordDialog: boolean = false;
  newKeyword: string = '';
  filteredKeywords: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private videoService: VideoDBService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private dialogService: DialogService,
    private sanitizer: DomSanitizer,
    private cd: ChangeDetectorRef,
    private router: Router,
    private cartSerice:CartPageService)
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

        this.videoService.getActorsByVideo(videoID).subscribe(data=>{
          if(data){
            this.actors= data;
            this.mainActorName = this.actors[0].actorName;
          }
        })

        this.videoService.getDirectorsByVideoId(videoID).subscribe(data=>{
          if(data){
            this.directors= data;
            console.log(data)
            this.mainDirectorName=this.directors[0].directorName;
            console.log(this.mainDirectorName)
          }
        })

        this.videoService.getKeywordByVideo(videoID).subscribe(data=>{
          if(data){
            console.log(data);
            this.keywords = data;
          }
        })

        this.videoService.getImagesByVideoID(videoID).subscribe(images=>{
          if(images){
            this.images=images;
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


  addCart(vId:number): void{
    this.newShoppingCart.memberId=this.userID
    this.newShoppingCart.videoId=vId

    this.cartSerice.createShoppingCart(this.newShoppingCart)
    .subscribe(
      response => {
        console.log('Shopping cart added:', response);
        // 可以在這裡加入跳轉或成功訊息提示
      },
      error => {
        console.error('Error adding shopping cart:', error);
      }
    );
  }

  onValueChange(newValue: any) {
    this.images = newValue;
  }

  openLoginWarning(event:any, lop:OverlayPanel){
    if(this.userID ===0){
      console.log(this.userID)

      lop.toggle(event);
    }else{
      this.addCart(this.videoIDForFunctions);
      this.onAddCart();
    }

  }

  onAddCart(){
    this.messageService.add({
      key: 'global',
      severity: 'success',
      summary: '已成功加入購物車!',
      detail: `${this.video.videoName}`
    });
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

  showKeywordDialog() {
    this.displayKeywordDialog = true;
    this.loadKeywordsForVideo();
  }

  searchKeywords(event: any) {
    const query = event.query;
    this.videoService.searchKeywords(query).subscribe(data => {
      this.filteredKeywords = data
    });
  }

  loadKeywordsForVideo() {
    this.videoService.getKeywordByVideo(this.videoIDForFunctions.toString()).subscribe(data=>{
      this.keywords = data as any;
    })
  }

  addKeyword() {

    if (this.newKeyword && typeof this.newKeyword === 'string') { // 確認 newKeyword 是字串
      console.log('Adding keyword:', this.newKeyword); // 確認是否為純字串

      this.videoService.addKeyword(this.videoIDForFunctions.toString(), this.newKeyword).subscribe(
        () => {
          this.loadKeywordsForVideo();
          //this.newKeyword = '';
          this.messageService.add({ severity: 'success', summary: '成功', detail: '關鍵字已新增' });
        },
        (error) => {
          if (error.status === 409) {
            this.messageService.add({ severity: 'warn', summary: '失敗', detail: '關鍵字已存在' });
          } else {
            this.messageService.add({ severity: 'error', summary: '錯誤', detail: '關鍵字新增失敗' });
          }
        }
      );
    } else {
      console.error('newKeyword is not a string or is empty.');
    }
  }

  removeKeyword(keywordId: number) {
    this.videoService.deleteKeywordFromVideo(this.videoIDForFunctions.toString(),keywordId).subscribe(() => {
      this.loadKeywordsForVideo();
      this.messageService.add({ severity: 'success', summary: '成功', detail: '關鍵字已移除' });
    });
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
