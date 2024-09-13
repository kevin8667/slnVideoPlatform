import { Theme } from '../../interfaces/forumInterface/Theme';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import ForumService from 'src/app/service/forum.service';

@Component({
  selector: 'app-new-article',
  templateUrl: './new-article.component.html',
  styleUrls: ['./new-article.component.css'],
})
export class NewArticleComponent implements OnInit {
  articleForm: any;
  themeTag: Theme[] = [];
  constructor(private fb: FormBuilder, private forumService: ForumService) {}

  ngOnInit(): void {
    this.articleForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
      theme: [null, Validators.required],
    });
    this.forumService.themeTag$.subscribe((data) => {
      this.themeTag = data;
    });
  }

  onSubmit(): void {
    if (this.articleForm.valid) {
      const articleData = this.articleForm.value;
      console.log('文章資料:', articleData);
      // 在這裡處理提交邏輯，比如呼叫 API 發送資料
    }
  }
}
