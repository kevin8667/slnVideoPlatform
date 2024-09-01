import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { ArticleView } from 'src/app/interface/ArticleView';
import { ForumService } from 'src/app/service/forum.service';
import { MessageService } from 'primeng/api';
@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css'],
  providers: [MessageService],
})
export class ArticleComponent implements OnInit {
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
  article: ArticleView = {} as ArticleView;
  safeContent: SafeHtml = ''; // 使用 SafeHtml 類型
  constructor(
    private route: Router,
    private forumService: ForumService,
    private sanitizer: DomSanitizer,
    private messageService: MessageService
  ) {}
  delete() {}
  ngOnInit(): void {
    this.forumService.getArticle(5).subscribe((data) => {
      this.article = data;
      this.safeContent = this.sanitizer.bypassSecurityTrustHtml(
        this.article.articleContent
      );
    });
  }
  navToReply() {
    this.route.navigateByUrl('forum/newP');
  }
}
