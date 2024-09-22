import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/data.service';
import { ChangeDetectorRef } from '@angular/core';
import { VideoDBService } from 'src/app/video-db.service';
import { Video } from 'src/app/video-db/interfaces/video'; // 引入 Video 接口
@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css'],
  providers: [DataService, VideoDBService],
})
export class TicketComponent implements OnInit {
  movieId: number = 1; // 測試用電影ID
  selectedCinemaId: number | null = null; // 選中的影院ID
  selectedCinema: any = null; // 選中的影院
  cinemas: any[] = []; // 影院清單
  showTimeID: number = 0;
  movieName: string = ''; // 從 API 獲取的電影名稱
  posterUrl: string = ''; // 從 API 獲取的電影海報 URL
  summary: string = ''; //簡介
  runningTime: string = '';

  constructor(
    private router: Router,
    private dataService: DataService,
    private cdr: ChangeDetectorRef,
    private videoDBService: VideoDBService // 注入 VideoDBService,
  ) {}
  // 傳換片長時間為""時""分
  formatRunningTime(time: string): string {
    const parts = time.split(':'); // 將時間字串分為小時、分鐘、秒
    const hours = parseInt(parts[0], 10); // 小時部分
    const minutes = parseInt(parts[1], 10); // 分鐘部分

    // 返回格式化的時間
    return `${hours}時${minutes}分`;
  }
  ngOnInit(): void {
    // 呼叫 VideoDBService 來取得電影資訊
    this.videoDBService.getVideoApiWithID(this.movieId.toString()).subscribe(
      (video: Video) => {
        console.log(video); // 確認 API 返回的資料
        this.movieName = video.videoName;
        this.posterUrl = video.thumbnailPath; // 確保 API 回傳中有海報 URL
        this.summary = video.summary;
        // 使用格式化函數將 runningTime 轉換為 "X時X分"
        this.runningTime = this.formatRunningTime(video.runningTime);

        // 手動觸發變更檢測
        this.cdr.detectChanges();
      },
      (error) => {
        console.error('Error fetching video data:', error);
      }
    );

    if (this.movieId) {
      this.dataService.getCinemas(this.movieId).subscribe((data: any) => {
        this.cinemas = data.map((cinema: any) => ({
          CinemaId: cinema.cinemaId,
          CinemaName: cinema.cinemaName,
          Halls: [],
        }));

        const savedCinemaId = localStorage.getItem('selectedCinemaId');

        if (savedCinemaId) {
          this.selectedCinemaId = parseInt(savedCinemaId, 10);
          this.selectedCinema = this.cinemas.find(
            (cinema) => cinema.CinemaId === this.selectedCinemaId
          );
        }

        // 如果有預設選中的影院，則載入該影院的放映時間
        if (this.selectedCinema) {
          this.loadShowtimes(this.selectedCinema.CinemaId);
        }

        // 手動觸發變更檢測
        this.cdr.detectChanges();
      });
    } else {
      console.error('沒有電影ID');
    }
  }

  onCinemaChange(event: any) {
    this.selectedCinemaId = parseInt(event.target.value, 10);
    this.selectedCinema = this.cinemas.find(
      (cinema) => cinema.CinemaId === this.selectedCinemaId
    );

    // 將選中的影院ID儲存到 localStorage
    localStorage.setItem('selectedCinemaId', this.selectedCinemaId.toString());

    // 載入放映時間
    this.loadShowtimes(this.selectedCinema.CinemaId);

    // 手動觸發變更檢測
    this.cdr.detectChanges();
  }

  loadShowtimes(cinemaId: number) {
    this.dataService.getShowtimesByCinema(cinemaId).subscribe((data: any) => {
      const halls = data.reduce((acc: any[], showtime: any) => {
        let hall = acc.find((h) => h.HallName === showtime.hallsName);
        if (!hall) {
          hall = { HallName: showtime.hallsName, Showtimes: [] };
          acc.push(hall);
        }

        const showTimeDate = new Date(showtime.showTimeDatetime);
        const time = showTimeDate.toLocaleTimeString([], {
          hour: '2-digit',
          minute: '2-digit',
          hour12: false,
        });
        const date = showTimeDate.toLocaleDateString([], {
          month: 'numeric',
          day: 'numeric',
        });
        const day = showTimeDate.toLocaleDateString([], { weekday: 'long' });

        hall.Showtimes.push({
          time,
          date,
          day,
          showTimeID: showtime.showtimeId,
          showTimeDate, // 我們要保存原始日期進行排序
        });

        return acc;
      }, []);

      // 對每個影廳的 Showtimes 進行日期和時間排序
      halls.forEach((hall: { Showtimes: any[] }) => {
        hall.Showtimes.sort((a: any, b: any) => {
          // 首先比較日期，然後比較時間
          const dateComparison =
            new Date(a.showTimeDate).getTime() -
            new Date(b.showTimeDate).getTime();
          if (dateComparison !== 0) {
            return dateComparison; // 如果日期不同，按照日期排序
          }
          // 如果日期相同，則按照時間排序
          return a.time.localeCompare(b.time);
        });
      });

      // 更新選中的影院的 Halls
      this.selectedCinema.Halls = halls;

      // 手動觸發變更檢測
      this.cdr.detectChanges();
    });
  }
  // 當點擊選擇時間按鈕時，將選中的資料儲存到 Local Storage，並跳轉至下一個頁面
  onTimeSelect(
    hallName: string,
    time: string,
    date: string,
    day: string,
    showTimeID: number
  ) {
    console.log('選中的 showtimeId:', showTimeID);

    // 儲存選中的資料到 localStorage
    localStorage.setItem('cinemaName', this.selectedCinema.CinemaName);
    localStorage.setItem('hallName', hallName);
    localStorage.setItem('showtime', time); // 儲存時間
    localStorage.setItem('date', date); // 儲存日期
    localStorage.setItem('day', day); // 儲存星期

    localStorage.setItem('selectedShowtimeId', showTimeID.toString());
    localStorage.setItem('movieId', this.movieId.toString());
    localStorage.setItem('movieName', this.movieName);
    localStorage.setItem('posterUrl', this.posterUrl);
    localStorage.setItem('summary', this.summary);
    localStorage.setItem('runningTime', this.runningTime);
    this.router.navigate(['ticket/ticketselection']);
  }
}
