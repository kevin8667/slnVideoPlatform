import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private apiUrl = 'https://localhost:7193/api/'; // API 端點
  constructor(private http: HttpClient) {}

  login(email: string, password: string, recaptchaResponse: string): Observable<any> {
    const body = { email, password, recaptchaResponse };
    return this.http.post(this.apiUrl + 'Login/AccountLogin', body);
  }

  forgetpwd(email: string): Observable<any> {
    return this.http.post(this.apiUrl + 'Login/Forgetpwd?email=' + email, {});
  }

  register(
    email: string,
    password: string,
    nickname: string,
    membername: string,
    phone: string,
    birth: Date,
    address: string,
    gender: string,
    uploadedFiles: any[]
  ): Observable<any> {
    const body = {
      email,
      password,
      nickname,
      membername,
      phone,
      birth,
      address,
      gender,
      uploadedFiles,
    };
    return this.http.post(this.apiUrl + 'Login/Register', body);
  }

  readmemberNotice(): Observable<any> {
    return this.http.get(this.apiUrl + 'Member/GetMemberNotice');
  }

  readmemberNoticeAll(): Observable<any> {
    return this.http.get(this.apiUrl + 'Member/GetMemberNoticeAll');
  }

  readmemberdata(): Observable<any> {
    return this.http.get(this.apiUrl + 'Member/GetMemberData');
  }

  updatememberdata(memberData: any): Observable<any> {
    return this.http.put(this.apiUrl + 'Member/PutMemberData', memberData);
  }

  updatememberPic( file: File ): Observable<any>{
    const formData = new FormData();
    formData.append('file', file); // 'file' 是後端預期的字段名

    return this.http.post(this.apiUrl + 'Member/PutMemberPic', formData);
    }

  GetMemberCoupon(): Observable<any> {
    return this.http.get(this.apiUrl + 'Coupon/GetCouponData');
  }

  GetGiftList(giftListId: string): Observable<any> {
    return this.http.get(this.apiUrl + 'Coupon/GetGiftList/' + giftListId);
  }

  GetFriendList(): Observable<any>{
    return this.http.get(this.apiUrl + 'Member/GetFriendList' );
  }
  GetMemberInfo(friendId:string): Observable<any>{
    debugger;
    return this.http.get(this.apiUrl + 'Member/GetMemberById/'+friendId );
  }

  InviteFriend(friendId:string,message:string): Observable<any>{
    debugger;
    return this.http.post(this.apiUrl + 'Member/InviteFriends?friendId='+friendId+'&message='+message ,{});
  }

  DeleteFriend(friendId:string ,action:string): Observable<any>{
    return this.http.delete(this.apiUrl + 'Member/DeleteFriend?friendId=' +friendId+'&action='+action );
  }

  addFriend(friendId:string): Observable<any>{
    return this.http.post(this.apiUrl + 'Member/AddFriend?friendId=' +friendId,{});
  }
}
