import { TicketModule } from './Ticket/ticket.module';
import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { TicketComponent } from './Ticket/ticket/ticket.component';
import { TestComponent } from './test/test.component';

const routes: Routes = [
  { path: '', component: TestComponent },

  {
    path: 'ticket',
    loadChildren: () =>
      import('./Ticket/ticket.module').then((m) => m.TicketModule),
  }, // 當訪問 /article 時，載入 ArticleComponent

  // { path: 'post/:id', component:  }  // 當訪問 /article/post/:id 時，載入 PostComponent
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(routes)],
})
export class AppRoutingModule {}
