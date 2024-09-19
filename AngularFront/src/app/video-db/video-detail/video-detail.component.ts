import { Season } from '../interfaces/season';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VideoDBService } from '../../video-db.service';
import { Video } from '../interfaces/video';
import { data } from 'jquery';
import { ConfirmationService, MessageService } from 'primeng/api';
import { RatingRateEvent } from 'primeng/rating';
import { OverlayPanel } from 'primeng/overlaypanel';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AddtoplaylistComponent } from '../addtoplaylist/addtoplaylist.component';

@Component({
  selector: 'app-video-detail',
  templateUrl: './video-detail.component.html',
  styleUrls: ['./video-detail.component.css'],
  providers: [ConfirmationService, MessageService, DialogService],
  // encapsulation:ViewEncapsulation.None
})
export class VideoDetailComponent implements OnInit {

  video: Video;

  season: Season | undefined;

  videos: Video[] = [];

  responsiveOptions: any[] | undefined;

  images: any[] = [];

  userRating!: number;

  visible: boolean = false;

  ref: DynamicDialogRef | undefined;
  displayOverlay: boolean = false;

  selectedIndex = 0; // 初始為第一張圖片
  selectedImage: string = this.images[this.selectedIndex]; // 預設選中第一個

  actors: any[] = [
    {
      name: '馬龍·白蘭度',
      imagePath: '/assets/img/Marlon.jpg',
    },
    {
      name: '艾爾·帕西諾',
      imagePath: '/assets/img/Alfredo.jpg',
    },
    {
      name: '詹姆士·肯恩',
      imagePath: '/assets/img/James.jpg',
    },
    {
      name: '勞勃·杜瓦',
      imagePath: '/assets/img/Robert.jpg',
    },
    {
      name: '黛安·基頓',
      imagePath: '/assets/img/Diane.jpg',
    },
    {
      name: '約翰·卡佐爾',
      imagePath: '/assets/img/John.jpg',
    },
  ];

  constructor(
    private route: ActivatedRoute,
    private videoService: VideoDBService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private dialogService: DialogService
  ) {
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
      thumbnailPath: '',
      lang: 'English',
      summary: 'This is a sample video summary.',
      views: 1000,
      ageRating: 'PG',
      trailerUrl: 'https://example.com/trailer',
      mainGenreName: ' ',
      seasonName: ' ',
      bgpath: '',
    };
  }

  onValueChange(newValue: any) {
    this.images = newValue;
  }

  onCarouselPageChange(event: any) {
    const visibleImagesCount = 3; // 假設 Carousel 可見圖片數量是 3

    // 透過 % 避免溢出，實現無限循環
    this.selectedIndex =
      (event.page + Math.floor(visibleImagesCount / 2)) % this.images.length;

    // 更新選中的圖片
    this.selectedImage = this.images[this.selectedIndex];
  }
  onImageClick(index: number) {
    this.selectedIndex = index;
    this.selectedImage = this.images[index];
  }

  confirm(event: RatingRateEvent, op: OverlayPanel) {
    op.hide();

    event.originalEvent.stopPropagation();

    if (!this.visible) {
      this.messageService.add({
        key: 'confirm',
        sticky: true,
        severity: 'warn',
        summary: 'Are you sure?',
        detail: 'Confirm to proceed',
      });
      this.visible = true;
    }
  }
  onConfirm() {
    this.messageService.clear('confirm');
    this.visible = false;
  }

  onReject() {
    this.messageService.clear('confirm');
    this.visible = false;
  }

  ngOnInit() {
    var videoID: string | null;
    this.route.paramMap.subscribe((params) => {
      videoID = params.get('id');

      if (videoID) {
        this.videoService.getVideoApiWithID(videoID).subscribe((data) => {
          this.video = data;
          console.log(data);
          if (this.video.seasonId) {
            this.videoService
              .getSeasonWithID(data.seasonId.toString())
              .subscribe((data) => {
                this.season = data;
              });
          }
        });

        this.videoService.getImagesByVideoID(videoID).subscribe((images) => {
          this.images = images;
          console.log(images);
        });
      }
    });

    this.selectedImage = this.images[this.selectedIndex];

    this.videoService.getVideoApi().subscribe((datas) => {
      this.videos = datas;
    });

    this.responsiveOptions = [
      {
        breakpoint: '1199px',
        numVisible: 1,
        numScroll: 1,
      },
      {
        breakpoint: '991px',
        numVisible: 2,
        numScroll: 1,
      },
      {
        breakpoint: '767px',
        numVisible: 1,
        numScroll: 1,
      },
    ];

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

  // 隱藏 overlay 的方法
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

}
