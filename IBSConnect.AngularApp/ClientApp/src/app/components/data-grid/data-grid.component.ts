import { Component, OnInit, Input, Output, EventEmitter, ViewContainerRef, Host, HostListener, SimpleChanges, NgZone } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { formatDate, formatNumber, formatNA } from "../../common/formatters";
import { ITableDef, IColumnSort } from "./models";


@Component({
  selector: 'data-grid',
  templateUrl: './data-grid.component.html',
  styleUrls: ['./data-grid.component.scss']
})
export class DataGridComponent implements OnInit {
  formatDate = formatDate;
  formatNumber = formatNumber;
  formatNA = formatNA;
  Math = Math;

  sort: { [key: string]: 'asc' | 'desc' } = {};


  //@HostListener('document:keydown', ['$event'])
  //keydown(event: KeyboardEvent) {
  //  if (this.hasFocus) {
  //    if (this.selectedRow) {
  //      let index = this.rows.indexOf(this.selectedRow);
  //      if (event.key === 'ArrowUp') {
  //        if (index > 0) {
  //          this.selectedRow = this.rows[index - 1];
  //          this.debouncer.next(this.selectedRow);
  //          event.preventDefault();
  //        }
  //      } else if (event.key === 'ArrowDown') {
  //        if (index < this.rows.length - 1) {
  //          this.selectedRow = this.rows[index + 1];
  //          this.debouncer.next(this.selectedRow);
  //          event.preventDefault();
  //        }
  //      } else if (event.key === 'Enter') {
  //        this.debouncer.next(this.selectedRow);
  //        event.preventDefault();
  //      }

  //    }
  //  }

  //}

  debouncer = new Subject<any>();
  startX: number;
  startY: number;
  currentX: number;
  currentY: number;
  isSelecting: boolean;

  //@HostListener('document:mousedown', ['$event'])
  //mousedown(event: MouseEvent) {
  //  this.isSelecting = false;
  //  let rect = (<HTMLElement>this.viewRef.element.nativeElement).getBoundingClientRect();
  //  this.startX = event.clientX;
  //  this.startY = event.clientY;
  //  this.hasFocus = event.clientX >= rect.left && event.clientX <= rect.right && event.clientY >= rect.top && event.clientY <= rect.bottom;
  //}

  //@HostListener('document:mousemove', ['$event'])
  //mousemove(event: MouseEvent) {
  //  if (event.buttons == 1 && this.hasFocus) {
  //    this.currentX = event.clientX;
  //    this.currentY = event.clientY;
  //    let dist = ((this.currentX - this.startX) * (this.currentX - this.startX)) + ((this.currentY - this.startY) * (this.currentY - this.startY));

  //    if (dist > 20) {
  //      this.isSelecting = true;
  //      this.selectedRow = null;
  //      if (this.selectedItems) {
  //        this.selectedItems.splice(0);
  //      }
  //    }
  //  }
  //}

  // @HostListener('document:mouseup', ['$event'])
  // mouseup(event: MouseEvent) {
  //   if (this.hasFocus) {

  //   }
  // }

  hasFocus: boolean = false;

  @Input()
  selectedRow: any;

  lastSort: string;

  @Input()
  filterContext: any;

  @Input()
  filter: (row: any) => boolean;

  @Input()
  filterIf: boolean;

  @Input()
  source!: ITableDef<any>;

  @Input()
  rows!: any[];

  @Input()
  total!: number;

  @Input()
  sortable: boolean;

  @Input()
  sortMethod: string;

  @Input()
  rowSelectable: boolean;

  @Input()
  multiSelect: boolean;

  @Input()
  selectedItems: any[];

  pageSize = 50;

  @Output()
  onSortChange = new EventEmitter<IColumnSort>();

  @Output()
  onPageChange = new EventEmitter<number>();

  @Output()
  onRowSelect = new EventEmitter<any>();

  currentPage: number = 1;

  get noResultsText() {
    return this.source.emptyRowsMessage ? this.source.emptyRowsMessage : "NO RESULTS";
  }



  selectRow(row: any, event: MouseEvent) {
    if (this.isSelecting) return;
    if (this.rowSelectable) {
      if ((event.shiftKey || event.ctrlKey) && this.selectedItems && this.multiSelect) {
        if (event.shiftKey) {
          if (this.selectedRow) {
            let lastIndex = this.rows.indexOf(this.selectedRow);
            let thisIndex = this.rows.indexOf(row);
            for (let i = lastIndex; i !== thisIndex; i += Math.sign(thisIndex - lastIndex)) {
              if (this.selectedItems.indexOf(this.rows[i]) === -1) {
                this.selectedItems.push(this.rows[i]);
              }
            }
            this.selectedItems.push(this.rows[thisIndex]);
          }
        }
        else if (event.ctrlKey) {
          if (this.selectedRow && this.selectedItems.indexOf(this.selectedRow) === -1) {
            this.selectedItems.push(this.selectedRow);
          }
          let index = this.selectedItems.indexOf(row);
          if (index === -1) {
            this.selectedItems.push(row);
          } else {
            this.selectedItems.splice(index, 1);
          }
        }
      } else {
        if (this.selectedItems) {
          this.selectedItems.splice(0, this.selectedItems.length);
        }

        this.selectedRow = row;

        if (this.onRowSelect) {
          this.onRowSelect.emit(row);
        }
      }

    }
  }

  //@Host() private parent: Component
  constructor(private zone: NgZone, private readonly viewRef: ViewContainerRef) {
    this.debouncer
      .pipe(debounceTime(100))
      .subscribe((value) => this.onRowSelect.emit(value));
  }

  ngOnInit() {
    //this.loadData();
  }


  format(column, field) {
    if (column.formatter) {
      return column.formatter(field);
    }
    else if (column.type) {
      switch (column.type) {
        case "number":
          return formatNumber(field);
        case "date":
          return formatDate(field);
        default:
          return field;
      }
    }
    return field;
  }

  rowClass: any[] = [];

  getRowClass(row) {
    let rowClass = {
      'cursor-pointer': this.rowSelectable && !this.isSelecting,
      'row-hover': this.rowSelectable,
      'selected-row': (this.rowSelectable && row === this.selectedRow) ||
        (this.multiSelect && this.selectedItems && this.selectedItems.indexOf(row) > -1)
    };

    if (this.source.rowClass) {
      var customClass = {};

      let className = this.source.rowClass(row);

      customClass[className] = true;

      rowClass = { ...rowClass, ...customClass };
    }

    return rowClass;
  }

  lastPage = 0;

  pageChanged(event) {
    if (event.page != this.currentPage) {
      this.currentPage = event.page;
      this.onPageChange.emit(this.currentPage);
    }
  }


  filteredRows: any[];


  ngOnChanges(changes: SimpleChanges) {
    let rows = this.loadData(this.rows);

    if (this.source) {
      if (this.source.columns) {
        for (let i = 0; i < this.source.columns.length; i++) {
          this.columnClass[i] = this.getColumnClass(this.source.columns[i]);
          this.headerColumnClass[i] = this.getHeaderColumnClass(this.source.columns[i]);
        }
      }
    }

    if (rows) {
      for (let i = 0; i < rows.length; i++) {
        this.rowClass[i] = this.getRowClass(rows[i]);
      }
    }

    this.filteredRows = rows;
  }

  loadData(rows: any[]) {

    if (rows) {
      if (this.sortMethod == 'external' || this.lastSort == null) {

      } else {
        rows = rows.sort((a, b) => {
          var keyA = a[this.lastSort],
            keyB = b[this.lastSort];
          if (this.sort[this.lastSort] == 'desc') {
            if (keyA < keyB) return 1;
            if (keyA > keyB) return -1;
          } else {
            if (keyA < keyB) return -1;
            if (keyA > keyB) return 1;
          }
          return 0;
        });

      }


      if (this.filterIf && this.filter && this.filterContext) {
        rows = rows.filter(row => this.filter.apply(this.filterContext, [row]));
      }


    }

    return rows;

  }

  columnClass: Record<number, any> = [];

  headerColumnClass: Record<number, any> = [];

  getColumnClass(column: { align?: string, classList?: string[] }) {
    console.log("getColumnClass");
    let classes: string[] = [];

    if (column.align) {
      classes.push(column.align);
    }
    if (column.classList && Array.isArray(column.classList)) {
      classes.push(...column.classList);
    }
    return classes;
  }

  getHeaderColumnClass(column: { align?: string, headerClass?: string }) {
    console.log("getHeaderColumnClass");
    let classes: string[] = [];

    if (column.align) {
      classes.push(column.align);
    }

    if (column.headerClass) {
      classes.push(column.headerClass);
    }

    return classes;
  }

  trackByFn(index, item) {
    return index;
  }

  toggleSort(field) {
    if (this.lastSort && this.lastSort != field) {
      delete this.sort[this.lastSort];
    }

    if (!this.sort[field]) {
      this.sort[field] = 'asc';
    } else {
      if (this.sort[field] == 'asc') {
        this.sort[field] = 'desc';
      } else {
        this.sort[field] = 'asc';
      }
    }

    if (this.sortMethod == 'external') {
      this.onSortChange.emit({ value: field, direction: this.sort[field] });
    }

    this.lastSort = field;
    return false;
  }
}
