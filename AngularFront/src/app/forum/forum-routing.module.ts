import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { NewArticleComponent } from './new-article/new-article.component';
import { NewPostComponent } from './new-post/new-post.component';
import { ArticleComponent } from './article/article.component';
import { EditComponent } from './edit/edit.component';
import { unsavedChangesGuard } from './guards/unsaved-changes.guard';
import { forumGuard } from './guards/forum.guard';

const routes: Routes = [
  { path: '', component: ArticleListComponent },
  { path: 'newA', component: NewArticleComponent },
  { path: 'newP', component: NewPostComponent },
  {
    path: 'ed/:type/:id',
    component: EditComponent,
    canActivate: [forumGuard],
    canDeactivate: [unsavedChangesGuard]
  },
  { path: ':id', component: ArticleComponent, canActivate: [forumGuard] },
  { path: '**', redirectTo: '/error' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ForumRoutingModule {}
