import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { RecaptchaModule } from 'ng-recaptcha';
// import { LuckyWheel, LuckyGrid } from 'lucky-canvas'

import { InputTextModule } from 'primeng/inputtext';
import { FileUploadModule } from 'primeng/fileupload';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CarouselModule } from 'primeng/carousel';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { CheckboxModule } from 'primeng/checkbox';
import { MessagesModule } from 'primeng/messages';

import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { MmainComponent } from './mmain/mmain.component';
import { CouponComponent } from './coupon/coupon.component';
import { MessageComponent } from './message/message.component';
import { FriendsComponent } from './friends/friends.component';
import { HistoryComponent } from './history/history.component';
// import { PassComponent } from './pass/pass.component';
// import { LoginCallbackComponent } from './login-callback/login-callback.component';


const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'coupon', component: CouponComponent },
  { path: 'friends', component: FriendsComponent },
  { path: 'mmain', component: MmainComponent },
  { path: 'message', component: MessageComponent },
  { path: 'history', component: HistoryComponent },
  // { path: 'crazygrid', component: CrazygridComponent },
  { path: '**', component: MmainComponent },
];

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    MmainComponent,
    CouponComponent,
    MessageComponent,
    FriendsComponent,
    HistoryComponent,


    // PassComponent,

  
    // LoginCallbackComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    RecaptchaModule,
    InputTextModule,
    FileUploadModule,
    TableModule,
    DialogModule,
    ButtonModule,
    CardModule,
    CarouselModule,
    RadioButtonModule,
    InputTextareaModule,
    BreadcrumbModule,
    CheckboxModule,
    MessagesModule
  ],
  exports: [RouterModule],

})
export class MemberModule { }
