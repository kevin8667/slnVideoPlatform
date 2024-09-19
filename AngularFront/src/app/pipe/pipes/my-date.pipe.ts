import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'forumDate',
})
export class forumDatePipe implements PipeTransform {
  transform(value: Date): string {
    const updateTime = new Date(value);
    const now = new Date();

    // 手動加上 8 小時來調整到台灣時間
    updateTime.setHours(updateTime.getHours() + 8);

    // 檢查年份是否需要顯示
    const year = updateTime.getFullYear() !== now.getFullYear() ? `${updateTime.getFullYear()}-` : '';

    // 格式化月份、日期、時間部分
    const month = (updateTime.getMonth() + 1).toString().padStart(2, '0');
    const day = updateTime.getDate().toString().padStart(2, '0');
    const hour = updateTime.getHours().toString().padStart(2, '0');
    const minutes = updateTime.getMinutes().toString().padStart(2, '0');

    // 組合格式化字串
    return `${year}${month}-${day} ${hour}:${minutes}`;
  }

}
