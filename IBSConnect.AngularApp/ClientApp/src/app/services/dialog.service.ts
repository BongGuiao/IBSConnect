import { Injectable, TemplateRef } from '@angular/core';
import { Subject } from 'rxjs';
import { DialogModalComponent, DialogSettings } from '../components/dialog-modal/dialog-modal.component';
import { DialogDirective } from "../directives/dialog.directive";

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  dialogHost: DialogDirective;

  constructor() {
  }

  /**
 * Sets the DialogDirective to be used by the service
 * @param host
 */
  setDirective(host: DialogDirective) {
    this.dialogHost = host;
  }

  /**
   * Opens a dialog modal. The return value of the Observable will be the text of the pressed button. If buttons is not set, it will default to ['OK','Cancel']
   * @param settings
   */
  open(settings: DialogSettings) {

    const viewContainerRef = this.dialogHost.viewContainerRef;
    //viewContainerRef.clear();


    const componentRef = viewContainerRef.createComponent<DialogModalComponent>(DialogModalComponent);

    let instance = componentRef.instance;
    let observable = componentRef.instance.open(settings);

    return {
      instance: instance,
      observable: observable
    };
  }


  /**
   * Opens a dialog modal with the buttons ['Yes','No'].  The return value is a boolean where Yes = true, No = false.
   * @param settings
   */
  openOk(settings: { title: string, message: string }) {

    const viewContainerRef = this.dialogHost.viewContainerRef;
    //viewContainerRef.clear();

    const componentRef = viewContainerRef.createComponent<DialogModalComponent>(DialogModalComponent);

    let instance = componentRef.instance;
    let observable = componentRef.instance.open({ ...settings, buttons: ['OK'] })

    return {
      instance: instance,
      observable: observable
    };
  }

  /**
   * Opens a dialog modal with the buttons ['Yes','No'].  The return value is a boolean where Yes = true, No = false.
   * @param settings
   */
  openYesNo(settings: { title: string, message: string }) {
    let result = new Subject<boolean>();


    const viewContainerRef = this.dialogHost.viewContainerRef;
    //viewContainerRef.clear();

    const componentRef = viewContainerRef.createComponent<DialogModalComponent>(DialogModalComponent);

    let instance = componentRef.instance;
    let observable = componentRef.instance.open({ ...settings, buttons: ['Yes', 'No'] })
      .subscribe((r) => {
        if (r == 'Yes') {
          result.next(true);
        } else {
          result.next(false);
        }
      });

    return {
      instance: instance,
      observable: result.asObservable()
    };
  }

  /**
  * Opens a dialog modal. The return value of the Observable will be the text of the pressed button. If buttons is not set, it will default to ['OK','Cancel']
  * @param settings
  */
  openCustom(settings: DialogSettings) {

    const viewContainerRef = this.dialogHost.viewContainerRef;
    //viewContainerRef.clear();


    const componentRef = viewContainerRef.createComponent<DialogModalComponent>(DialogModalComponent);

    let instance = componentRef.instance;
    let observable = componentRef.instance.open(settings);

    return {
      instance: instance,
      observable: observable
    };
  }

}
