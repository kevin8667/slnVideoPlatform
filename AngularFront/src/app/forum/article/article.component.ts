import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css'],
})
export class ArticleComponent {
  constructor(private route: Router) {}
  navToReply() {
    this.route.navigateByUrl('forum/newP');
  }
}
