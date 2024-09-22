import { Component } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MessageService } from 'primeng/api';





@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [MemberService,DatePipe,MessageService]
})


export class RegisterComponent {
  email: string = '';
  pwd: string = '';
  nickname: string = '';
  membername: string = '';
  phone: string = '';
  birth: Date = new Date('2000-01-01');
  address: string = '';
  gender: string = '';
  uploadedFiles: any[] = [];

  previewImage: string | ArrayBuffer | null = null;


  constructor(private memberService: MemberService,private router: Router,private messageService: MessageService){
  }

  onSelect(event: any) {
    const file = event.files[0]; // 只處理單張圖片
    if (file) {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        this.previewImage = e.target.result;
      };

      reader.readAsDataURL(file); // 讀取圖片並轉換為 base64 字符串
    }
  }

  onUpload(event: any) {
    // 上傳完成後清除預覽
    this.previewImage = null;
    this.messageService.add({severity: 'info', summary: 'File Uploaded', detail: ''});
  }

  removeImage(): void {
    this.previewImage = null;
  }
  onsave(){
    console.log("email :"+this.email);
    console.log("pwd :"+this.pwd);
    console.log("nickname :"+this.nickname);
    console.log("membername :"+this.membername);
    console.log("phone :"+this.phone);
    console.log("birth :"+this.birth);
    console.log("address :"+this.address);
    console.log("gender :"+this.gender);
    console.log("uploadedFiles :"+this.uploadedFiles);

    this.memberService.register(this.email, this.pwd, this.nickname, this.membername, this.phone, this.birth, this.address, this.gender, this.uploadedFiles).subscribe({
      next: (response) => {
        debugger
        if(response.hasAlertMsg){
          alert(response.alertMsg);
        }
        if(response.isSuccess){

          this.router.navigateByUrl('login');

        }
      },
      error: (error) => {
        console.error('Register error:', error);
        alert('Register failed');
      }
    });
  }

  back(){
    this.router.navigateByUrl('login');
  }

  setCookie(name: string, value: string, days: number) {
    const expires = new Date();
    expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
    const expiresString = "expires=" + expires.toUTCString();
    document.cookie = `${name}=${value}; ${expiresString}; path=/`;
}

demo(){
  this.email='Jajajademo28825252@gmail.com';
  this.pwd='Jajajademo28825252';
  this.membername='黃嘉宇';
  this.nickname='資展轉職仔';
  this.phone='0933682666';
  this.birth = new Date('1991/03/29')
  this.gender='M';
  this.address='台北市萬華區西藏路424號';
}
}


