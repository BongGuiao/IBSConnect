import { Pipe, PipeTransform } from '@angular/core';

// export interface Expression {
//     lhs: any,
//     op: '=' | '!=' | '<' | '>'
// }

@Pipe({
  name: 'filter',
})
export class FilterPipe implements PipeTransform {
  transform(items: any[], filter: (item: any) => boolean, context: any): any {
    if (!items || !filter) {
      return items;
    }

    return items.filter(filter.bind(context));
  }
}

