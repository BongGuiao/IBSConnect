import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { FilterRequest, IMember, IQueryResult, ISession } from '../models/models';

@Injectable({ providedIn: 'root' })
export class BillingService {

  constructor(private http: HttpClient) {
  }

  getCurrentBilling(filter: FilterRequest) {
    return this.http.post<IQueryResult<IMember>>(`api/billing/search`, filter);
  }

  getMemberBilling(id: number) {
    return this.http.get<IMember>(`api/billing/members/${id}`);
  }


  createBillingPeriod(name: string) {
    return this.http.post<IMember>(`api/billing`, { name });
  }

  updateBillingPeriod(id: number, name: string) {
    return this.http.post<IMember>(`api/billing/${id}`, { name });
  }
}
