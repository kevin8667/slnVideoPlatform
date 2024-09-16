import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'forumDate',
})
export class forumDatePipe implements PipeTransform {
  transform(value: Date | string, type?: number): string {
    if (type !== 1) {
      const updateTime = new Date(value);
      const now = new Date();

      // 手動加上 8 小時來調整到台灣時間
      updateTime.setHours(updateTime.getHours() + 8);

      // 檢查年份是否需要顯示
      const year =
        updateTime.getFullYear() !== now.getFullYear()
          ? `${updateTime.getFullYear()}-`
          : '';

      // 格式化月份、日期、時間部分
      const month = (updateTime.getMonth() + 1).toString().padStart(2, '0');
      const day = updateTime.getDate().toString().padStart(2, '0');
      const hour = updateTime.getHours().toString().padStart(2, '0');
      const minutes = updateTime.getMinutes().toString().padStart(2, '0');

      // 組合格式化字串
      return `${year}${month}-${day} ${hour}:${minutes}`;
    } else {
      // 確保 value 是有效的日期字串，將字串轉換為 Date 物件
      const date = new Date(value);

      // 使用 JavaScript 的內建方法來格式化時間
      return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false });

    }
  }
}
