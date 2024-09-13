import { Component } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-coupon',
  templateUrl: './coupon.component.html',
  styleUrls: ['./coupon.component.css'],
  providers: [MemberService],
})
export class CouponComponent {
  constructor(private memberService: MemberService, private router: Router) {}
  memberId: number = 1;
  couponData: any = [];
  giftList: any = [];
  hasData = false;
  visible: boolean = false;

  ngOnInit() {
    this.GetMemberCoupon();
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
        alert('GetMemberCoupon failed');
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
        alert('GetMemberCoupon failed');
      },
    });
  }
}
