import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import ForumService from 'src/app/service/forum.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
})
export class EditComponent {
  articleForm: any;
  constructor(private fb: FormBuilder, private forumService: ForumService) {}
  ngOnInit(): void {
    this.articleForm = this.fb.group({
      content: ['', Validators.required],
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
