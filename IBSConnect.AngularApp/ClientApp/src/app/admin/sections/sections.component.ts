import { Component } from '@angular/core';
import { Item } from '../../models/models';
import { MetaDataService } from '../../services/metadata.service';
import { ToastrService } from 'ngx-toastr';
import { SimpleEditableComponent } from '../simpleEditable.component';

@Component({
  selector: 'app-sections',
  templateUrl: './sections.component.html',
  styleUrls: ['./sections.component.scss']
})
export class SectionsComponent extends SimpleEditableComponent {


  constructor(
    private toastrService: ToastrService,
    private metadataService: MetaDataService) {
    super();
  }

  loadData() {
    this.metadataService.getSections().subscribe(d => {
      this.rows = d;
    });
  }

  saveRow(row: Item) {
    if (row.id && row.id > 0) {
      this.metadataService.updateSection(row.id, row.name).subscribe(() => {
        this.toastrService.success("Row updated");
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
    else {
      this.metadataService.createSection(row.name).subscribe((id) => {
        row.id = id;
        this.toastrService.success("Row saved");
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
  }

  deleteRow(row: Item){
    this.metadataService.deleteSection(row.id).subscribe(() => {
      this.toastrService.success("Row deleted");
      this.loadData();
    }, err => {
      this.toastrService.error(err.error.message);
    });
  }
}
