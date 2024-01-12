import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { ICurrentSession, ISession } from '../models/models';


@Injectable({ providedIn: 'root' })
export class SessionsService {

  constructor(private http: HttpClient) {
  }

  start() {
    return this.http.put(`api/sessions/start`, {});
  }

  end() {
    return this.http.put(`api/sessions/end`, {});
  }

  getCurrent() {
    return this.http.get<ICurrentSession>(`api/sessions/current`, {});
  }

  getHistory() {
    return this.http.get<ISession[]>(`api/sessions`, {});
  }

}
