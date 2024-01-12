import { Component } from '@angular/core';
import { Item } from '../../models/models';
import { MetaDataService } from '../../services/metadata.service';
import { ToastrService } from 'ngx-toastr';
import { SimpleEditableComponent } from '../simpleEditable.component';
import { DialogService } from "../../services/dialog.service";


@Component({
  selector: 'app-colleges',
  templateUrl: './colleges.component.html',
  styleUrls: ['./colleges.component.scss']
})
export class CollegesComponent extends SimpleEditableComponent {


  constructor(
    private dialogService: DialogService,
    private toastrService: ToastrService,
    private metadataService: MetaDataService) {
    super();
  }

  loadData() {
    this.metadataService.getColleges().subscribe(d => {
      this.rows = d;
    });
  }

  saveRow(row: Item) {
    if (row.id && row.id > 0) {
      this.metadataService.updateCollege(row.id, row.name).subscribe(() => {
        this.toastrService.success("Row updated");
        this.loadData();
      }, err => {
        this.loadData();
      });
    }
    else {
      this.metadataService.createCollege(row.name).subscribe((id) => {
        row.id = id;
        this.toastrService.success("Row saved");
        this.loadData();
      }, err => {
        this.loadData();
      });
    }
  }

  deleteRow(row: Item) {
    this.dialogService.openYesNo({ title: "Delete College", message: `Are you sure you want to delete ${row.name}?` })
      .observable.subscribe(d => {
        if (d) {
          this.metadataService.deleteCollege(row.id).subscribe(() => {
            this.toastrService.success("Row deleted");
            this.loadData();
          }, err => {
            this.loadData();
          });
        }
      });
  }
}
