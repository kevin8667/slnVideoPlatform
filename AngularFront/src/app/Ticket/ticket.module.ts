// import { TicketOrderComponent } from './ticket-order/ticket-order.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DataService } from 'src/app/data.service';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { TicketSelectionComponent } from './ticket-selection/ticket-selection.component';
import { TicketComponent } from './ticket/ticket.component';
import { TicketReservationComponent } from './ticket-reservation/ticket-reservation.component';


const routes: Routes = [
  { path: '', component: TicketComponent },
  { path: 'ticketselection', component: TicketSelectionComponent },
  { path: 'ticketreservation', component: TicketReservationComponent },

];

@NgModule({
  declarations: [
    TicketComponent,
    TicketSelectionComponent,
    // TicketOrderComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forChild(routes),
  ], // 使用 RouterModule 並設定子路由
  providers: [DataService],
  exports: [RouterModule], // 匯出 RouterModule，讓其他模組可以使用這些路由
})
export class TicketModule {}
