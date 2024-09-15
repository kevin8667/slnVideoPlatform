import { Component, OnInit } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css'],
  providers: [MemberService],
})
export class MessageComponent implements OnInit {
  messages: any[] = [];
  items: any[] = [];
  home: any;
  checked: boolean[] = [];
  checkedAll: boolean = false;
  checkedAllTxt: string = '全選';
  searchTerm: string = '';
  filteredMessages: any[] = [];

  constructor(private memberService: MemberService, private router: Router) { }

  ngOnInit() {
    this.readmessages();
    this.items = [
      { label: '會員首頁', url: 'http://localhost:4200/login#/login/mmain' },
      { label: '我的通知', url: 'http://localhost:4200/login#/login/message' },
    ];
  onCheckedAllChange(event: boolean) {
    this.checkedAll = event;
    this.checkedAllTxt = this.checkedAll ? "取消全選" : "全選";
    
    // 根據 checkedAll 的狀態設置 checked 陣列
    this.checked = this.checked.map(() => this.checkedAll);
  }

  toggleCheckbox(index: number) {
    // 切換特定行的複選框狀態
    this.checked[index] = !this.checked[index];
    this.updateSelectAllCheckbox();
  }

  onCheckedAllChange(event: boolean) {
    this.checkedAll = event;
    this.checkedAllTxt = this.checkedAll ? "取消全選" : "全選";

    // 根據 checkedAll 的狀態設置 checked 陣列
    this.checked = this.checked.map(() => this.checkedAll);
  }

  updateSelectAllCheckbox() {
    this.checkedAll = this.checked.every(checked => checked);
    this.checkedAllTxt = this.checkedAll ? "取消全選" : "全選";
  }

  readmessages() {
    this.memberService.readmemberNoticeAll().subscribe({
      next: (response) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }

        if (response.isSuccess) {
          if (Array.isArray(response.datas)) {
            this.messages = response.datas;

            // 格式化每个消息的 creTime
            this.messages.forEach(message => {
              if (typeof message.creTime === 'string') {
                message.creTime = this.formatDate(message.creTime);
              }
            });

            // 初始化 filteredMessages 和 checked
            this.filteredMessages = [...this.messages];
            this.checked = new Array(this.filteredMessages.length).fill(false);
          } else {
            console.error('返回的數據不是一個有效的陣列:', response.datas);
            this.messages = [];
            this.filteredMessages = [];
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

  searchMessages() {
    const searchTerm = this.searchTerm.toLowerCase();

    // 檢查 searchTerm 是否為 null 或者是否只包含空白字符
    if (!searchTerm || searchTerm.trim() === '') {
      this.filteredMessages = this.messages
    } else {

      // 根據 searchTerm 過濾消息
      this.filteredMessages = this.messages.filter(message =>
        message.title.toLowerCase().includes(searchTerm) ||
        message.noticeContent.toLowerCase().includes(searchTerm)
      );
    }

    // 更新 checked 陣列的長度
    this.checked = new Array(this.filteredMessages.length).fill(false);
  }

  deleteMessages() {
    const selectedMessages = this.filteredMessages.filter((_, index) => this.checked[index]);

    if (selectedMessages.length === 0) {
      alert('請選擇要刪除的訊息');
      return;
    }

    const confirmDelete = confirm('確定要刪除訊息嗎');
    if (!confirmDelete) {
      return;
    }

    const deleteRequests = selectedMessages.map(message => {
      console.log(`嘗試刪除訊息 ID: ${message.memberNoticeID}`);
      return this.memberService.DeleteMemberNotice(message.memberNoticeID);
    });

    forkJoin(deleteRequests).subscribe({
      next: responses => {
        alert('成功刪除訊息'); // 提供成功反馈
        this.readmessages(); // 刷新消息列表
      },
      error: error => {
        console.error('刪除訊息時發生錯誤:', error);
        alert('刪除訊息時發生錯誤，請稍後再試');
      }
    });
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