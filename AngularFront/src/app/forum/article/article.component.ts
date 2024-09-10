import { Post } from './../../interface/Post';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleView } from 'src/app/interface/ArticleView';
import ForumService from 'src/app/service/forum.service';
import { MessageService } from 'primeng/api';
@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css'],
  providers: [MessageService],
})
export class ArticleComponent implements OnInit {
  NumToString(count: number) {
    return String(count);
  }

  // getSafe = (data: string) => this.forumService.getSafe(data);
  article: ArticleView = {} as ArticleView;
  articleId!: number;
  posts: Post[] = [];

  constructor(
    private route: Router,
    private forumService: ForumService,
    private messageService: MessageService,
    private actRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.articleId = Number(this.actRoute.snapshot.paramMap.get('id'));
    if (isNaN(this.articleId)) {
      this.route.navigateByUrl('forum');
      return;
    }

    this.forumService.getArticle(this.articleId).subscribe((data) => {
      if (!data.lock) {
        this.route.navigateByUrl('forum');
        return;
      }

      this.article = data;
    });

    this.forumService.getPosts(this.articleId).subscribe((data) => {
      this.posts = data;
    });
  }

  safeHtml(data: string) {
    return this.forumService.getSafe(data);
  }

  edit(id: number, type: string) {
    this.route.navigate(['/forum', 'ed', type, id]);
  }
  navToReply(articleId: number) {
    this.route.navigate(['/forum', 'new', 'post', articleId]);
  }

  dislike(e:any) {
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
