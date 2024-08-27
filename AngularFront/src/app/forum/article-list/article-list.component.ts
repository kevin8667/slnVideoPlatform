import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css'],
})
export class ArticleListComponent {
  constructor(private route: Router) {}
  navto() {

    this.route.navigateByUrl('forum/article/12512')
  }
}
