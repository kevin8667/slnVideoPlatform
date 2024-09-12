import { Component, OnInit } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css'],
  providers: [MemberService],
})
export class MessageComponent implements OnInit {
  messages: any[] = []; // 初始化为数组

  constructor(private memberService: MemberService, private router: Router) {}

  ngOnInit() {
    this.readmessages();
  }
  filteredMessages = [...this.messages]; // 初始化為所有訊息
  readmessages() {
    this.memberService.readmemberNoticeAll().subscribe({
      next: (response) => {
        console.log('response:', response);

        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }

        if (response.isSuccess) {
          // 直接使用 response.datas 而不是 response.data.datas
          if (Array.isArray(response.datas)) {
            this.messages = response.datas;

            // 格式化每个消息的 creTime
            this.messages.forEach(message => {
              if (typeof message.creTime === 'string') {
                message.creTime = this.formatDate(message.creTime);
              }
            });

            // 初始化 filteredMessages
            this.filteredMessages = [...this.messages];
          } else {
            console.error('返回的數據不是一個有效的陣列:', response.datas);
            this.messages = []; // 或者根据需要进行其他处理
            this.filteredMessages = []; // 更新 filteredMessages
          }
        }
      },
      error: (error) => {
        console.error('readMemberData error:', error);
        alert(error.error.message || 'An unexpected error occurred');
        this.router.navigateByUrl('login');
      },
    });
  }

  searchMessages(event: Event) {
    const searchTerm = (event.target as HTMLInputElement).value.toLowerCase(); // 獲取搜尋詞
    this.filteredMessages = this.messages.filter(message =>
      message.title.toLowerCase().includes(searchTerm) ||
      message.noticeContent.toLowerCase().includes(searchTerm)
    );
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  goHome() {
    this.router.navigateByUrl('login');
  }

  goMMain() {
    this.router.navigateByUrl('login/mmain');
  }
}
