import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-new-post',
  templateUrl: './new-post.component.html',
  styleUrls: ['./new-post.component.css'],
})
export class NewPostComponent implements OnInit {
  articleForm: any;
  ngOnInit(): void {
    this.articleForm = this.fb.group({
      content: ['', Validators.required],
    });
  }

  constructor(private fb: FormBuilder) {}

  onSubmit(): void {
    if (this.articleForm.valid) {
      const articleData = this.articleForm.value;
      console.log('文章資料:', articleData);
      // 在這裡處理提交邏輯，比如呼叫 API 發送資料
    }
  }
}
