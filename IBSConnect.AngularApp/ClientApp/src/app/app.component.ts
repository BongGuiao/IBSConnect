import { Component, ViewChild, TemplateRef, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ErrorService } from './services/error.service';
import { BsModalRef, BsModalService, ModalContainerComponent } from 'ngx-bootstrap/modal';
import { ErrorComponent } from './error/error.component';
import { DialogDirective } from "./directives/dialog.directive";
import { TipsComponent } from "./components/tips/tips.component";
import { DialogService } from "./services/dialog.service";

@Component({
  selector: 'app-root',
  styleUrls: ['./app.component.scss'],
  templateUrl: './app.component.html'
})
export class AppComponent implements OnDestroy {
  title = 'app';

  private ngUnsubscribe = new Subject();

  errorMessage: string;

  modalRef: BsModalRef<ErrorComponent>;

  @ViewChild(DialogDirective, { static: true }) dialogHost!: DialogDirective;

  @ViewChild('tipsHost')
  tipsHost: TemplateRef<any>;

  @ViewChild('helpHost')
  helpHost: TemplateRef<any>;

  @ViewChild('tips')
  tipsInstance: TipsComponent;

  constructor(
    private dialogService: DialogService,
    private errorService: ErrorService,
    private modalService: BsModalService) {
    this.initializeErrors();
  }

  ngAfterViewInit(): void {
    //this.tipsService.setHost(this.tipsHost);
    this.dialogService.setDirective(this.dialogHost);
  }


  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  private initializeErrors() {
    this
      .errorService
      .getErrors()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((errors) => {
        //this.displayErrorRef.error = errors
        const initialState = { errorMessage: errors[0] };
        this.modalRef = this.modalService.show(ErrorComponent, { initialState });
      });
  }
}
