import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { TicketSelectionComponent } from './ticket-selection/ticket-selection.component';
import { TicketComponent } from './ticket/ticket.component';
import { TicketRoutingModule } from './ticket.routing.module';

@NgModule({
  declarations: [
    TicketComponent,
    TicketSelectionComponent,

    // TicketReservationComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    TicketRoutingModule, // 使用 TicketRoutingModule
  ],
  providers: [],
})
export class TicketModule {}
