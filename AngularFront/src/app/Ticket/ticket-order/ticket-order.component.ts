import { Component } from '@angular/core';

@Component({
  selector: 'app-ticket-order',
  templateUrl: './ticket-order.component.html',
  styleUrls: ['./ticket-order.component.css'],
})
export class TicketOrderComponent {
  ticketOptions = [0, 1, 2, 3, 4, 5]; // 可以選擇的票數
  selectedTicket = {
    earlyBird: 0,
    cardDiscount: 0,
    loveTicket: 0,
  };
  totalPrice: number = 0;

  ngOnInit(): void {
    this.calculateTotal(); // 初始化時計算總價
  }

  calculateTotal() {
    this.totalPrice =
      this.selectedTicket.earlyBird * 200 +
      this.selectedTicket.cardDiscount * 180 +
      this.selectedTicket.loveTicket * 100;
  }
}
 