import { memberName } from 'src/app/interfaces/forumInterface/memberIName';
import { Component, OnInit, Input } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-info',
  templateUrl: './mmain.component.html',
  styleUrls: ['./mmain.component.css'],
  providers: [MemberService],
})
export class MmainComponent implements OnInit {
  cantSee = true;
  memberId: number = 10;
  memberData: any = {};
  latestNews: any[] = [];
  password = '';
  canSee: boolean = false; // 控制顯示輸入框或標籤
  canSeepwd: boolean = false; // 控制密碼的顯示/隱藏
  disableline: boolean =true;

  constructor(
    private memberService: MemberService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {

    this.authService.MemberBehaviorData.subscribe((memberData) => {
      console.log("MemberBehabiorData >>")
      console.log(memberData);
    });


    this.authService.isLoggedIn.subscribe((isLoggedIn) => {
      if (isLoggedIn) {
        this.readMemberData();
        this.loadLatestNews();
      } else {

        this.router.navigateByUrl('login');
      }
    });
  }

  seepwd() {
    this.canSeepwd = true;
  }

  cantseepwd() {
    this.canSeepwd = false;
  }

  disablelinebtn(){
    if(this.memberData.bindingLine !='Y'){
      this.disableline = true;
    }
    else{
      this.disableline = false;
    }

  }

  modifyClick() {
    this.canSee = !this.canSee;
    this.cantSee = !this.cantSee;
    if (!this.canSee) {
      this.canSeepwd = false;
    }
  }
  Return() {
    this.canSee = !this.canSee;
    this.cantSee = !this.cantSee;
    this.readMemberData();
  }

  SaveData() {
    const confirmation = confirm("確定修改資料嗎？");
  if (confirmation){
    this.memberService.updatememberdata(this.memberData).subscribe({
      next: (response) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
        if (response.isSuccess) {
          this.memberData = response.data;
          this.modifyClick(); // 切換回顯示模式
        }
      },
      error: (error) => {
        console.error('SaveData error:', error);
        alert('儲存資料失敗');
      },
    });}
    else{
      this.readMemberData();
      return;
    }
  }

  readMemberData() {
    this.memberService.readmemberdata().subscribe({
      next: (response) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
        if (response.isSuccess) {
          this.memberData = response.data;
          if (typeof this.memberData.birth === 'string') {
            this.memberData.birth = this.formatDate(this.memberData.birth);
          }
          this.disablelinebtn();
        }
      },
      error: (error) => {
        console.error('readMemberData error:', error);
        alert(error.error.message);
        this.router.navigateByUrl('login');
      },
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

loadLatestNews() {
  this.memberService.readmemberNotice().subscribe({
    next: (response) => {
      this.latestNews = Array.isArray(response.datas)
        ? response.datas.map((item: any) => ({
            id: item.memberNoticeId,
            date: item.creTime,
            title: item.title,
            summary: item.noticeContent,
            showSummary: false,
          }))
        : [];

      if (response.hasAlertMsg) {
        alert(response.alertMsg);
      }
    },
    error: (error) => {
      console.error('loadLatestNews error:', error);
      alert('讀取訊息失敗');
    },
  });
}


  toggleSummary(newsId: number) {
    debugger;
    const newsItem = this.latestNews.find((news) => news.id === newsId);
    if (newsItem) {
      newsItem.showSummary = !newsItem.showSummary;
    }
  }

  onEditClick() {
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = 'image/*';
    fileInput.style.display = 'none';
    document.body.appendChild(fileInput);

    fileInput.addEventListener('change', (event: Event) => {
      const files = (event.target as HTMLInputElement).files;
      if (files && files.length > 0) {
        const file = files[0];
        this.uploadPicture(file);
      }
      document.body.removeChild(fileInput);
    });

    fileInput.click();
  }

  uploadPicture(file: File) {
    this.memberService.updatememberPic(file).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.memberData.photoPath = response.data.photoPath;
          this.memberService.updatememberdata(this.memberData).subscribe({
            next: (response) => {
              if (response.hasAlertMsg) {
                alert(response.alertMsg);
              }
              if (response.isSuccess) {
                this.memberData = response.data;
              }
            },
            error: (error) => {
              console.error('SaveData error:', error);
              alert('儲存資料失敗');
            },
          });
        } else {
          alert('圖片上傳失敗，請重試。');
        }
      },
      error: (error) => {
        console.error('Picture upload failed:', error);
        alert('圖片上傳失敗');
      },
    });
  }

  oncoupon() {
    this.router.navigateByUrl('login/coupon');
  }

  onhistory() {
    this.router.navigateByUrl('login/history');
  }

  onfriends() {
    this.router.navigateByUrl('login/friends');
  }

  onmessage() {
    this.router.navigateByUrl('login/message');
  }

  onroulette(){
    this.router.navigateByUrl('login/roulette');
  }

  onplaylist(){
    this.router.navigateByUrl('playlist/member');
  }

  getMemberGenderDescription(): string {
    switch (this.memberData.gender) {
      case 'M':
        return '男性';
      case 'F':
        return '女性';
      default:
        return '未知';
    }
  }

  getMemberGradeDescription(): string {
    switch (this.memberData.grade) {
      case 'A':
        return '一般會員';
      case 'B':
        return '進階會員';
      default:
        return '未知等級';
    }
  }

  goToPlaylist() {
    this.router.navigate(['/playlist/member']);
  }

  BindingLine(): void {
                this.authService.loginWithLine(true);
}


private getCurrentLineId(): string {

    return "example_line_id";
}

demo(){
  this.memberData.memberName='陳俊廷'
  this.memberData.password='Ting54ting'
  this.memberData.nickName='小Paul'
  this.memberData.gender='M'
  this.memberData.phone='0956566566'
  this.memberData.address='台北市中正區重慶南路二段5號'
}
}
