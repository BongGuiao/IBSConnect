import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { IAuthenticatedUser } from '../models/models';

@Component({
  selector: 'side-nav-menu',
  templateUrl: './side-nav-menu.component.html',
  styleUrls: ['./side-nav-menu.component.scss']
})
export class SideNavMenuComponent implements OnInit {
  currentUser: IAuthenticatedUser;

  constructor(private router: Router,
    private authenticationService: AuthenticationService) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  ngOnInit(): void {
  }


  get isLoggedIn() {
    return this.currentUser != null;
  }


  get isAdmin() {
    return this.currentUser.role == 'Administrator';
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

}
