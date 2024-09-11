import { Post } from '../../interfaces/forumnterface/Post';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleView } from 'src/app/interfaces/forumnterface/ArticleView';
import ForumService from 'src/app/service/forum.service';
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css'],
})
export class ArticleComponent implements OnInit {
  deletePost(postId: number) {
    if (!confirm('確定要刪除嗎?')) return;
    this.forumService.deletePost(postId).subscribe({
      next: (res) => {
        alert('已刪除成功');
      },
      error: (err) => console.error('刪除回文發生例外:', err),
      complete: () => location.reload(),
    });
  }
  deleteArticle(articleId: number) {
    this.confirmationService.confirm({
      message: '確定要刪除這篇文章嗎？',
      accept: () => {
        this.forumService.deleteArticle(articleId).subscribe({
          next: () => {
            this.messageService.add({severity:'success', summary: '成功', detail: '文章已刪除'});
            this.router.navigate(['/forum']);
          },
          error: (err) => {
            console.error('刪除文章失敗:', err);
            this.messageService.add({severity:'error', summary: '錯誤', detail: '刪除文章失敗：' + err});
          }
        });
      }
    });
  }
  NumToString(count: number) {
    return String(count);
  }

  // getSafe = (data: string) => this.forumService.getSafe(data);
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
    this.messageService.add({
      severity: 'success',
      summary: '成功',
      detail: '按讚囉!',
    });
  }
}
