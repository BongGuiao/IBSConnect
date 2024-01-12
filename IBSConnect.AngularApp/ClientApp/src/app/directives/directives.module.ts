import { NgModule } from "@angular/core";
import { InputFocusDirective } from "./autoFocus.directive";
import { DialogDirective } from "./dialog.directive";
import { ScrollIntoViewIfDirective } from "./scrollIntoView.directive";

@NgModule({
  declarations: [
    InputFocusDirective,
    DialogDirective,
    ScrollIntoViewIfDirective
  ],
  exports: [
    InputFocusDirective,
    DialogDirective,
    ScrollIntoViewIfDirective
  ]
})
export class DirectivesModule { }
