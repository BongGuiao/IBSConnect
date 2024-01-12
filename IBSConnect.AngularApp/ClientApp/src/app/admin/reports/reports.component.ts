import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { faDownload } from '@fortawesome/free-solid-svg-icons';
import { ReportsService } from "../../services/reports.service";
import { DialogService } from "../../services/dialog.service";


interface DateRange {
  start: string;
  end: string;
}

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  faDownload = faDownload;

  dateRange: DateRange = <DateRange>{};

  constructor(
    private dialogService: DialogService,
    private reportsService: ReportsService
  ) { }

  ngOnInit(): void {
  }

  getOutstandingBills() {
    this.reportsService.getOutstandingBills();
  }

  getUsageByCollege() {
    this.showDateDialog().observable.subscribe(d => {
      if (d == "OK") {
        this.reportsService.getUsageByCollege(this.dateRange);
      }
    });
  }

  getUsageByUnitArea() {
    this.showDateDialog().observable.subscribe(d => {
      if (d == "OK") {
        this.reportsService.getUsageByUnitArea(this.dateRange);
      }
    });
  }

  getUsageByDemo() {
    this.showDateDialog().observable.subscribe(d => {
      if (d == "OK") {
        this.reportsService.getUsageByDemo(this.dateRange);
      }
    });
  }

  getUsageByDemoApps() {
    this.showDateDialog().observable.subscribe(d => {
      if (d == "OK") {
        this.reportsService.getUsageByDemoApps(this.dateRange);
      }
    });
  }

  @ViewChild('dateDialog')
  dateDialog!: TemplateRef<any>;

  showDateDialog() {
    return this.dialogService.openCustom({
      title: "Select Date Range",
      template: this.dateDialog,
      buttons: ["OK", "Cancel"],
      buttonConditions: {
        "OK": () => { return this.dateRange.start != null && this.dateRange.end != null }
      }
    });
  }
}
