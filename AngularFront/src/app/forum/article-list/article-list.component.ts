import { ArticleView } from './../../interface/ArticleView';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ForumPagingDTO } from 'src/app/interface/ForumPagingDTO';
import { ForumServiceService } from 'src/app/service/forum-service.service';
@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css'],
})
export class ArticleListComponent implements OnInit {
  articles: ArticleView[] = [];

  forumPagingDTO: ForumPagingDTO | undefined;

  forumDto = {
    categoryId: 0,
    keyword: '',
    page: 1,
    pageSize: 10,
    sortType: 'asc',
    sortBy: '',
  };

  constructor(private route: Router, private service: ForumServiceService) {}

  ngOnInit(): void {
    this.service.getArticleView(this.forumDto).subscribe((data) => {
      this.articles = data.forumResult;
    });
  }
  navto() {
    this.route.navigateByUrl('forum/article/12512');
  }
}
