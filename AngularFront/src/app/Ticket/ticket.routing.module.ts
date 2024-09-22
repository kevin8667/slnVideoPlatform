import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TicketSelectionComponent } from './ticket-selection/ticket-selection.component';
import { TicketComponent } from './ticket/ticket.component';
import { TicketReservationComponent } from './ticket-reservation/ticket-reservation.component';
import { OrderComponent } from './order/order.component';

const routes: Routes = [
  { path: '', component: TicketComponent },
  { path: 'order', component: OrderComponent },
  { path: 'ticketselection', component: TicketSelectionComponent },
  { path: 'ticketreservation', component: TicketReservationComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TicketRoutingModule {}
