import { Component, OnInit, ViewEncapsulation } from '@angular/core'; // 引入 ViewEncapsulation
import { ActivatedRoute, Router } from '@angular/router'; // Angular
import { MessageService, ConfirmationService, MenuItem } from 'primeng/api'; // 第三方庫
import { ArticleView } from 'src/app/interfaces/forumInterface/ArticleView'; // 自定義模組
import { Post } from '../../interfaces/forumInterface/Post'; // 自定義模組
import ForumService from 'src/app/service/forum.service'; // 自定義模組

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css'],
})
export class ArticleComponent implements OnInit {
  article: ArticleView = {} as ArticleView;
  articleId!: number;
  posts: Post[] = [];
  menuItems: MenuItem[] = [];

  constructor(
    private router: Router,
    private forumService: ForumService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private actRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.articleId = Number(this.actRoute.snapshot.paramMap.get('id'));
    if (isNaN(this.articleId)) {
      this.router.navigateByUrl('forum');
      return;
    }

    this.forumService.getArticle(this.articleId).subscribe((data) => {
      if (!data.lock) {
        this.router.navigateByUrl('forum');
        return;
      }

      this.article = data;
    });

    this.forumService.getPosts(this.articleId).subscribe((data) => {
      this.posts = data;
    });

    this.initializeMenu();
  }

  private initializeMenu() {
    this.menuItems = [
      {
        label: '編輯',
        icon: 'pi pi-pencil',
        command: () => this.edit(this.articleId, 'article')
      },
      {
        label: '刪除',
        icon: 'pi pi-trash',
        command: () => this.deleteArticle(this.articleId)
      }
    ];
  }

  getPostMenuItems(postId: number): MenuItem[] {
    return [
      {
        label: '編輯',
        icon: 'pi pi-pencil',
        command: () => this.edit(postId, 'post')
      },
      {
        label: '刪除',
        icon: 'pi pi-trash',
        command: () => this.deletePost(postId)
      }
    ];
  }

  deletePost(postId: number) {
    this.confirmationService.confirm({
      message: '確定要刪除這篇回文嗎？',
      accept: () => {
        this.forumService.deletePost(postId).subscribe({
          next: () => this.showMessage('success', '成功', '回文已刪除'),
          error: (err) => this.handleError('刪除回文失敗', err),
          complete: () => location.reload()
        });
      }
    });
  }

  deleteArticle(articleId: number) {
    this.confirmationService.confirm({
      message: '確定要刪除這篇文章嗎？',
      accept: () => {
        this.forumService.deleteArticle(articleId).subscribe({
          next: () => {
            this.showMessage('success', '成功', '文章已刪除');
            this.router.navigate(['/forum']);
          },
          error: (err) => this.handleError('刪除文章失敗', err)
        });
      }
    });
  }

  safeHtml(data: string) {
    return this.forumService.getSafe(data);
  }

  edit(id: number, type: string) {
    this.router.navigate(['/forum', 'ed', type, id]);
  }

  navToReply(articleId: number) {
    this.router.navigate(['/forum', 'new', 'post', articleId]);
  }

  dislike() {
    this.messageService.add({
      severity: 'warn',
      summary: '注意',
      detail: '已點踩!',
    });
  }

  like() {
    this.showMessage('success', '成功', '按讚囉!');
  }

  private showMessage(severity: string, summary: string, detail: string) {
    this.messageService.add({ severity, summary, detail });
  }

  private handleError(summary: string, err: any) {
    console.error(summary, err);
    this.showMessage('error', '錯誤', `${summary}：${err}`);
  }

  NumToString(count: number) {
    return String(count);
  }
}
