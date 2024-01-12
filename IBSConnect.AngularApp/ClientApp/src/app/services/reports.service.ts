import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { IUser } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ReportsService {

  constructor(private http: HttpClient) {
  }

  getOutstandingBills() {
    return this.getBlobWithPost(`/api/reports/bills/outstanding`, {}).subscribe();
  }

  getUsageByCollege(range: {start: string, end: string}) {
    return this.getBlobWithPost(`/api/reports/usage/bycollege`, range).subscribe();
  }

  getUsageByUnitArea(range: { start: string, end: string }) {
    return this.getBlobWithPost(`/api/reports/usage/byunitarea`, range).subscribe();
  }

  getUsageByDemo(range: { start: string, end: string }) {
    return this.getBlobWithPost(`/api/reports/usage/bydemographics`, range).subscribe();
  }


  getUsageByDemoApps(range: { start: string, end: string }) {
    return this.getBlobWithPost(`/api/reports/usage/bydemographicapplications`, range).subscribe();
  }

  getBlobWithPost(url: string, body: any) {
    let subject = new Subject<any>();
    this.http.post(`${url}`, body, { observe: 'response', responseType: 'blob' }).subscribe((d) => {
      this.handleBlob(d);
      subject.complete();
    });
    return subject;
  }

  getBlob(url: string) {
    let subject = new Subject<any>();
    this.http.get(`${url}`, { observe: 'response', responseType: 'blob' }).subscribe((d) => {
      this.handleBlob(d);
      subject.complete();
    });
    return subject;
  }

  handleBlob(response: HttpResponse<Blob>) {
    let filename: string = this.getFileName(response);
    let binaryData = [];
    binaryData.push(response.body);
    let downloadLink = document.createElement('a');
    downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: 'blob' }));
    downloadLink.setAttribute('download', filename);
    document.body.appendChild(downloadLink);
    downloadLink.click();
    window.setTimeout(function () {
      document.body.removeChild(downloadLink);
      window.URL.revokeObjectURL(downloadLink.href);
    },
      0);
  }

  getFileName(response: HttpResponse<Blob>) {
    let filename: string;
    try {
      const contentDisposition: string = response.headers.get('content-disposition');
      const r = /filename="(.+?)"/;
      filename = r.exec(contentDisposition)[1];
    }
    catch (e) {
      filename = 'unknown';
    }
    return filename;
  }

}
