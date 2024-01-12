import { TemplateRef } from "@angular/core";

export interface IColumnGroupDef {
  text: string;
  colspan: number;
  templateRef?: any;
  align?: 'center' | 'left' | 'right';
  classList?: string[];
}

export interface IColumnDef {
  /** The name of the property in the row element */
  name?: string;
  /** The text to display in the header */
  text?: string;
  /** The text template to display in the header */
  textTemplateRef?: TemplateRef<any>;
  /** The data type of this column */
  type?: 'number' | 'string' | 'date';
  /** A function to apply to the property */
  formatter?: (a: any) => any;
  template?: string;
  templateRef?: TemplateRef<any>;
  width?: string;
  minWidth?: string;
  align?: 'center' | 'left' | 'right';
  classList?: string[];
}

export interface ITableDef<T> {
  count?: number;
  columns: IColumnDef[];
  columnGroups?: IColumnGroupDef[];
  rowClass?: (row: T) => string | string;
  maxRows?: number;
  emptyRowsMessage?: string;
  maxHeight?: string;
  minWidth?: string;
  columnValues?: Record<string, string[]>
}


export interface IColumnSort {
  value: string;
  direction: 'asc' | 'desc';
}
