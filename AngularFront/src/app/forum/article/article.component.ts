import { LikeDTO } from './../../interfaces/forumInterface/LikeDTO';
import { Component, HostListener, OnInit } from '@angular/core'; // 引入 ViewEncapsulation
import { ActivatedRoute, Router } from '@angular/router'; // Angular
import { MessageService, ConfirmationService, MenuItem } from 'primeng/api'; // 第三方庫
import { ArticleView } from 'src/app/interfaces/forumInterface/ArticleView'; // 自定義模組
import { Post } from '../../interfaces/forumInterface/Post'; // 自定義模組
import ForumService from 'src/app/services/forumService/forum.service'; // 自定義模組
import { AllReactionsDTO } from 'src/app/interfaces/forumInterface/AllReactionsDTO';
@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css'],
  providers: [MessageService, ConfirmationService],
})
export class ArticleComponent implements OnInit {
  article: ArticleView = {} as ArticleView;
  articleId!: number;
  posts: Post[] = [];
  menuItems: MenuItem[] = [];
  debounceTimer!: number;
  currentUserId!: number;
  pendingReaction: any = null;
  show = false;
  constructor(
    private router: Router,
    private forumService: ForumService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private actRoute: ActivatedRoute
  ) {}
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification(event: any): void {
    // 清除計時器
    if (this.debounceTimer !== 0) {
      clearTimeout(this.debounceTimer);
    }

    // 如果有未提交的評價數據，則進行提交
    if (this.pendingReaction) {
      const contentId = this.pendingReaction.contentId;
      if (contentId === this.articleId) {
        this.forumService.ArticleCount(this.pendingReaction).subscribe({
          next: (data) => {
            console.log('離開前成功提交文章反應:', data);
            this.pendingReaction = null; // 提交成功後清空 pendingReaction
          },
          error: (err: any) => this.handleError('離開前更新文章反應失敗', err),
        });
      } else {
        this.forumService.PostCount(this.pendingReaction).subscribe({
          next: (data) => {
            console.log('離開前成功提交回覆反應:', data);
            this.pendingReaction = null; // 提交成功後清空 pendingReaction
          },
          error: (err: any) => this.handleError('離開前更新回覆反應失敗', err),
        });
      }
    }
  }

  ngOnInit(): void {
    this.forumService.loadQuill();
    this.currentUserId = this.forumService.getCurrentUser().id;
    this.articleId = Number(this.actRoute.snapshot.paramMap.get('id'));
    this.forumService
      .getUserReaction(this.currentUserId, this.articleId)
      .subscribe({
        next: (reactions) => {
          const articleReactionType = reactions.articleReaction?.reactionType;
          const postReactions = reactions.postReactions;

          // 根據文章 reactionType 初始化文章的反應
          if (articleReactionType === true) {
            this.reactionMap[0] = 'like';
          } else if (articleReactionType === false) {
            this.reactionMap[0] = 'dislike';
          } else {
            this.reactionMap[0] = null;
          }

          // 迴圈檢查每篇文章的回文反應
          postReactions.forEach((postReaction) => {
            const postReactionType = postReaction.reactionType;
            const postId = postReaction.contentId;

            if (postReactionType === true) {
              this.reactionMap[postId] = 'like';
            } else if (postReactionType === false) {
              this.reactionMap[postId] = 'dislike';
            } else {
              this.reactionMap[postId] = null;
            }
          });
        },
        error: (error) =>
          console.error('Error fetching user reactions:', error),
      });

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
        command: () => this.edit(this.articleId, 'article'),
      },
      {
        label: '刪除',
        icon: 'pi pi-trash',
        command: () => this.deleteArticle(this.articleId),
      },
    ];
  }

  getPostMenuItems(postId: number): MenuItem[] {
    return [
      {
        label: '編輯',
        icon: 'pi pi-pencil',
        command: () => this.edit(postId, 'post'),
      },
      {
        label: '刪除',
        icon: 'pi pi-trash',
        command: () => this.deletePost(postId),
      },
    ];
  }

  deletePost(postId: number) {
    this.confirmationService.confirm({
      message: '確定要刪除這篇回文嗎？',
      accept: () => {
        this.forumService.deletePost(postId).subscribe({
          next: () => this.showMessage('success', '成功', '回文已刪除'),
          error: (err) => this.handleError('刪除回文失敗', err),
          complete: () => location.reload(),
        });
      },
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
          error: (err) => this.handleError('刪除文章失敗', err),
        });
      },
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
  // 使用 Map 來追踪每個內容的反應狀態
  reactionMap: { [contentId: number]: 'like' | 'dislike' | null } = {}; // 記錄用戶對每篇文章的反應

  getReaction(contentId: number) {
    return this.reactionMap[contentId] || null; // 返回當前用戶對該文章的反應
  }

  toggleReaction(reaction: 'like' | 'dislike' | null, contentId: number) {
    const currentReaction = this.getReaction(contentId);

    // 決定新的反應狀態
    let newReaction: 'like' | 'dislike' | null;
    if (currentReaction === reaction) {
      newReaction = null; // 取消反應
    } else {
      newReaction = reaction; // 更新反應
    }
    if (this.debounceTimer !== 0) {
      clearTimeout(this.debounceTimer);
    }

    // 更新本地反應狀態
    this.reactionMap[contentId] = newReaction;
    // 準備發送到後端的數據
    this.pendingReaction = this.createReactionDTO(contentId, newReaction);
    this.sendReaction(contentId, newReaction, currentReaction);
  }
  private createReactionDTO(
    contentId: number,
    newReaction: 'like' | 'dislike' | null
  ) {
    return {
      memberId: this.currentUserId,
      contentId: contentId === 0 ? this.articleId : contentId,
      reactionType:
        newReaction === 'like'
          ? true
          : newReaction === 'dislike'
          ? false
          : null,
    };
  }
  private sendReaction(
    contentId: number,
    newReaction: 'like' | 'dislike' | null,
    currentReaction: 'like' | 'dislike' | null
  ): void {
    // 如果新的反應和舊的反應相同，則直接 return，不做任何處理
    if (newReaction === currentReaction) {
      console.log('反應未改變，無需更新');
      return;
    }

    const likeDTO = {
      memberId: this.currentUserId,
      contentId: contentId === 0 ? this.articleId : contentId,
      reactionType:
        newReaction === 'like'
          ? true
          : newReaction === 'dislike'
          ? false
          : null,
    };

    this.pendingReaction = likeDTO;

    if (contentId === 0) {
      this.updateArticleReaction(newReaction, currentReaction);
      this.debounceTimer = window.setTimeout(() => {
        this.forumService.ArticleCount(likeDTO).subscribe({
          next: (data) => {
            console.log('文章反應已提交:', data);
            this.pendingReaction = null; // 提交成功後清空 pendingReaction
          },
          error: (err: any) => this.handleError('更新文章反應失敗', err),
        });
      }, 1000);
    } else {
      this.updatePostReaction(contentId, newReaction, currentReaction);
      this.debounceTimer = window.setTimeout(() => {
        this.forumService.PostCount(likeDTO).subscribe({
          next: (data) => {
            console.log('回覆反應已提交:', data);
            this.pendingReaction = null; // 提交成功後清空 pendingReaction
          },
          error: (err: any) => this.handleError('更新回覆反應失敗', err),
        });
      }, 1000);
    }
  }

  private updateArticleReaction(
    newReaction: 'like' | 'dislike' | null,
    currentReaction: 'like' | 'dislike' | null
  ) {
    if (newReaction === 'like') {
      this.article.likeCount++;
      this.showMessage('success', '成功', '已按讚');

      if (currentReaction === 'dislike') {
        this.article.dislikeCount--;
      }
    } else if (newReaction === 'dislike') {
      this.article.dislikeCount++;
      this.showMessage('error', '注意', '已點踩!!');

      if (currentReaction === 'like') {
        this.article.likeCount--;
      }
    } else {
      // 取消反應
      if (currentReaction === 'like') {
        this.article.likeCount--;
        this.showMessage('warn', '成功', '讚已復原');
      } else if (currentReaction === 'dislike') {
        this.article.dislikeCount--;
        this.showMessage('info', '成功', '踩已復原');
      }
    }
  }

  private updatePostReaction(
    postId: number,
    newReaction: 'like' | 'dislike' | null,
    currentReaction: 'like' | 'dislike' | null
  ) {
    const postIndex = this.posts.findIndex((p) => p.postId === postId);
    if (postIndex === -1) return;

    if (newReaction === 'like') {
      this.posts[postIndex].likeCount++;
      this.showMessage('success', '成功', '已按讚');

      if (currentReaction === 'dislike') {
        this.posts[postIndex].dislikeCount--;
      }
    } else if (newReaction === 'dislike') {
      this.posts[postIndex].dislikeCount++;
      this.showMessage('error', '注意', '已點踩!!');

      if (currentReaction === 'like') {
        this.posts[postIndex].likeCount--;
      }
    } else {
      // 取消反應
      if (currentReaction === 'like') {
        this.posts[postIndex].likeCount--;
        this.showMessage('warn', '成功', '讚已復原');
      } else if (currentReaction === 'dislike') {
        this.posts[postIndex].dislikeCount--;
        this.showMessage('info', '成功', '踩已復原');
      }
    }
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
