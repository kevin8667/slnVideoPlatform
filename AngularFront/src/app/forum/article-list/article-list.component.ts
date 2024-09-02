import { ArticleView } from './../../interface/ArticleView';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ForumPagingDTO } from 'src/app/interface/ForumPagingDTO';
import { Theme } from 'src/app/interface/Theme';
import { ForumService } from 'src/app/service/forum.service';

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
    totalRecords: 0,
  };
  images: any[] = [
    {
      pic: '/assets/images/forumAd.png',
    },
    {
      pic: '/assets/images/forumAd2.png',
    },
  ];

  responsiveOptions: any[] = [
    {
      breakpoint: '1024px',
      numVisible: 5,
    },
    {
      breakpoint: '768px',
      numVisible: 3,
    },
    {
      breakpoint: '560px',
      numVisible: 1,
    },
  ];
  constructor(private route: Router, private forumService: ForumService) {}

  ngOnInit(): void {
    this.load();

    this.forumService.themeTag$.subscribe((data) => {
      this.themeTag = data;
    });
  }

  private load() {
    this.forumService
      .getArticleView(this.forumDto)
      .subscribe((data: { forumResult: ArticleView[]; totalCount: number }) => {
        this.articles = data.forumResult;
        this.pages.totalRecords = data.totalCount;
      });
  }

  loadTheme(id: number) {
    this.forumDto.categoryId = id;
    this.pages.first = 0; // 重設為起始位置
    this.forumDto.page = 1; // 設置為第1頁
    this.load();
  }

  changePage(event: any) {
    this.pages.first = event.first ?? 1;
    this.forumDto.page = event.page + 1;

    this.load();
  }

  onSliderChange() {
    // 設置頁面大小並重設起始位置
    this.forumDto.page = 1; // 設置為第1頁

    // 重新加載文章
    this.load();
  }

  search() {
    this.load();
    this.forumDto.keyword = '';
  }
  openCreateArticleDialog() {
    this.route.navigateByUrl('forum/newA');
  }

}
