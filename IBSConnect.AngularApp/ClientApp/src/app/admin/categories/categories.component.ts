import { Component, ViewChild, TemplateRef } from '@angular/core';
import { Item, CategoryItem } from '../../models/models';
import { MetaDataService } from '../../services/metadata.service';
import { ToastrService } from 'ngx-toastr';
import { SimpleEditableComponent } from '../simpleEditable.component';
import { DialogService } from '../../services/dialog.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent extends SimpleEditableComponent {
  @ViewChild('freeTier')
  freeTierTemplate!: TemplateRef<any>;

  @ViewChild('actions')
  actionsTemplate!: TemplateRef<any>;

  constructor(
    private dialogService: DialogService,
    private toastrService: ToastrService,
    private metadataService: MetaDataService) {
    super();
  }

  init() {
    setTimeout(() => {
      this.source = {
        columns: [
          { templateRef: this.editableTemplate, text: "Name" },
          { templateRef: this.freeTierTemplate, text: "Free Tier", width: "90px", align: "center" },
          { templateRef: this.actionsTemplate, text: "", width: "60px", align: "center" }
        ]
      };
      this.loadData();
    }, 0);
  }

  loadData() {
    this.metadataService.getCategories().subscribe(d => {
      this.rows = d;
    });
  }

  saveRow(row: CategoryItem) {
    if (row.id && row.id > 0) {
      this.metadataService.updateCategory(row.id, row.name, row.isFreeTier).subscribe(() => {
        this.toastrService.success("Row updated");
        this.loadData();
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
    else {
      this.metadataService.createCategory(row.name, row.isFreeTier).subscribe((id) => {
        row.id = id;
        this.toastrService.success("Row saved");
        this.loadData();
      }, err => {
        this.toastrService.error(err.error.message);
      });
    }
  }

  deleteRow(row: CategoryItem) {
    this.dialogService.openYesNo({ title: "Delete Category", message: `Are you sure you want to delete ${row.name}?` })
      .observable.subscribe(d => {
        if (d) {
          this.metadataService.deleteCategory(row.id).subscribe(() => {
            this.toastrService.success("Row deleted");
            this.loadData();
          }, err => {
            this.toastrService.error(err.error.message);
          });
        }
      });

  }
}
