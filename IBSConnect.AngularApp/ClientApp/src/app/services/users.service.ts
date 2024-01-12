import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { IUser } from '../models/models';

@Injectable({ providedIn: 'root' })
export class UserService {

  constructor(private http: HttpClient) {
  }

  getUsers() {
    return this.http.get<IUser[]>(`api/users`);
  }

  getUser(id: number) {
    return this.http.get<IUser[]>(`api/users/${id}`);
  }

  addUser(user: IUser) {
    return this.http.post(`api/users`, user);
  }

  updateUser(id: number, user: IUser) {
    return this.http.put(`api/users/${id}`, user);
  }

  deleteUser(id: number) {
    return this.http.delete(`api/users/${id}`);
  }

  changePassword(changePasswordRequest: { oldPassword: string, newPassword: string }) {
    return this.http.post(`api/users/changepassword`, changePasswordRequest);
  }

}
