import { Input } from '@angular/core';
import { Directive, ElementRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[scrollIntoViewIf]'
})
export class ScrollIntoViewIfDirective {


  constructor(private viewContainer: ViewContainerRef) {

  }

  container: any;
  lastValue: boolean;
  parentTop: number;

  @Input()
  set scrollIntoViewIf(condition: boolean) {
    if (condition && !this.lastValue && !this.isVisible()) {
      this.viewContainer.element.nativeElement.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }
    this.lastValue = condition;
  }

  getScrollParent(node) {
    if (node == null) {
      return null;
    }

    this.parentTop = this.parentTop + node.offsetTop;

    if (node.scrollHeight > node.clientHeight) {
      return node;
    } else {
      return this.getScrollParent(node.parentNode);
    }
  }

  isVisible() {
    this.container = this.container ?? this.getScrollParent(this.viewContainer.element.nativeElement);
    let ele = this.viewContainer.element.nativeElement;

    if (this.container == null) {
      return true;
    }

    const eleTop = ele.offsetTop + this.parentTop;
    const eleBottom = eleTop + ele.clientHeight;

    const containerTop = this.container.scrollTop;
    const containerBottom = containerTop + this.container.clientHeight;

    // The element is fully visible in the container
    return (
      (eleTop >= containerTop && eleBottom <= containerBottom) ||
        // Some part of the element is visible in the container
        (eleTop < containerTop && containerTop < eleBottom) ||
        (eleTop < containerBottom && containerBottom < eleBottom)
    );
  };
}
