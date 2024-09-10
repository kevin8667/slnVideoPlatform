import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleComponent } from './article/article.component';
import { EditComponent } from './edit/edit.component';
import { unsavedChangesGuard } from './guards/unsaved-changes.guard';
import { forumGuard } from './guards/forum.guard';

const routes: Routes = [
  { path: '', component: ArticleListComponent },
  { path: ':id', component: ArticleComponent, canActivate: [forumGuard] },
  {
    path: 'new/:type',
    component: EditComponent,
    canActivate: [forumGuard],
    canDeactivate: [unsavedChangesGuard],
  },
  {
    path: 'new/:type/:articleId',
    component: EditComponent,
    canActivate: [forumGuard],
    canDeactivate: [unsavedChangesGuard],
  },
  {
    path: 'ed/:type',
    component: EditComponent,
    canActivate: [forumGuard],
    canDeactivate: [unsavedChangesGuard],
  },
  {
    path: 'ed/:type/:id',
    component: EditComponent,
    canActivate: [forumGuard],
    canDeactivate: [unsavedChangesGuard],
  },
  { path: '**', redirectTo: '/error' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ForumRoutingModule {}
