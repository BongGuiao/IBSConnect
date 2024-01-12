import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgModel } from '@angular/forms/';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'search-text',
  templateUrl: './search-text.component.html',
  styleUrls: ['./search-text.component.scss']
})
export class SearchTextComponent implements OnInit {
  faTimes = faTimes;

  @Input()
  placeholder;

  @Input()
  value: string;

  @ViewChild('input')
  input: NgModel;

  @Output()
  change = new EventEmitter<any>();

  constructor() {

  }

  ngOnInit(): void {

  }

  clear() {
    this.input.control.setValue('');
  }

  ngAfterViewInit() {
    this.input.valueChanges
      .pipe(debounceTime(250))
      .pipe(distinctUntilChanged())
      .subscribe((value) => {
        this.change.emit(value);
      });

    if (!this.placeholder) {
      this.placeholder = 'Search';
    }
  }
}
