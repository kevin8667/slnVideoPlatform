import { ArticleView } from './../../interface/ArticleView';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ForumPagingDTO } from 'src/app/interface/ForumPagingDTO';
import { Theme } from 'src/app/interface/Theme';
import { ForumServiceService } from 'src/app/service/forum-service.service';
@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css'],
})
export class ArticleListComponent implements OnInit {
  articles: ArticleView[] = [];

  forumPagingDTO: ForumPagingDTO | undefined;

  themeTag: Theme[] = [];

  forumDto = {
    categoryId: 0,
    keyword: '',
    page: 1,
    pageSize: 10,
    sortType: 'asc',
    sortBy: '',
  };
  pages = {
    first: 0,
    rows: 10,
    totalRecords: 0,
  };
  constructor(private route: Router, private service: ForumServiceService) {}

  ngOnInit(): void {
    this.load();

    this.service.getTheme().subscribe((data) => {
      return (this.themeTag = data);
    });
  }

  private load() {
    this.service.getArticleView(this.forumDto).subscribe((data) => {
      this.articles = data.forumResult;
      this.pages.totalRecords = data.totalCount;
    });
  }

  loadTheme(id: number) {
    this.forumDto.categoryId = id;
    this.load();
  }

  changePage(event: any) {
    this.pages.first = event.first ?? 1;
    this.pages.rows = event.rows ?? 10;
    this.forumDto.pageSize = this.pages.rows;
    this.forumDto.page = Math.floor(this.pages.first / this.pages.rows) + 1;

    this.load();
  }

  onSliderChange() {
    // 設置頁面大小並重設起始位置
    this.forumDto.pageSize = this.pages.rows;
    this.pages.first = 1; // 重設為起始位置
    this.forumDto.page = 1; // 設置為第1頁

    // 重新加載文章
    this.load();
  }

  search(event: any) {
    console.log(event);
  }
  openCreateArticleDialog() {
    this.route.navigateByUrl('forum/newA');
  }
}
