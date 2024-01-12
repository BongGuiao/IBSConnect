import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { FilterRequest, IMember, IQueryResult, ISession } from '../models/models';



export interface BackupFile {
  fileName: string;
  dateCreated: string;
  size: number;
}


@Injectable({ providedIn: 'root' })
export class DatabaseService {

  constructor(private http: HttpClient) {
  }

  backup() {
    return this.http.get(`api/database/backup`);
  }

  restore(filename: string) {
    return this.http.get(`api/database/restore?filename=${filename}`);
  }

  get() {
    return this.http.get<BackupFile[]>(`api/database`);
  }

}

