import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';

const routes: Routes = [
  { path: '', component: AppComponent },
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


