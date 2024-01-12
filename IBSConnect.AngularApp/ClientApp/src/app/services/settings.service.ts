import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { FilterRequest, IMember, IQueryResult, ISession, ISetting } from '../models/models';

@Injectable({ providedIn: 'root' })
export class SettingsService {

  constructor(private http: HttpClient) {
  }


  get() {
    return this.http.get<ISetting[]>(`api/settings`);
  }

  update(settings: ISetting[]) {
    return this.http.put(`api/settings`, settings);
  }

  resetHistroy(semester: string, userId:number) {
    const param = {id: 1, userId:userId,sy_Semester:semester}
    return this.http.post(`api/settings/resethistory`, param);
  }


}
