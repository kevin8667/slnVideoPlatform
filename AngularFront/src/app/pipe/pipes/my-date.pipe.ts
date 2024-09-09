import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'forumDate',
})
export class forumDatePipe implements PipeTransform {
  transform(value:Date): string {
    value = new Date(value)
    return `${value.toLocaleDateString()} ${value.toTimeString().substring(0,5)}`
  }
}
