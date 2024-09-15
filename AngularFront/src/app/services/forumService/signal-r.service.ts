import { Chatroom } from './../../interfaces/forumInterface/Chatroom';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  private messagesSubject = new BehaviorSubject<Chatroom[]>([]);
  public messages$ = this.messagesSubject.asObservable();

  constructor(private client: HttpClient) {
    this.startConnection();
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7193/chathub') // 替換為您的 Hub URL
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');
        this.addReceiveMessageListener();
        this.getMessages();
      })
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  private addReceiveMessageListener() {
    this.hubConnection.on('ReceiveMessage', (chatroom: Chatroom) => {
      const currentMessages = this.messagesSubject.value;
      this.messagesSubject.next([chatroom, ...currentMessages]);
    });
  }

  public sendMessage(chatroom: Chatroom) {
    this.hubConnection
      .invoke('SendMessage', chatroom)
      .catch((err) => console.error('發送消息錯誤:', err));
  }

  public getMessages() {
    const api = 'https://localhost:7193/api/ChatRooms';
    this.client.get<Chatroom[]>(api).subscribe({
      next: (data) => {
        this.messagesSubject.next(data);
      },
      error: (err) => console.error('取得歷史訊息錯誤:', err),
    });
  }
}
