import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { QuillEditorComponent } from 'ngx-quill';
import { ArticleView } from 'src/app/interfaces/forumInterface/ArticleView';
import { memberName } from 'src/app/interfaces/forumInterface/memberIName';
import { Post } from 'src/app/interfaces/forumInterface/Post';
import { Theme } from 'src/app/interfaces/forumInterface/Theme';
import ForumService from 'src/app/services/forumService/forum.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
})
export class EditComponent implements OnInit {
  isSubmitting: boolean = false;
  isLoading: boolean = false;
  formSubmitted: boolean = false;


  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any): void {
    // 只有當表單已修改且未提交時，才顯示提示
    if (this.articleForm.dirty) {
      console.log('預設的');
      $event.returnValue = true;
    }
  }
  @ViewChild('quillEditor') quillEditor!: QuillEditorComponent;
  @ViewChild('fileInput') fileInput: any;
  articleForm!: FormGroup<any>;
  article!: ArticleView;
  articleId = 0;
  post!: Post;
  id?: number | null;
  type!: string;
  themeTag: Theme[] = [];
  user: memberName = {
    memberId: 0,
    nickName: '',
  };
  constructor(
    private fb: FormBuilder,
    private forumService: ForumService,
    private actRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.forumService.user$.subscribe((data) => (this.user = data));
    this.forumService.loadCss('../../../assets/css/quill.snow.css');
    this.initializeParams();

    this.initializeForm();
    if (this.type === 'post') {
      this.changeFormGroup();
    } else {
      this.forumService.themeTag$.subscribe({
        next: (data) => (this.themeTag = data),
        error: (err) => this.handleError(err),
        complete: () => (this.isLoading = false),
      });
    }

    this.loadingContent();
  }
  private initializeParams() {
    const idRoute = this.actRoute.snapshot.paramMap.get('id');
    this.id = idRoute ? Number(idRoute) : null;
    this.articleId = Number(this.actRoute.snapshot.paramMap.get('articleId'));
    this.type = String(this.actRoute.snapshot.paramMap.get('type'));
  }

  private initializeForm() {
    this.articleForm = this.fb.group({
      content: ['', [Validators.required, Validators.minLength(20)]],
      title: ['', Validators.required],
      theme: [null, Validators.required],
    });
  }

  private changeFormGroup() {
    this.articleForm.removeControl('title');
    this.articleForm.removeControl('theme');
  }

  private loadingContent() {
    this.isLoading = true;

    if (!this.id) {
      this.isLoading = false;
      return;
    }
    if (this.type === 'post') {
      this.loadPost();
    } else {
      this.loadArticle();
    }
  }

  private loadPost() {
    this.forumService.getPost(this.id!).subscribe({
      next: (data: Post) => {
        this.post = data;
        this.articleForm.patchValue({ content: data.postContent });
      },
      error: (err) => this.handleError(err),
      complete: () => (this.isLoading = false),
    });
  }

  private loadArticle() {
    this.forumService.getArticle(this.id!).subscribe((data) => {
      this.article = data;
      this.articleForm.patchValue({
        content: data.articleContent,
        title: data.title,
        theme: data.themeId,
      });
    });
  }

  safeHtml(data: string) {
    return this.forumService.getSafe(data);
  }

  onSubmit(): void {
    if (this.articleForm.invalid) return;

    this.isSubmitting = true;

    const formData = this.articleForm.getRawValue();
    const action = this.id ? this.updateContent : this.createContent;

    action.call(this, formData).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (err) => this.handleError(err),
      complete: () => {
        this.navigateBack();
      },
    });
  }

  private updateContent(formData: any) {
    if (this.type === 'article') {
      return this.forumService.updateArticle(this.id!, {
        articleContent: formData.content,
        themeId: formData.theme,
        title: formData.title,
      });
    } else {
      return this.forumService.updatePost(this.id!, {
        postContent: formData.content,
      });
    }
  }

  private createContent(formData: any) {
    if (this.type === 'article') {
      return this.forumService.createArticle({
        articleContent: formData.content,
        themeId: formData.theme,
        title: formData.title,
        articleImage: '',
        authorId: this.user.memberId, // 使用方法獲取當前用戶ID
        lock: true,
        nickName: '',
        postDate: new Date(),
        replyCount: 0,
        themeName: '',
        updateDate: new Date(),
        articleId: 0,
        likeCount: 0,
        dislikeCount: 0,
      } as ArticleView);
    } else {
      return this.forumService.createPost({
        postContent: formData.content,
        articleId: this.articleId,
        postId: 0, // 新帖子ID由後端生成
        posterId: this.user.memberId, // 使用方法獲取當前用戶ID
        postDate: new Date(),
        lock: true,
        postImage: '',
        nickName: '', // 這可能需要從用戶服務取得
        likeCount: 0,
        dislikeCount: 0,
      } as Post);
    }
  }

  openFile() {
    this.fileInput.nativeElement.click();
  }
  public async onFileSelected(event: any) {
    const file = event.target.files[0];
    if (!this.isValidImage(file)) {
      event.target.value = '';
      alert('請選擇有效的圖片檔案');
      return;
    }

    const formData = new FormData();
    formData.append('UserPhoto', file);
    await this.uploadImg(formData);
  }

  private isValidImage(file: File): boolean {
    return file && file.type.startsWith('image/') && file.size > 0;
  }

  private async uploadImg(formData: FormData) {
    try {
      // 調用服務上傳圖片，這裡假設後端返回一個圖片 URL
      const response: any = await this.forumService.getPicture(formData); // 上傳文件

      if (response && response.filePath) {
        // 確保後端返回文件 URL
        const quill = this.quillEditor.quillEditor;
        let range = quill.getSelection();

        // 如果沒有選中的位置，將圖片插入到末尾
        if (!range) {
          range = {
            index: quill.getLength(),
            length: 0,
          };
        }
        if (this.type === 'article') {
          const newArticle =
            '這是一篇推薦影評人最推崇的影片，內容涵蓋各類經典佳作。';

          // 將圖片 URL 插入到 Quill 編輯器
          quill.insertEmbed(range.index, 'image', response.filePath);
          quill.insertText(range.index + 100, newArticle);
          this.articleForm.patchValue({
            content: quill.root.innerHTML, // 更新內容欄位
            title: '推薦影評人必看的電影', // 填寫標題欄位
            theme: 5, // 填寫主題欄位
          });
          this.articleForm.controls['content'].setValue(quill.root.innerHTML);
        } else {
          const newPost = `這部可以看啊！
          從頭到尾沒有冷場啊！

          節奏拿捏的很好！

          而且不得不說，這女主角演技很棒！

          我會二刷~~
          `;

          // 將圖片 URL 插入到 Quill 編輯器
          quill.insertEmbed(range.index, 'image', response.filePath);
          quill.insertText(range.index + 100, newPost);
          this.articleForm.patchValue({
            content: quill.root.innerHTML, // 更新內容欄位
          });
          this.articleForm.controls['content'].setValue(quill.root.innerHTML);
        }
      } else {
        console.error('圖片上傳失敗，未返回有效的文件路徑');
      }
    } catch (error) {
      console.error('圖片上傳發生錯誤:', error);
    }
  }
  private handleError(error: any) {
    console.error('發生錯誤:', error);
    this.isSubmitting = false;
    // TODO: 添加顯示錯誤訊息給用戶的邏輯
  }

  private navigateBack(): void {
    setTimeout(() => {
      if (this.type === 'post' && this.articleId) {
        this.router.navigate(['forum', this.articleId]);
      } else {
        this.router.navigate(['forum']);
      }
    }, 0);
  }
}
