import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-testttttttt',
  templateUrl: './testttttttt.component.html',
  styleUrls: ['./testttttttt.component.css']
})
export class TesttttttttComponent implements OnInit {

  rows: number = 5;
  cols: number = 8;
  selectedSeats: { row: number, col: number }[] = []; // 用於儲存已選擇的座位
  seatMap: any[][] = [];
  maxSelectableSeats: number = 3; // 用戶最多可以選擇 3 個座位

  ngOnInit(): void {
    this.initializeSeats();
  }

  // 初始化座位
  initializeSeats(): void {
    for (let i = 0; i < this.rows; i++) {
      let row: any[] = [];
      for (let j = 0; j < this.cols; j++) {
        row.push({ selected: false });
      }
      this.seatMap.push(row);
    }
  }

  // 選擇座位
  selectSeat(row: number, col: number): void {
    const seat = this.seatMap[row][col];
    if (seat.selected) {
      // 取消選擇座位
      seat.selected = false;
      this.selectedSeats = this.selectedSeats.filter(
        (s) => !(s.row === row && s.col === col)
      );
    } else {
      // 如果已選中的座位少於最大數量
      if (this.selectedSeats.length < this.maxSelectableSeats) {
        seat.selected = true;
        this.selectedSeats.push({ row, col });
      } else {
        alert('已達到可選擇的最大座位數量');
      }
    }
  }
}
