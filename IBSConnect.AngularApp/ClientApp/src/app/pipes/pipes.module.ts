import { NgModule } from '@angular/core';
import { FilterPipe } from './filter.pipe';
import { SortByPipe } from './sortBy.pipe';
import { TimeSpanPipe } from './timeSpan.pipe';
import { FileSizePipe } from "./fileSize";

@NgModule({
  declarations: [
    SortByPipe,
    FilterPipe,
    TimeSpanPipe,
    FileSizePipe
  ],
  exports: [
    SortByPipe,
    FilterPipe,
    TimeSpanPipe,
    FileSizePipe
  ]
})
export class PipesModule { }
