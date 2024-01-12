import { Directive, ElementRef, OnInit } from "@angular/core";


@Directive({
    selector: 'input[autoFocus]'
})
export class InputFocusDirective implements OnInit {
    constructor(private element: ElementRef<HTMLInputElement>) { }

    ngOnInit(){
        this.element.nativeElement.focus();
    }
}