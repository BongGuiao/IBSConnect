import { Input } from '@angular/core';
import { Directive, ElementRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[dialogHost]',
})
export class DialogDirective {
  constructor(public viewContainerRef: ViewContainerRef) { }
}



