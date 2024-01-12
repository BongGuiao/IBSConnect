import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { DatabaseService, BackupFile } from "../../services/database.service";
import { ToastrService } from 'ngx-toastr';
import { ITableDef } from "../../components/data-grid/models";
import { DialogService } from "../../services/dialog.service";
import { formatDate, formatSize } from "../../common/formatters";

@Component({
  selector: 'app-database',
  templateUrl: './database.component.html',
  styleUrls: ['./database.component.scss']
})
export class DatabaseComponent implements OnInit {
  source: ITableDef<BackupFile>;
  rows: BackupFile[];

  @ViewChild('action')
  actionTemplate: TemplateRef<any>;

  @ViewChild('created')
  createdTemplate: TemplateRef<any>;

  @ViewChild('size')
  sizeTemplate: TemplateRef<any>;

  @ViewChild('confirm')
  confirmTemplate: TemplateRef<any>;

  backupNow: boolean = true;

  constructor(
    private dialogService: DialogService,
    private toastrService: ToastrService,
    private databaseService: DatabaseService) {
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.source = {
        columns: [
          { name: 'fileName', text: "File" },
          { templateRef: this.createdTemplate, text: "Creaated", width: "120px" },
          { templateRef: this.sizeTemplate, text: "Size", width: "120px" },
          { templateRef: this.actionTemplate, text: "", width: "70px" }
        ]
      };

      this.loadData();
    });

  }

  loadData() {
    this.databaseService.get().subscribe(d => {
      this.rows = d;
    });
  }

  confirmText: string;

  restore(row: BackupFile) {
    this.dialogService.openCustom({
      title: "Restore Database",
      template: this.confirmTemplate,
      buttons: ["Restore", "Cancel"],
      buttonConditions: {
        "Restore": () => this.confirmText && this.confirmText == "confirm"
      }
    }).observable.subscribe((d) => {
      if (d == "Restore") {
        this.databaseService.restore(row.fileName).subscribe(d => {
          this.toastrService.success("Database restored");
          this.loadData();
        });
      }
    });
  }

  backup() {
    this.databaseService.backup().subscribe(d => {
      this.toastrService.success("Database backup created");
      this.loadData();
    });
  }
}
