import { Component,OnInit } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css'],
  providers: [MemberService]
})
export class HistoryComponent  {

  memberId: number | null = null;
  error: boolean = false;

  constructor(private memberService: MemberService,private router: Router){
  }






  points='5';
  quantity='10';
  productSpecs='大人炫炮紫';
  productName='酷冰圈';
  productImage='';
  orderNumber='100';
  orderDate='2024/09/03';

  goHome() {
    this.router.navigateByUrl('login');
  }

  goMMain() {
    this.router.navigateByUrl('login/mmain');
  }

  contactCustomerService(){}

  buyAgain(){}



}
