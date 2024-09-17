import { Component, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { CreateVideoDTO } from '../interfaces/CreateVideoDTO';
import { ImageDTO } from '../interfaces/CreateVideoDTO';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-new-video',
  templateUrl: './new-video.component.html',
  styleUrls: ['./new-video.component.css']
})
export class NewVideoComponent {

  videoForm: FormGroup;
  thumbnailPreview: string | ArrayBuffer | null = null;
  imagePreview: (string | ArrayBuffer | null)[] = [];

  uploadedImages:  (File | undefined)[] = [];
  uploadedThumbnail!: File;  // 儲存縮圖

  constructor(private fb: FormBuilder, private http: HttpClient) {
    // 初始化表單
    this.videoForm = this.fb.group({
      videoName: ['', Validators.required],
      typeId: ['', Validators.required],
      seriesId: [null],
      mainGenreId: ['', Validators.required],
      seasonId: [null],
      episode: [null],
      runningTime: [''],
      isShowing: [false, Validators.required],
      releaseDate: [new Date(), Validators.required],
      rating: [null],
      popularity: [null],
      thumbnailPath: [''],
      lang: ['', Validators.required],
      summary: [''],
      ageRating: [''],
      trailerUrl: [''],
      bgpath: [''],
      images: this.fb.array([]) // 確保這裡初始化為 FormArray
    });
  }

  onSubmit() {
    if (this.videoForm.valid) {
      const video: CreateVideoDTO = this.videoForm.value;
  
      // 先上傳所有圖片，然後更新表單的圖片路徑
      const formData = new FormData();
  
      // 上傳縮圖
      if (this.uploadedThumbnail) {
        formData.append('thumbnail', this.uploadedThumbnail, this.uploadedThumbnail.name);
      } else {
        console.error('No thumbnail selected');  // 確認是否有選擇縮圖
        return;  // 如果沒有選擇縮圖，則返回並不繼續
      }
  
      // 上傳其他圖片
      this.uploadedImages.forEach((file, index) => {
        if (file) {
          formData.append(`images[${index}]`, file, file.name);
        }
      });
  
      // 上傳圖片並取得檔案路徑
      this.http.post<any>('https://localhost:7193/api/VideoList/uploadImages', formData).subscribe(
        (response) => {
          // 成功上傳後，將檔案路徑更新到表單資料
          video.thumbnailPath = response.thumbnailPath;  // 更新縮圖路徑
          video.images = response.imagePaths;  // 更新其他圖片路徑
  
          // 提交表單資料到後端
          this.http.post<any>(`https://localhost:7193/api/VideoList/newVideo=${video.videoName}`, video).subscribe(
            (res) => {
              console.log('表單提交成功', res);
            },
            (error) => {
              console.error('表單提交失敗', error);
            }
          );
        },
        (error) => {
          console.error('圖片上傳失敗：', error);
        }
      );
    }
  }

  // 動態新增圖片 (images)
  addImage() {
    this.uploadedImages.push(undefined); // 為新的圖片預留位置
    this.imagePreview.push(null); // 為新的預覽預留位置
  }

  onImageSelect(event: any, index: number) {
    const file = event.files[0]; // 獲取選中的檔案
  
    // 將檔案暫存
    this.uploadedImages[index] = file;
  
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview[index] = reader.result;
    };
    reader.readAsDataURL(file); // 預覽圖片
  }


 // 選擇縮圖圖片，不上傳，只是暫存檔案
onThumbnailSelect(event: any) {
  const file = event.files[0]; // 獲取選中的檔案

  // 將檔案暫存
  this.uploadedThumbnail = file;

  const reader = new FileReader();
  reader.onload = () => {
    this.thumbnailPreview = reader.result;
  };
  reader.readAsDataURL(file); // 預覽圖片
}

  // 將 Base64 資料轉換為 Blob
  dataURLtoBlob(dataUrl: string): Blob {
    const arr = dataUrl.split(',');
    const mime = arr[0].match(/:(.*?);/)![1];
    const bstr = atob(arr[1]);
    let n = bstr.length;
    const u8arr = new Uint8Array(n);
    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }
    return new Blob([u8arr], { type: mime });
  }

}
