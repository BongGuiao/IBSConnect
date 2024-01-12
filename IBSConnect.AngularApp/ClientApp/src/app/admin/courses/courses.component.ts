import { Component } from '@angular/core';
import { Item } from '../../models/models';
import { MetaDataService } from '../../services/metadata.service';
import { ToastrService } from 'ngx-toastr';
import { SimpleEditableComponent } from '../simpleEditable.component';
import { DialogService } from '../../services/dialog.service';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent extends SimpleEditableComponent {
  allRows: Item[]

  constructor(
    private dialogService: DialogService,
    private toastrService: ToastrService,
    private metadataService: MetaDataService) {
    super();
  }

  loadData() {
    this.metadataService.getCourses().subscribe(d => {
      this.allRows = d;
      this.query = "";
      this.setFilter(this.query);
    });
  }

  query: string;

  setFilter(query: string) {
    this.query = query;
    this.rows = this.allRows.filter(this.filter.bind(this));
  }

  filter(row: Item) {
    if (this.query && this.query.trim().length > 0) {
      return row.name.toLowerCase().indexOf(this.query.trim().toLowerCase()) >= 0;
    }
    return true;
  }

  saveRow(row: Item) {
    if (row.id && row.id > 0) {
      this.metadataService.updateCourse(row.id, row.name).subscribe(() => {
        this.toastrService.success("Row updated");
        this.loadData();
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
    else {
      this.metadataService.createCourse(row.name).subscribe((id) => {
        row.id = id;
        this.toastrService.success("Row saved");
        this.loadData();
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
  }

  deleteRow(row: Item) {
    this.dialogService.openYesNo({ title: "Delete Course", message: `Are you sure you want to delete ${row.name}?` })
      .observable.subscribe(d => {
        if (d) {

          this.metadataService.deleteCourse(row.id).subscribe(() => {
            this.toastrService.success("Row deleted");
            this.loadData();
          }, err => {
            this.toastrService.error(err.error.message);
          });
        }
      });
  }

}
