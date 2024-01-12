import { Pipe, PipeTransform } from '@angular/core';

// export interface Expression {
//     lhs: any,
//     op: '=' | '!=' | '<' | '>'
// }

@Pipe({
  name: 'fileSize',
})
export class FileSizePipe implements PipeTransform {
  transform(value: any): any {
    if (value > 1073741824) {
      return Math.round(value / 1073741824) + "GB";
    }
    if (value > 1048576) {
      return Math.round(value / 1048576) + "MB";
    }
    if (value > 1024) {
      return Math.round(value / 1024) + "KB";
    }
  }
}
