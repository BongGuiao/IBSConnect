import { Component, EventEmitter, Input, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

export interface DialogSettings {
  title: string;
  message?: string;
  template?: TemplateRef<any>;
  buttons?: string[];
  buttonClasses?: Record<string, string[]>;
  buttonConditions?: Record<string, () => boolean>;
  closeOnClick?: Record<string, boolean>;
  observable?: Observable<any>;
  size?: 'sm' | 'md' | 'lg';
}


@Component({
  selector: 'dialog-modal',
  templateUrl: './dialog-modal.component.html',
  styleUrls: ['./dialog-modal.component.scss']
})
export class DialogModalComponent implements OnInit {
  private selectSubject: BehaviorSubject<string>;


  @Input()
  title: string;

  @Input()
  message: string;

  @Input()
  contentTemplate: TemplateRef<any>;

  @Output()
  onSelect = new EventEmitter<string>();

  buttons: string[] = ['OK', 'Cancel'];

  buttonConditions: Record<string, () => boolean>;

  buttonClasses: Record<string, string[]>;

  btnClasses: Record<string, any> | string | string [];

  closeOnClick: Record<string, boolean>;

  modalRef: BsModalRef;

  @ViewChild('dialogModal')
  dialogModal: TemplateRef<any>;

  constructor(private modalService: BsModalService) { }

  select(choice: string) {
    this.onSelect.emit(choice);
    this.selectSubject.next(choice);
    if (this.closeOnClick === undefined || this.closeOnClick[choice] === undefined) {
      this.close();
    } else {
      if (this.closeOnClick[choice]) {
        this.close();
      }
    }
  }

  open(settings?: DialogSettings) {
    
    if (settings) {
      this.title = settings.title;
      this.message = settings.message;
      this.contentTemplate = settings.template;
      if (settings.buttons) {
        this.buttons = settings.buttons;
      } else {
        this.buttons = ['OK', 'Cancel'];
      }
      if (settings.buttonConditions) {
        this.buttonConditions = settings.buttonConditions;
      }

      this.btnClasses = {};

      for (let button of this.buttons) {
        this.btnClasses[button] = "btn-primary";
      }


      if (settings.buttonClasses) {
        this.buttonClasses = settings.buttonClasses;
        for (let button of this.buttons) {
          this.btnClasses[button] = this.buttonClasses[button] ?? this.btnClasses[button];
        }
        // { 'btn-primary': i == 0, 'btn-secondary': i > 0 }
      }
      if (settings.closeOnClick) {
        this.closeOnClick = settings.closeOnClick;
      }
      if (settings.observable) {
        settings.observable.pipe(finalize(() => this.close())).subscribe();
      }
    }

    // Hack to make sure dialogModal is not null when we call show()
    // Since this is being called from a created component, it won't have gon through the normal lifecycle? (ngAfterViewInit)
    setTimeout(() => {
      this.modalRef = this.modalService.show(this.dialogModal);
      if (settings.size) {
        this.modalRef.setClass("modal-" + settings.size);
      }
    }, 0);

    this.selectSubject = new BehaviorSubject<string>(null);
    return this.selectSubject.asObservable();
  }

  getDisabled(button: string) {
    if (this.buttonConditions) {
      if (this.buttonConditions[button]) {
        return !(this.buttonConditions[button]());
      }
      return false;
    }
    return false;
  }

  close() {
    this.modalRef.hide();
  }

  ngOnInit(): void {
  }

}
