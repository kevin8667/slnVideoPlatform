// import { TicketSelectionComponent } from './../../ticket-selection/ticket-selection.component';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'app-ticket-selection',
  templateUrl: './ticket-selection.component.html',
  styleUrls: ['./ticket-selection.component.css'],
})
export class TicketSelectionComponent implements OnInit {
  ticketOptions = [0, 1, 2, 3, 4, 5]; // 可以選擇的票數
  selectedTicket = {
    fullVote: 0,
    studentVote: 0,
    oldpeopleTicket: 0,
  };
  totalPrice: number = 0;

  // 新增的變量來保存電影資訊
  movieName: string = '';
  cinemaName: string = '';
  hallName: string = '';
  showtime: string = '';
  posterUrl: string = '';
  releaseDate: string = '';

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    // 使用 ActivatedRoute 來接收 URL 參數
    this.route.queryParams.subscribe((params) => {
      this.movieName = params['movieName'] || '';
      this.cinemaName = params['cinemaName'] || '';
      this.hallName = params['hallName'] || '';
      this.showtime = params['showtime'] || '';
      this.posterUrl = params['posterUrl'] || '';
      this.releaseDate = params['releaseDate'] || '';
    });

    this.calculateTotal(); // 初始化時計算總價
  }

  calculateTotal() {
    this.totalPrice =
      this.selectedTicket.fullVote * 200 +
      this.selectedTicket.studentVote * 180 +
      this.selectedTicket.oldpeopleTicket * 100;
  }
}
