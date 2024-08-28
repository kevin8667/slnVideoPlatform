import { ForumPagingDTO } from './../interface/ForumPagingDTO';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ForumServiceService {

  constructor(private client: HttpClient) {

  }
  getArticleView(data: {}) {
    return this.client.post<ForumPagingDTO>('https://localhost:7193/api/Articles',data);
  }
}
