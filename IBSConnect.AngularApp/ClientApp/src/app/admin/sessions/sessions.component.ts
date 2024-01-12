import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ISession } from 'src/app/models/models';
import { DialogService } from 'src/app/services/dialog.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-sessions',
  templateUrl: './sessions.component.html',
  styleUrls: ['./sessions.component.scss']
})
export class SessionsComponent implements OnInit {

  sessions!: ISession[];

  constructor(
    private toastrService: ToastrService,
    private membersService: MembersService,
    private dialogService: DialogService) {

  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.membersService.getActiveSessions()
      .subscribe(d => {
        this.sessions = d;
      });
  }

  closeActiveSessions() {
    this.dialogService.openYesNo({ title: "Close Sessions", message: "Are you sure you want to close the over-cutoff sessions?" }).observable.subscribe(d => {
      if (d) {
        this.membersService.closeActiveSessions()
          .subscribe(d => {
            this.toastrService.success("Over-cutoff sessions closed");
            this.loadData();
          });
      }
    });
  }
}
