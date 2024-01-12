import * as moment from 'moment';

export function formatDate(value) {
  return moment(value).format('MMMM Do, YYYY');
}

export function formatMonth(value) {
  return moment().month(value).format("MMMM")
}


export function formatNA(value) {
  return value == 0 ? "N/A" : value;
}

export function formatNumber(value) {
  return value.toLocaleString();
}


export function formatSize(value) {
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
