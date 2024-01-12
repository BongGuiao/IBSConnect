import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { IAuthenticatedUser } from '../models/models';
import { AuthenticationService } from '../services/authentication.service';
import { DialogService } from "../services/dialog.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isExpanded = false;

  currentUser: IAuthenticatedUser;

  constructor(
    private dialogService: DialogService,
    private router: Router,
    private route: ActivatedRoute,
        private authenticationService: AuthenticationService
  ) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  get isLoggedIn() {
    return this.currentUser != null;
  }

  get isMember() {
    return this.currentUser.role == 'Member';
  }

  get isAdmin() {
    return this.currentUser.role == 'Administrator';
  }

  logout() {
    if (this.isMember) {
      let dialog = {
        title: "Log out",
        message: "Logging out will not end your session. Your time will continue to run. Make sure to click End Session to properly stop your time and log out of the system completely. Are you sure you want to log out?"
      }
      this.dialogService.openYesNo(dialog).observable.subscribe(d => {
        if (d) {
          this.authenticationService.logout();
          this.router.navigate(['/']);
        }
      });
    } else {
      this.authenticationService.logout();
      this.router.navigate(['/']);
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
