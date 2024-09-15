import { Component,OnInit } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css'],
  providers: [MemberService]
})
export class HistoryComponent  implements OnInit{


  error: boolean = false;
  items: any[] = [];
  home: any;

  constructor(private memberService: MemberService,private router: Router){
  }

  ngOnInit() {
  this.items = [
    { label: '會員首頁', url: 'login/mmain' },
    { label: '購物紀錄', url: 'login/friends' },
  ];

  this.home = { icon: 'pi pi-home', url: 'login' };
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
