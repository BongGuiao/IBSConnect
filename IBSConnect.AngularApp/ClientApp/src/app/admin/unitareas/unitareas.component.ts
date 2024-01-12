import { Component } from '@angular/core';
import { Item } from '../../models/models';
import { MetaDataService } from '../../services/metadata.service';
import { ToastrService } from 'ngx-toastr';
import { SimpleEditableComponent } from '../simpleEditable.component';
import { DialogService } from '../../services/dialog.service';

@Component({
  selector: 'app-unitareas',
  templateUrl: './unitareas.component.html',
  styleUrls: ['./unitareas.component.scss']
})
export class UnitAreasComponent extends SimpleEditableComponent {

  constructor(
    private dialogService: DialogService,
    private toastrService: ToastrService,
    private metadataService: MetaDataService) {
    super();
  }

  loadData() {
    this.metadataService.getUnitAreas().subscribe(d => {
      this.rows = d;
    });
  }

  saveRow(row: Item) {
    if (row.id && row.id > 0) {
      this.metadataService.updateUnitArea(row.id, row.name).subscribe(() => {
        this.toastrService.success("Row updated");
        this.loadData();
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
    else {
      this.metadataService.createUnitArea(row.name).subscribe((id) => {
        row.id = id;
        this.toastrService.success("Row saved");
        this.loadData();
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
  }

  deleteRow(row: Item) {
    this.dialogService.openYesNo({ title: "Delete Unit Area", message: `Are you sure you want to delete ${row.name}?` })
      .observable.subscribe(d => {
        if (d) {
          this.metadataService.deleteUnitArea(row.id).subscribe(() => {
            this.toastrService.success("Row deleted");
            this.loadData();
          }, err => {
            this.toastrService.error(err.error.message);
          });

        }
      });

  }
}