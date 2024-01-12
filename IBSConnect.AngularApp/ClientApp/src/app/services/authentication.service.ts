import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { IUser, IAuthenticatedUser } from '../models/models';
import { parseJwt } from "../common/jwt";

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<IAuthenticatedUser>;
  public currentUser: Observable<IAuthenticatedUser>;

  constructor(private router: Router,
    private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<IAuthenticatedUser>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): IAuthenticatedUser {
    return this.currentUserSubject.value;
  }

  login(username: string, password: string) {
    
    return this.http.post<IAuthenticatedUser>(`api/users/authenticate`, { username, password })
      .pipe(map(user => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        let parsedUser = parseJwt(user.token);
        parsedUser.token = user.token;
        localStorage.setItem('currentUser', JSON.stringify(parsedUser));
        this.currentUserSubject.next(parsedUser);
        return user;
      }));
  }

  loginMember(request: {idNo: string, password: string; applications: number[]}) {
    return this.http.post<IAuthenticatedUser>(`api/members/authenticate`, request)
      .pipe(map(user => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        let parsedUser = parseJwt(user.token);
        parsedUser.token = user.token;
        localStorage.setItem('currentUser', JSON.stringify(parsedUser));
        this.currentUserSubject.next(parsedUser);
        return user;
      }));
  }

  resetUser() {
    this.currentUserSubject.value.passExpired = false;
    this.currentUserSubject.value.passExpiring = false;
    this.currentUserSubject.value.daysLeft = 0;
  }

  logout(queryParams?: { returnUrl?: string, reason?: string }) {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/'], { queryParams: queryParams });
  }


  logoutUser() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  forgotPassword(emailAddress: string) {
    return this.http.post(`api/users/forgotpassword`, { emailAddress });
  }

  resetPassword(token: string, password: string) {
    return this.http.post(`api/users/resetpassword`, { token, password });
  }

}
