import { Component, OnInit } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';

interface Friend {
  friendId: string;
  nickName: string;
  invitedMessage: string;
  creationDate: string;
  friendStatus: string;
  memberName: string;
  photoPath: string;
  action: string;
}

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css'],
  providers: [MemberService],
})
export class FriendsComponent implements OnInit {
  friends: Friend[] = [];
  action:string='';
  invitingFriends:Friend[]=[];
  pendingFriends:Friend[]=[];

  responsiveOptions: any[] | undefined;
  isLoading: boolean = true;
  visible: boolean = false;
  friendId: string = '';
  loadFriend: boolean = false;
  fetchFriendData: any = {};
  sendMsg:string='';
  DeleteFriend: string ='';


  constructor(private memberService: MemberService, private router: Router) {}

  ngOnInit() {
    this.loadFriends();
    this.responsiveOptions = [
      {
        breakpoint: '1199px',
        numVisible: 1,
        numScroll: 1,
      },
      {
        breakpoint: '991px',
        numVisible: 2,
        numScroll: 1,
      },
      {
        breakpoint: '767px',
        numVisible: 1,
        numScroll: 1,
      },
    ];
  }

  loadFriends() {
    this.isLoading = true;
    this.memberService.GetFriendList().subscribe({
      next: (response) => {
        debugger;
        if (Array.isArray(response.datas)) {
          this.friends = response.datas.map((data: any) => ({
            friendId: data.friendId,
            nickName: data.nickName,
            invitedMessage: data.invitedMessage,
            creationDate: data.creationDate,
            friendStatus: data.friendStatus,
            memberName: data.memberName,
            photoPath: data.photoPath,
          })).filter((friend: any) => friend.friendStatus === '已接受');

          this.invitingFriends = response.datas.map((data: any) => ({
            friendId: data.friendId,
            nickName: data.nickName,
            invitedMessage: data.invitedMessage,
            creationDate: data.creationDate,
            friendStatus: data.friendStatus,
            memberName: data.memberName,
            photoPath: data.photoPath,
          })).filter((friend: any) => friend.friendStatus === '邀請中');
          this.pendingFriends = response.datas.map((data: any) => ({
            friendId: data.friendId,
            nickName: data.nickName,
            invitedMessage: data.invitedMessage,
            creationDate: data.creationDate,
            friendStatus: data.friendStatus,
            memberName: data.memberName,
            photoPath: data.photoPath,
          })).filter((friend: any) => friend.friendStatus === '待回覆');
        }

        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
      },
      error: (error) => {
        console.error('loadFriends error:', error);
        alert('读取好友信息失败');
      },
      complete: () => {
        this.isLoading = false;
      },
    });
  }

  goHome() {
    this.router.navigateByUrl('login');
  }

  goMMain() {
    this.router.navigateByUrl('login/mmain');
  }

  addFriend(friendId:string) {
    this.memberService.addFriend(friendId).subscribe({
      next: (response) => {
        if (response.isSuccess) {
        }
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }

        this.loadFriends();
      },

      error: (error) => {
        console.error('addFriend error:', error);
        alert('addFriend error');
      },
    });
  }

  removeFriend(friendId: number) {
    debugger;
    this.memberService.DeleteFriend(friendId).subscribe({
      next: (response) => {
        if (response.isSuccess) {
        }
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }

        this.loadFriends();
      },

      error: (error) => {
        console.error('removeFriends error:', error);
        alert('removeFriends error');
      },
    });
}

  AddFriendClick() {
    this.visible = true;
  }

  fetchFriendDetails() {
    this.memberService.GetMemberInfo(this.friendId).subscribe({
      next: (response) => {
        debugger;
        if (response.isSuccess) {
          this.loadFriend=true;
          this.fetchFriendData = response.data;
        }

        if (response.hasAlertMsg) {
          this.loadFriend=false;

          alert(response.alertMsg);
        }
      },
      error: (error) => {
        console.error('loadFriends error:', error);
        alert('loadFriends error');
      },
      complete: () => {
        this.isLoading = false;
      },
    });
  }
  sendInvitation(){
    if(!this.sendMsg || this.sendMsg.trim() === ''){
      alert('請輸入邀請訊息。');
      return;
    }

    this.memberService.InviteFriend(this.friendId,this.sendMsg).subscribe({
      next: (response) => {
        debugger;
        if (response.isSuccess) {
          this.visible=false;
          this.loadFriends();
        }

        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
      },
      error: (error) => {
        if(!this.sendMsg || this.sendMsg.trim() === ''){
          alert('請輸入邀請訊息。')
        }
        console.error('loadFriends error:', error);
        alert('sendInvitation error');
      },
      complete: () => {
        this.isLoading = false;
      },
    });
  }
  hideDialog(){
    this.visible = false;

  }
}
