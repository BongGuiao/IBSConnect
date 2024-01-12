import { Pipe, PipeTransform } from '@angular/core';

// export interface Expression {
//     lhs: any,
//     op: '=' | '!=' | '<' | '>'
// }

@Pipe({
  name: 'timeSpan',
})
export class TimeSpanPipe implements PipeTransform {
  transform(value: any, format?: string): any {
    let elapsedTime = "";

    //let days = Math.round(value / (60 * 24));

    //value = value % (60 * 24);


    let mins = value % 60;

    value = value - mins;

    let hours = Math.round(value / 60);

    let short = format == 'short' || format == 's';

    let hoursFormat = short ? "hrs" : "hours";
    let hourFormat = short ? "hr" : "hour";

    let minutesFormat = short ? "mins" : "minutes";
    let minuteFormat = short ? "min" : "minute";


    //if (days == 1) {
    //  elapsedTime += ` ${days} days`;
    //}
    //else if (days > 1) {
    //  elapsedTime += ` ${days} days`;
    //}

    if (hours == 1) {
      elapsedTime += ` ${hours} ${hourFormat}`;
    }
    else if (hours > 1) {
      elapsedTime += ` ${hours} ${hoursFormat}`;
    }

    if (mins == 1) {
      elapsedTime += ` ${mins} ${minuteFormat}`;
    }
    else if (mins > 1) {
      elapsedTime += ` ${mins} ${minutesFormat}`;
    }
    else {
      elapsedTime += ` ${mins} ${minutesFormat}`;
    }
    return elapsedTime.substring(1);
  }
}
