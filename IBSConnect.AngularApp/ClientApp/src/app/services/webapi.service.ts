import { HttpClient, HttpResponse } from '@angular/common/http';
import { Subject } from 'rxjs';

export class WebApiService {

  constructor(private http: HttpClient, protected rootUrl: string) {

  }

  getBlobWithPost(path: string, body: any) {
    let subject = new Subject<any>();
    this.http.post(`${this.rootUrl}/${path}`, body, { observe: 'response', responseType: 'blob' }).subscribe((d) => {
      this.handleBlob(d);
      subject.complete();
    });
    return subject;
  }

  getBlob(path: string) {
    let subject = new Subject<any>();
    this.http.get(`${this.rootUrl}/${path}`, { observe: 'response', responseType: 'blob' }).subscribe((d) => {
      this.handleBlob(d);
      subject.complete();
    });
    return subject;
  }

  post<T>(path: string, body: any) {
    return this.http.post<T>(`${this.rootUrl}/${path}`, body);
  }

  get<T>(path: string) {
    return this.http.get<T>(`${this.rootUrl}/${path}`);
  }

  put<T>(path: string, body: any) {
    return this.http.put<T>(`${this.rootUrl}/${path}`, body);
  }

  delete<T>(path: string) {
    return this.http.delete<T>(`${this.rootUrl}/${path}`);
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
    window.setTimeout(function() {
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
