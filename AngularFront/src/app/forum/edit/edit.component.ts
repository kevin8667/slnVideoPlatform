import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { QuillEditorComponent } from 'ngx-quill';
import { ArticleView } from 'src/app/interfaces/forumInterface/ArticleView';
import { Post } from 'src/app/interfaces/forumInterface/Post';
import { Theme } from 'src/app/interfaces/forumInterface/Theme';
import ForumService from 'src/app/service/forum.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
})
export class EditComponent implements OnInit {
  isSubmitting: boolean = false;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any): void {
    if (this.articleForm.touched && !this.isSubmitting) {
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

  constructor(
    private fb: FormBuilder,
    private forumService: ForumService,
    private actRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const idRoute = this.actRoute.snapshot.paramMap.get('id');
    this.id = idRoute ? Number(idRoute) : null;
    this.articleId = Number(this.actRoute.snapshot.paramMap.get('articleId'));
    this.type = String(this.actRoute.snapshot.paramMap.get('type'));

    this.initializeForm();
    if (this.type === 'post') {
      this.changeFormGroup();
    }

    this.loadingContent();
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
    this.forumService.themeTag$.subscribe((data) => (this.themeTag = data));
    if (!this.id) return;

    if (this.type === 'post') {
      this.forumService.getPost(this.id).subscribe((data: Post) => {
        this.post = data;
        this.articleForm.patchValue({
          content: data.postContent,
        });
      });
    } else {
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
    this.isSubmitting = true;

    const articleValue = this.articleForm.getRawValue();
    const postValue = this.articleForm.getRawValue();

    try {
      if (this.id) {
        if (this.type === 'article') {
          const updatedData: Partial<ArticleView> = {
            articleContent: articleValue['content'],
            themeId: articleValue['theme'],
            title: articleValue['title'],
          };

          this.forumService.updateArticle(this.id, updatedData).subscribe({
            next: (response) => console.log(response),
            error: (err) => {
              console.error('傳送資料發生意外:', err);
              this.isSubmitting = false;
            },
            complete: () => {
              this.isSubmitting = false;
              history.back();
            },
          });
        } else {
          const updatedData: Partial<Post> = {
            postContent: postValue['content'],
          };

          this.forumService.updatePost(this.id, updatedData).subscribe({
            next: (response) => console.log(response),
            error: (err) => {
              console.error(err);
              this.isSubmitting = false;
            },
            complete: () => {
              this.isSubmitting = false;
              history.back();
            },
          });
        }
      } else {
        if (this.type === 'article') {
          // 新增文章
          const updatedData: ArticleView = {
            articleContent: articleValue['content'],
            themeId: articleValue['theme'],
            title: articleValue['title'],
            articleImage: '',
            authorId: 2,
            lock: true,
            nickName: '',
            postDate: new Date(),
            replyCount: 0,
            themeName: '',
            updateDate: new Date(),
            articleId: 0,
            likeCount: 0,
            dislikeCount: 0,
          };
          this.forumService.createArticle(updatedData).subscribe({
            next: (response) => console.log(response),
            error: (err) => {
              console.error('傳送資料發生意外:', err);
              this.isSubmitting = false;
            },
            complete: () => {
              this.isSubmitting = false;
              history.back();
            },
          });
        } else {
          // 新增回文
          if (this.articleId === 0) return;
          const updatedData: Post = {
            postContent: postValue['content'],
            articleId: this.articleId,
            postId: 0,
            posterId: 5,
            postDate: new Date(),
            lock: true,
            postImage: '',
            nickName: '',
            likeCount: 0,
            dislikeCount: 0,
          };
          this.forumService.createPost(updatedData).subscribe({
            next: (response) => console.log(response),
            error: (err) => {
              console.error('傳送資料發生意外:', err);
              this.isSubmitting = false;
            },
            complete: () => {
              this.isSubmitting = false;
              history.back();
            },
          });
        }
      }
    } catch (error) {
      console.error('發生例外的錯誤:', error);
      this.isSubmitting = false;
      return;
    }
  }

  openFile() {
    this.fileInput.nativeElement.click();
  }
  onFileSelected(event: any) {
    const selectValue = event.target.files[0];
    const imageType = /image.*/;

    if (!selectValue.type.match(imageType) || selectValue.size === 0) {
      event.target.value = '';
      alert('請選擇圖片');
      return;
    }
    const formData = new FormData();
    formData.append('UserPhoto', selectValue);

    this.uploadImg(formData);
    // const reader = new FileReader();

    // reader.onload = () => {
    //   this.uploadImg(reader.result);
    // };
    // reader.readAsDataURL(selectValue)
  }

  async uploadImg(formData: FormData) {
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

        // 將圖片 URL 插入到 Quill 編輯器
        quill.insertEmbed(range.index, 'image', response.filePath);

        this.articleForm.controls['content'].setValue(quill.root.innerHTML);
      } else {
        console.error('圖片上傳失敗，未返回有效的文件路徑');
      }
    } catch (error) {
      console.error('圖片上傳發生錯誤:', error);
    }
  }
}
