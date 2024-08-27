import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { TestComponent } from './test/test.component';
import { AppRoutingModule } from './app-routing.module';
import { ArticleListComponent } from './forum/article-list/article-list.component';
import { ArticleComponent } from './forum/article/article.component';

@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    ArticleListComponent,
    ArticleComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
