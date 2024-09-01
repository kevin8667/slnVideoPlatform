import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Video } from './interfaces/video';

@Injectable({
  providedIn: 'root',
})
export class SearchStateService {
  private readonly SEARCH_PARAMS_KEY = 'searchParams';
  private readonly SEARCH_RESULTS_KEY = 'searchResults';

  // 保存搜索参数到 sessionStorage
  saveSearchParams(params: any): void {
    sessionStorage.setItem(this.SEARCH_PARAMS_KEY, JSON.stringify(params));
  }

  // 从 sessionStorage 获取搜索参数
  getSearchParams(): any {
    const params = sessionStorage.getItem(this.SEARCH_PARAMS_KEY);
    return params ? JSON.parse(params) : null;
  }

  // 保存搜索结果到 sessionStorage
  saveSearchResults(results: any): void {
    sessionStorage.setItem(this.SEARCH_RESULTS_KEY, JSON.stringify(results));
  }

  // 从 sessionStorage 获取搜索结果
  getSearchResults(): any {
    const results = sessionStorage.getItem(this.SEARCH_RESULTS_KEY);
    return results ? JSON.parse(results) : null;
  }

  // 清除保存的搜索状态
  clearSearchState(): void {
    sessionStorage.removeItem(this.SEARCH_PARAMS_KEY);
    sessionStorage.removeItem(this.SEARCH_RESULTS_KEY);
  }
}