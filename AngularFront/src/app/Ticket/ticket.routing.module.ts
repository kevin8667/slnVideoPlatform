import { NgModule } from '@angular/core';
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
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TicketRoutingModule {}
