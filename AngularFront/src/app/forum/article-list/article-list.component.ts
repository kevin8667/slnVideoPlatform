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
    first3: 1,
    rows3: 10,
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
    this.pages.first3 = event.first ?? 0; // 使用 ?? 來處理 undefined
    this.pages.rows3 = event.rows ?? 10; // 使用 ?? 來處理 undefined
    this.forumDto.pageSize = this.pages.rows3;
    this.forumDto.page = Math.floor(this.pages.first3 / this.pages.rows3) + 1;

    this.load();
  }

  onSliderChange() {
    // 設置頁面大小並重設起始位置
    this.forumDto.pageSize = this.pages.rows3;
    this.pages.first3 = 1; // 重設為起始位置
    this.forumDto.page = 1; // 設置為第1頁

    // 重新加載文章
    this.load();
  }

  search(event: any) {
    console.log(event);
  }
  navto() {
    // this.route.navigateByUrl('forum/article/12512');
  }
}
