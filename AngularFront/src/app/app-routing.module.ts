import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ArticleListComponent } from './forum/article-list/article-list.component';

const routes: Routes = [
  { path: '', component: AppComponent },
  {
    path: 'forum',
    loadChildren: () =>
      import('./forum/forum.module').then(m => m.ForumModule),
  },
  { path: '**', redirectTo: '/' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
