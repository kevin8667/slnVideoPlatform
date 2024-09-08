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
  articleForm!: FormGroup<any>;
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
    this.id = Number(this.actRoute.snapshot.paramMap.get('id'));
    this.type = String(this.actRoute.snapshot.paramMap.get('type'));
    this.articleForm = this.fb.group({
      content: ['', [Validators.required, Validators.minLength(20)]],
      title: ['', Validators.required],
      theme: [null, Validators.required],
    });
    if (this.type !== 'article') {
      this.articleForm.removeControl('title');
      this.articleForm.removeControl('theme');
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
    try {
      if (this.type === 'article') {
        const articleValue = this.articleForm.getRawValue();
        const updatedData: Partial<ArticleView> = {
          articleContent: articleValue['content'],
          themeId: articleValue['theme'],
          title: articleValue['title'],
        };

        this.forumService.updateArticle(this.id, updatedData).subscribe({
          next: (response) => console.log(response),
          error: (err) => console.error('傳送資料發生意外:', err),
          complete: () => history.back(),
        });
      }

      if (this.type === 'post') {
        const postValue = this.articleForm.getRawValue();

        const updatedData: Partial<Post> = {
          postContent: postValue['content'],
        };
        this.forumService.updatePost(this.id, updatedData).subscribe({
          next: (response) => console.log(response),
          error: (err) => console.error(err),
          complete: () => history.back(),
        });
      }
    } catch (error) {
      console.error('發生例外的錯誤:', error);
      return;
    } finally {
      return;
    }
  }
}
