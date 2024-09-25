
import { VideoDBService } from 'src/app/video-db.service';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { CreateVideoDTO } from '../interfaces/CreateVideoDTO';
import { ImageDTO } from '../interfaces/CreateVideoDTO';
import { HttpClient } from '@angular/common/http';
import { FileUpload } from 'primeng/fileupload';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';

interface VideoType
{
  typeName:string;
  typeId:number;
}

@Component({
  selector: 'app-new-video',
  templateUrl: './new-video.component.html',
  styleUrls: ['./new-video.component.css'],
  providers:[MessageService]
})
export class NewVideoComponent implements OnInit {
  @ViewChild('thumbnailUpload') thumbnailUpload: FileUpload | undefined;
  @ViewChild('bgPreview') bgUpload: FileUpload | undefined;


  videoForm: FormGroup;

  thumbnailPreview: string | ArrayBuffer | null = null;
  bgPreview: string | ArrayBuffer | null = null;
  imagePreview: (string | ArrayBuffer | null)[] = [];

  uploadedImages:  (File | undefined)[] = [];
  uploadedThumbnail!: File;  // 儲存縮圖
  uploadedBg!: File; 

  types = [
    { typeName: '電影', typeId: 1 },
    { typeName: '影集', typeId: 2 }
  ];

  genres:any[]=[];

  isEditing:boolean= false;

  selectedType: VideoType | undefined;

  constructor(private fb: FormBuilder, private http: HttpClient, private route: ActivatedRoute, private videoDBService:VideoDBService,private messageService: MessageService, private router:Router) {
    // 初始化表單
    this.videoForm = this.fb.group({
      videoName: ['', Validators.required],
      typeId: [null, Validators.required],
      seriesId: [null],
      mainGenreId: ['', Validators.required],
      seasonId: [null],
      episode: [null],
      runningTime: [''],
      isShowing: [false, Validators.required],
      releaseDate: [new Date(), Validators.required],
      thumbnailPath: [''],
      lang: ['', Validators.required],
      summary: [''],
      ageRating: [''],
      trailerUrl: [''],
      bgpath: [''],
      images: this.fb.array([]) // 確保這裡初始化為 FormArray
    });

  }

  ngOnInit(): void {

    this.videoDBService.getGenresApi().subscribe(data=>{
      this.genres= data;
    })

    this.route.queryParams.subscribe(params =>{
      if(params["videoID"]){
          this.isEditing= true;
          this.videoDBService.getVideoApiWithID(params["videoID"]).subscribe(data=>{
            this.videoDBService.getImagesByVideoID(data.videoId.toString()).subscribe(imgs=>{
              console.log(imgs)
              this.videoForm.patchValue({
                image:imgs
              })
            })

            this.videoForm.patchValue({
              videoName: data.videoName,
              typeId: data.typeId,
              seriesId: data.seasonId,
              mainGenreId: data.mainGenreId,
              seasonId: data.seasonId,
              episode: data.episode,
              runningTime: data.runningTime,
              isShowing: data.isShowing,
              releaseDate: data.releaseDate,
              thumbnailPath: '',
              lang: data.lang,
              summary: data.summary,
              ageRating: data.ageRating,
              trailerUrl: data.trailerUrl,
              bgpath:data.bgpath,
            });
          })
      }
    })
  }

  testValue(){
    console.log(this.videoForm.value);
    //console.log(typeof this.videoForm.value.typeId);
  }

  completeForm()
  {
  this.videoForm.patchValue({
    videoName: "TEST",
    typeId: 1,
    mainGenreId: 1,
    lang: "TEST"
  });
  }

  get imagesArray(): FormArray {
    return this.videoForm.get('images') as FormArray;
  }

  onSubmit() {
    if (this.videoForm.valid) {
      const video: CreateVideoDTO = this.videoForm.value;

      // 先上傳所有圖片，然後更新表單的圖片路徑
      const formData = new FormData();

      // 上傳縮圖
      if (this.uploadedThumbnail) {
        formData.append('Thumbnail', this.uploadedThumbnail, this.uploadedThumbnail.name);
      } else {
        console.error('No thumbnail selected');  // 確認是否有選擇縮圖
        return;  // 如果沒有選擇縮圖，則返回並不繼續
      }

      if (this.uploadedBg) {
        formData.append('Background', this.uploadedBg, this.uploadedBg.name);
      }


      // 上傳其他圖片
      this.uploadedImages.forEach((file) => {
        if (file) {
          formData.append('Images', file, file.name);  // 將 key 統一為 'images'
        }
      });

      formData.forEach((value, key) => {
        console.log(key, value);
      });

      // 上傳圖片並取得檔案路徑
      this.http.post<any>('https://localhost:7193/api/VideoList/uploadImages', formData).subscribe(
        (response) => {
          console.log(response);

          // 成功上傳後，將檔案路徑更新到表單資料
          video.thumbnailPath = response.thumbnailPath;  // 更新縮圖路徑
          video.bgpath = response.bgPath;
          video.images = response.imagePaths.map((paths: any) => ({ imagePath: paths }));  // 將圖片路徑轉換為 ImageCreateDTO 格式

          const imageControls = this.imagesArray.controls;

          response.imagePaths.forEach((path: string, index: number) => {
            if (imageControls[index]) {
              imageControls[index].get('imageUrl')?.setValue(path);  // 更新圖片路徑
            }
          });

          console.log(this.imagesArray);

          console.log(video);
          // 提交表單資料到後端
          this.http.post<any>(`https://localhost:7193/api/VideoList/newVideo=${video.videoName}`, video).subscribe(
            (res) => {
              console.log('表單提交成功', res);
              this.messageService.add({
                key: 'global',
                severity: 'success',
                summary: '成功',
                detail: '已成功新增影片! 將在3秒後跳轉...'
              });

              setTimeout(() => {
                console.log(res.videoId);
                this.router.navigate([`/details/${res.videoId}`])
              }, 3000);
            },
            (error) => {
              console.error('表單提交失敗', error);
              this.messageService.add({ severity: 'warning', summary: '失敗', detail: '新增影片失敗!' });
            }
          );
        },
        (error) => {
          console.error('圖片上傳失敗：', error);
        }
      );
    }
  }

  onImageSelect(event: any) {
    const files = event.files as File[]; // 獲取選中的檔案

  // 確保 files 存在
  if (files && files.length > 0) {

    Array.from(files).forEach((file: File) => {
      this.uploadedImages.push(file);

      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview.push(reader.result);
      };
      reader.readAsDataURL(file);
    });
  }

  console.log(this.uploadedImages);
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

  onBackgroundSelect(event: any) {
    const file = event.files[0]; // 獲取選中的背景圖片檔案

    // 將背景圖暫存
    this.uploadedBg = file;

    const reader = new FileReader();
    reader.onload = () => {
      this.bgPreview = reader.result;  // 背景圖片預覽
    };
    reader.readAsDataURL(file); // 預覽背景圖
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
