import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-new-article',
  templateUrl: './new-article.component.html',
  styleUrls: ['./new-article.component.css'],
})
export class NewArticleComponent implements OnInit {
  articleForm: any;
  ngOnInit(): void {
    this.articleForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
      category: [null, Validators.required],
      tags: [[], Validators.required],
    });
  }
  categories = [
    { id: 1, name: '技術' },
    { id: 2, name: '生活' },
    { id: 3, name: '娛樂' },
  ];
  tagOptions = [
    { label: 'Angular', value: 'Angular' },
    { label: 'TypeScript', value: 'TypeScript' },
    { label: 'JavaScript', value: 'JavaScript' },
  ];

  constructor(private fb: FormBuilder) {}

  onSubmit(): void {
    if (this.articleForm.valid) {
      const articleData = this.articleForm.value;
      console.log('文章資料:', articleData);
      // 在這裡處理提交邏輯，比如呼叫 API 發送資料
    }
  }
}
