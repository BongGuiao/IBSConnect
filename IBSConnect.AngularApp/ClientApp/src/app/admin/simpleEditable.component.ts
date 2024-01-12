import { Component, TemplateRef, ViewChild } from "@angular/core";
import { faPlus, faTrash } from "@fortawesome/free-solid-svg-icons";
import { ITableDef } from "../components/data-grid/models";
import { Item } from "../models/models";

@Component({
    template: ""
})
export class SimpleEditableComponent {
    faTrash = faTrash;
    faPlus = faPlus;

    source!: ITableDef<Item>;
    isEditing = false;
    rows!: Item[];

    @ViewChild('editable')
    editableTemplate!: TemplateRef<any>;

    constructor() {
      this.init();
    }

    init() {
      setTimeout(() => {
        this.source = {
          columns: [
            { templateRef: this.editableTemplate, text: "Name" }
          ]
        };
        this.loadData();
      }, 0);
    }

    loadData() {
    }

    saveRow(row: Item) {
    }

    deleteRow(row: Item) {
    }

    editing: Record<number, boolean> = {};
    oldValues: Record<number, string> = {};

    add() {
        let row = <Item>{};
        this.rows.push(row);
        this.edit(row);
    }

    edit(row: Item) {
        for (let row of this.rows) {
            if (this.editing[row.id]) {
                this.cancel(row);
            }
        }
        this.editing[row.id] = true;
        this.oldValues[row.id] = row.name;
        this.isEditing = true;
    }

    save(row: Item) {
        if (this.oldValues[row.id] && this.oldValues[row.id].trim() == row.name.trim()) {
            this.cancel(row);
            return;
        }
        if (row.name.length == 0) return;
        this.editing[row.id] = false;
        delete this.oldValues[row.id];
        this.saveRow(row);
        this.isEditing = false;
    }

    cancel(row: Item) {
        if (row.id) {
            this.editing[row.id] = false;
            row.name = this.oldValues[row.id];
        } else {
            this.delete(row);
        }
        this.isEditing = false;
    }

    delete(row: Item) {
        if (row.id) {
            this.deleteRow(row);
        }else{
            let index = this.rows.indexOf(row);
            this.rows.splice(index, 1);
        }
        delete this.editing[row.id];
        delete this.oldValues[row.id];
        //let index = this.rows.indexOf(row);
        //this.rows.splice(index, 1);
        this.isEditing = false;
    }

}
