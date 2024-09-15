import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { TestComponent } from './test/test.component';
import { AppRoutingModule } from './app-routing.module';
import { Router } from '@angular/router';
import { RouterModule, Routes } from '@angular/router';
import { TicketReservationComponent } from './Ticket/ticket-reservation/ticket-reservation.component';



@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    TicketReservationComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
