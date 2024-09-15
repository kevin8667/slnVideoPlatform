import { Component,OnInit} from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-coupon',
  templateUrl: './coupon.component.html',
  styleUrls: ['./coupon.component.css'],
  providers: [MemberService],
})
export class CouponComponent implements OnInit{
  constructor(private memberService: MemberService, private router: Router) {}
  memberId: number = 1;
  couponData: any = [];
  giftList: any = [];
  hasData = false;
  visible: boolean = false;
  items: any[] = [];
  home: any;

  ngOnInit() {
    this.GetMemberCoupon();
    this.items = [
      { label: '會員首頁', url: 'http://localhost:4200/#/login/mmain' },
      { label: '我的優惠券', url: 'http://localhost:4200/#/login/coupon' },
    ];
    this.home = { icon: 'pi pi-home', url: 'login' };
  }

  GetMemberCoupon() {
  
    this.memberService.GetMemberCoupon().subscribe({
      next: (response) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
        if (response.isSuccess) {
          this.couponData = response.datas;
          this.hasData = true;
        }
      },
      error: (error) => {
        console.error('GetMemberCoupon error:', error);
        alert('取不到會員優惠券');
        this.hasData = false; 
      },
    });
  }

  goHome() {
    this.router.navigateByUrl('login');
  }

  goMMain() {
    this.router.navigateByUrl('login/mmain');
  }

  ShowGiftList(giftlistid: number) {
    debugger;
    this.visible = true;
    this.memberService.GetGiftList(giftlistid).subscribe({
      next: (response) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
        if (response.isSuccess) {
          this.giftList = response.datas;
        }
      },
      error: (error) => {
        console.error('GetMemberCoupon error:', error);
        alert('會員優惠券有誤');
      },
    });
  }
}
