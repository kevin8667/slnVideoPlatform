export interface PagedResult<T> {
  items: T[];
  totalResults: number;
  pageNumber: number;
  pageSize: number;
}
