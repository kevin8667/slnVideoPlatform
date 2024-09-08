import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ArticleView } from 'src/app/interface/ArticleView';
import { Post } from 'src/app/interface/Post';
import { Theme } from 'src/app/interface/Theme';
import ForumService from 'src/app/service/forum.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
})
export class EditComponent {
  articleForm!: FormGroup;
  article!: ArticleView;
  post!: Post;
  id!: number;
  type!: string;
  themeTag: Theme[] = [];

  constructor(
    private fb: FormBuilder,
    private forumService: ForumService,
    private actRoute: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.articleForm = this.fb.group({
      content: ['', [Validators.required, Validators.minLength(20)]],
      title: ['', this.type === 'article' ? Validators.required : null],
      theme: [0, this.type === 'article' ? Validators.required : null],
    });

    this.id = Number(this.actRoute.snapshot.paramMap.get('id'));
    this.type = String(this.actRoute.snapshot.paramMap.get('type'));

    if (this.type !== 'post' && this.type !== 'article') {
      console.error('無效的 type 值:', this.type);
      // 如果無效，可以拋出錯誤或者導航到錯誤頁面
      // this.router.navigate(['/error']);
      return;
    }
    if (this.type === 'post') {
      this.forumService.getPost(this.id).subscribe((data: Post) => {
        this.post = data;
        this.articleForm.patchValue({
          content: data.postContent,
        });
      });
    }
    if (this.type === 'article') {
      this.forumService.themeTag$.subscribe((data) => (this.themeTag = data));
      this.forumService.getArticle(this.id).subscribe((data) => {
        this.article = data;
        this.articleForm.patchValue({
          content: data.articleContent,
          title: data.title,
          theme: data.themeId,
        });
      });
    }
  }

  safeHtml(data: string) {
    return this.forumService.getSafe(data);
  }

  onSubmit(): void {
    if (this.articleForm.invalid) return;

    if (this.type === 'article') {
      this.article.articleContent = this.articleForm.controls['content'].value;
      this.article.title = this.articleForm.controls['title'].value;
      this.article.themeId = this.articleForm.controls['theme'].value;

      this.forumService.updateArticle(this.id, this.article).subscribe({
        next: (response) => console.log(response),
        error: (err) => console.error('傳送資料發生意外:', err),
        complete: () => history.back(),
      });
    }

    if (this.type === 'post') {
      this.post.postContent = this.articleForm.controls['content'].value;
      this.forumService.updatePost(this.id, this.post).subscribe({
        next: (response) => console.log(response),
        error: (err) => console.error(err),
        complete: () => history.back(),
      });
    }
  }
}
