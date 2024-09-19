import { ArticleView } from '../../interfaces/forumInterface/ArticleView';
import {
  AfterViewChecked,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Router } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { Chatroom } from 'src/app/interfaces/forumInterface/Chatroom';
import { ForumPagingDTO } from 'src/app/interfaces/forumInterface/ForumPagingDTO';
import { memberName } from 'src/app/interfaces/forumInterface/memberIName';
import { Theme } from 'src/app/interfaces/forumInterface/Theme';
import ForumService from 'src/app/services/forumService/forum.service';
import { SignalRService } from 'src/app/services/forumService/signal-r.service';

@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css'],
  providers: [SignalRService],
})
export class ArticleListComponent implements OnInit, AfterViewChecked {
  articles: ArticleView[] = [];
  themeTag: Theme[] = [];
  debounceTimer!: number;
  forumPagingDTO: ForumPagingDTO | undefined;
  user!: memberName;
  loading = false;
  message = '';
  messages: Chatroom[] = [];
  private messageSubscription?: Subscription;
  private previousMessageCount = 0;
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
  @ViewChild('chatContainer') private chatContainer!: ElementRef;
  constructor(
    private router: Router,
    private forumService: ForumService,
    private signalRService: SignalRService
  ) {}

  ngOnInit(): void {
    this.load();
    this.forumService.user$.subscribe((data) => {
      if (data) this.user = data;
    });
    this.messageSubscription = this.signalRService.messages$.subscribe({
      next: (data: Chatroom[]) => {
        this.messages = data;
      },
      error: (err) => console.error('接收訊息發生例外:', err),
    });
    this.forumService.themeTag$.subscribe((data) => {
      this.themeTag = data;
    });
  }

  ngAfterViewChecked() {
    if (this.messages.length !== this.previousMessageCount) {
      this.scrollToBottom();
      this.previousMessageCount = this.messages.length; // 更新訊息數量
    }
  }

  private scrollToBottom(): void {
    try {
      this.chatContainer.nativeElement.scroll({
        top: this.chatContainer.nativeElement.scrollHeight,
        behavior: 'smooth', // 平滑滾動
      });
    } catch (err) {
      console.error('滾動到底部失敗', err);
    }
  }

  sendMessage(message: string) {
    if (!message.trim()) return;
    const chatroom: Chatroom = {
      senderId: this.user.memberId,
      chatMessage: message,
      nickname: this.user.nickName,
      sendtime: new Date().toISOString(),
    };
    this.signalRService.sendMessage(chatroom);
    setTimeout(() => this.scrollToBottom(), 0);
    this.message = '';
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
  }
  openCreateArticleDialog() {
    if (this.user.memberId < 1) {
      // 捕获当前的URL（用户想要访问的页面）
      this.redirect();
    } else {
      // 用户已登录，导航到创建文章页面
      this.router.navigateByUrl('forum/new/article');
    }
  }

  private redirect() {
    const currentUrl = this.router.url;

    // 导航到登录页面，并传递 returnUrl 查询参数
    this.router.navigate(['login'], { queryParams: { returnUrl: currentUrl } });
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
  getFirstImageSrc(htmlContent: string): string | null {
    const imgTagStart = htmlContent.indexOf('<img');
    if (imgTagStart === -1) {
      return null; // 沒有找到 img 元素
    }

    const srcStart = htmlContent.indexOf('src="', imgTagStart);
    if (srcStart === -1) {
      return null; // 沒有找到 src 屬性
    }

    const srcEnd = htmlContent.indexOf('"', srcStart + 5); // 5 是 `src="` 的長度
    const imgSrc = htmlContent.substring(srcStart + 5, srcEnd);
    return imgSrc;
  }
  navToArticle(id: any) {
    this.router.navigate(['forum', id]);
  }
}
