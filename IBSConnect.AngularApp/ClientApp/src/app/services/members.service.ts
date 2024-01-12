import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { FilterRequest, IMember, IQueryResult, ISession, ImportResult, IMemberBill, IPayment, IPaymentArrears, IIBSTranHistory } from '../models/models';

@Injectable({ providedIn: 'root' })
export class MembersService {

  totalTimeArrears = 0;
  arrearsRate = 0;
  constructor(private http: HttpClient) {
  }

  getMembers(filter: FilterRequest) {
    return this.http.post<IQueryResult<IMember>>(`api/members/search`, filter);
  }

  getMember(id: number) {
    return this.http.get<IMember>(`api/members/${id}`);
  }

  addMember(user: IMember) {
    return this.http.post(`api/members`, user);
  }

  updateMember(id: number, user: IMember) {
    return this.http.put(`api/members/${id}`, user);
  }

  deleteMember(id: number) {
    return this.http.delete(`api/members/${id}`);
  }

  changePassword(changePasswordRequest: { oldPassword: string, newPassword: string }) {
    return this.http.post(`api/members/changepassword`, changePasswordRequest);
  }

  getMemberSessions(id: number) {
    return this.http.get<ISession[]>(`api/members/${id}/sessions`);
  }

  getActiveSessions() {
    return this.http.get<ISession[]>(`api/members/sessions/active`);
  }

  closeActiveSessions() {
    return this.http.put(`api/members/sessions/active/close`, {});
  }

  updateMemberSession(id: number, session: ISession) {
    return this.http.put(`api/members/${id}/sessions`, session);
  }

  uploadMembers(formData: FormData) {
    return this.http.put<ImportResult>(`api/members/upload`, formData);
  }

  getMemberBill(id: number) {
    return this.http.get<IMemberBill>(`api/members/${id}/bill`);
  }

  getMembersBill(filter: FilterRequest) {
    return this.http.post<IQueryResult<IMemberBill>>(`api/members/billing`, filter);
  }

  creditMinutes(id: number, minutes: number) {
    return this.http.put<IQueryResult<IMemberBill>>(`api/members/${id}/credit`, { minutes });
  }

  getPayments(id: number) {
    return this.http.get<IPayment[]>(`api/members/${id}/payments`);
  }

  getPaymentArrears(id: number) {
    return this.http.get<IPaymentArrears[]>(`api/members/${id}/paymentarrears`);
  }
  addPayment(id: number, amount: number) {
    return this.http.post(`api/members/${id}/payments`, { amount });
  }
  addPaymentArrears(id: number, amount: number) {
    return this.http.post(`api/members/${id}/paymentarrears`, { amount });
  }
  getTotalArrears(id: number) {
    return this.http.get<IIBSTranHistory[]>(`api/members/${id}/totalarrears`);
  }

}
