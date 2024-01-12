import { Component } from '@angular/core';
import { Item } from '../../models/models';
import { MetaDataService } from '../../services/metadata.service';
import { ToastrService } from 'ngx-toastr';
import { SimpleEditableComponent } from '../simpleEditable.component';
import { DialogService } from '../../services/dialog.service';

@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.scss']
})
export class ApplicationsComponent extends SimpleEditableComponent {

  constructor(
    private dialogService: DialogService,
    private toastrService: ToastrService,
    private metadataService: MetaDataService) {
    super();
  }

  loadData() {
    this.metadataService.getApplications().subscribe(d => {
      this.rows = d;
    });
  }

  saveRow(row: Item) {
    if (row.id && row.id > 0) {
      this.metadataService.updateApplication(row.id, row.name).subscribe(() => {
        this.toastrService.success("Row updated");
        this.loadData();
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
    else {
      this.metadataService.createApplication(row.name).subscribe((id) => {
        row.id = id;
        this.toastrService.success("Row saved");
        this.loadData();
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
  }

  deleteRow(row: Item) {
    this.dialogService.openYesNo({ title: "Delete Application", message: `Are you sure you want to delete ${row.name}?` })
      .observable.subscribe(d => {
        if (d) {

          this.metadataService.deleteApplication(row.id).subscribe(() => {
            this.toastrService.success("Row deleted");
            this.loadData();
          }, err => {
            this.toastrService.error(err.error.message);
          });
        }
      });
  }

}
