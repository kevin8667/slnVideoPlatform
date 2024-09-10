import { ArticleView } from '../../interfaces/forumnterface/ArticleView';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { ForumPagingDTO } from 'src/app/interfaces/forumnterface/ForumPagingDTO';
import { Theme } from 'src/app/interfaces/forumnterface/Theme';
import ForumService from 'src/app/service/forum.service';

@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css'],
})
export class ArticleListComponent implements OnInit {
  // getSafe = (data: string) => this.forumService.getSafe(data);
  articles: ArticleView[] = [];
  themeTag: Theme[] = [];
  debounceTimer!: number;
  forumPagingDTO: ForumPagingDTO | undefined;

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
    // {
    //   pic: '/assets/images/forumAd2.png',
    // },
    {
      pic: '/assets/images/forumAd3.png',
    },
    {
      pic: '/assets/images/forumAd4.png',
    },
    {
      pic: '/assets/images/forumAd5.png',
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
  loading = false;
  ngOnInit(): void {
    this.load();

    this.forumService.themeTag$.subscribe((data) => {
      this.themeTag = data;
    });
  }

  private async load() {
    this.loading = true;
    try {
      const data = await lastValueFrom(
        this.forumService.getArticleView(this.forumDto)
      );
      this.articles = data.forumResult;
      this.pages.totalRecords = data.totalCount;
    } catch (err) {
      console.error('讀取文章發生例外:', err);
    } finally {
      this.loading = false;
    }
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
    // 清除之前的計時器，避免累積執行
    if (this.debounceTimer !== 0) {
      clearTimeout(this.debounceTimer);
    }

    // 設置頁面大小並重設起始位置
    this.forumDto.page = 1; // 設置為第1頁

    // 設置新的計時器，並保存到 debounceTimer
    this.debounceTimer = window.setTimeout(() => {
      this.load(); // 使用箭頭函數，確保 this 的上下文不變
      this.debounceTimer = 0; // 重置計時器
    }, 500); // 延遲n秒
  }

  search() {
    this.load();
    this.forumDto.keyword = '';
  }
  openCreateArticleDialog() {
    this.route.navigateByUrl('forum/new/article');
  }

  truncateText(articleContent: string, maxLength: number) {
    if (!articleContent) {
      return '並無內容，請盡速修改'; // 或者其他預設值，例如 '無內容'
    }
    const parser = new DOMParser();
    const doc = parser.parseFromString(articleContent, 'text/html');
    const text = doc.body.innerText;

    const truncatedText =
      text.length <= maxLength ? text : text.substring(0, maxLength) + '...';
    return truncatedText;
  }
}
