import { Component, OnInit, OnDestroy } from '@angular/core';
import { ICurrentSession } from '../models/models';
import { SessionsService } from '../services/sessions.service';
import { formatDate } from '@angular/common';
import { DialogService } from '../services/dialog.service';
import { AuthenticationService } from "../services/authentication.service";
import { Router, NavigationEnd } from '@angular/router';
import { withLatestFrom } from 'rxjs/operators';


@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.scss']
})
export class SessionComponent implements OnInit, OnDestroy {

  session: ICurrentSession;
  picture: string;
  startTime: string;
  handle: number;

  constructor(
    private router: Router,
    private dailogService: DialogService,
    private sessionsService: SessionsService,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.sessionsService.getCurrent().subscribe(d => {
      this.session = d;

      this.startTime = formatDate(d.startTime, "MMM d, yyyy hh:mm:ss a", "en");

      if (this.session && this.session.picture) {
        this.picture = `/api/members/image?filename=${this.session.picture}`;
      } else {
        this.picture = "/assets/placeholder.png";
      }
    });

    this.handle = setInterval(<TimerHandler>(() => {
        this.refresh();
      }),
      60000);

    this.router.events.subscribe(
      event => {
        if (event instanceof NavigationEnd) {
          clearInterval(this.handle);
        }
      });
  }

  logout() {
    this.authenticationService.logout();
  }

  refresh() {
    this.sessionsService.getCurrent().subscribe(d => {
      this.session = d;
    });
  }

  endSession() {
    this.dailogService.openYesNo({
      title: "End Session",
      message: "Are you sure you want to end your session and log out of IBS Connect?"
    }).observable.subscribe(d => {
      if (d) {
        this.sessionsService.end().subscribe(() => {
          this.authenticationService.logout();
        });
      }
    });
  }

  ngOnDestroy(): void {
    clearInterval(this.handle);
  }

}
