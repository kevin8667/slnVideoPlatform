export interface Chatroom {
  nickname: string;
  senderId: number;
  chatMessage: string;
  sendtime: string;
  isMined?:boolean;
}
