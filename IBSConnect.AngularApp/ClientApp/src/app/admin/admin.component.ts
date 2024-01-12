import { Component, OnInit, ViewChild } from '@angular/core';
import { DialogDirective } from "../directives/dialog.directive";
import { DialogService } from "../services/dialog.service";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {


  constructor(private dialogService: DialogService) {
  }

  ngOnInit(): void {

  }

}
