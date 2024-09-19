import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { scGuardGuard } from './shopping-cart/guards/sc-guard.guard';

const routes: Routes = [
  { path: '', loadChildren: () => import('./video-db/video-db.module').then(m => m.VideoDbModule)  },
  { path: 'login', loadChildren: () => import('./member/member.module').then(m => m.MemberModule)},
  { path: 'auth/callback', component: AuthCallbackComponent },
  {
    path: 'playlist',
    loadChildren: () =>
      import('./playlist/playlist.module').then((m) => m.PlaylistModule),
  },
  {
    path: 'ticket',
    loadChildren: () =>
      import('./Ticket/ticket.module').then((m) => m.TicketModule),
  },
  {
    path: 'forum',
    loadChildren: () =>
      import('./forum/forum.module').then((m) => m.ForumModule),
  },
  { path: 'video-db', loadChildren: () => import('./video-db/video-db.module').then(m => m.VideoDbModule) },
  {
    path: 'shoppingCart',
    component: ShoppingCartComponent,
    loadChildren: () =>
      import('./shopping-cart/shopping-cart.module').then(
        (m) => m.ShoppingCartModule
      ),
  },
  { path: 'error', component: ErrorPageComponent },
  { path: '**', redirectTo: '/error' },
  ];

  @NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
  })
export class AppRoutingModule { }



