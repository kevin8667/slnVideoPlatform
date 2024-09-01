import { AfterViewInit, Component, ElementRef, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Theme } from 'src/app/interface/Theme';
import { ForumService } from 'src/app/service/forum.service';

@Component({
  selector: 'app-new-article',
  templateUrl: './new-article.component.html',
  styleUrls: ['./new-article.component.css'],
})
export class NewArticleComponent implements OnInit {
  articleForm: any;
  themeTag: Theme[] = [];

  constructor(private fb: FormBuilder, private forumService: ForumService) {}

  // ngAfterViewInit() {
  //   new Promise<void>((resolve, reject) => {
  //     const script = document.createElement('script');
  //     script.src = 'https://cdn.ckeditor.com/4.22.1/standard/ckeditor.js';
  //     script.onload = () => resolve();
  //     script.onerror = () =>
  //       reject(new Error('CKEditor script loading failed'));
  //     document.body.appendChild(script);
  //   })
  //     .then(() => {
  //       // 确保 CKEditor 在脚本加载完成后初始化
  //       // @ts-ignore
  //       if (typeof CKEDITOR !== 'undefined') {
  //         // @ts-ignore
  //         CKEDITOR.replace('content', {
  //           width: '100%',
  //         });
  //       } else {
  //         console.error('CKEditor is not available');
  //       }
  //     })
  //     .catch((error) => {
  //       console.error(error);
  //     });
  // }

  ngOnInit(): void {
    this.articleForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
      themeTag: [null, Validators.required],
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
