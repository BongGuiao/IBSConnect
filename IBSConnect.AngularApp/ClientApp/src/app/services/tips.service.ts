import { Injectable, TemplateRef } from '@angular/core';
import { Subject } from 'rxjs';
import { DialogService } from './dialog.service';
import { TipsComponent } from "../components/tips/tips.component";
import { tap } from 'rxjs/operators';
import { TipsDatabase } from "../help/tips";

@Injectable({
  providedIn: 'root'
})
export class TipsService {
  host: TemplateRef<any>;
  instance: TipsComponent;
  tipsDatabase: TipsDatabase;

  constructor(private dialogService: DialogService) {
    this.tipsDatabase = new TipsDatabase();
  }

  setHost(host: TemplateRef<any>) {
    this.host = host;
  }

  setInstance(instance: TipsComponent) {
    this.instance = instance;
  }

  openTopic(topic: string) {
    this.tipsDatabase.setTopic(topic);

    this.dialogService.openCustom({
      title: 'Info',
      buttons: ['OK'],
      template: this.host
    }).observable.subscribe(d => {

    });

  }

  open() {
    this.tipsDatabase.random();

    this.dialogService.openCustom({
      title: 'Did you know?',
      buttons: ['Close','Previous', 'Next', 'Random'],
      closeOnClick: {
        'Previous': false,
        'Next': false,
        'Random': false,
        'Close': true,
      },
      template: this.host
    }).observable.subscribe(d => {
      if (d == 'Previous') {
        this.tipsDatabase.prev();
      }
      else if (d == 'Next') {
        this.tipsDatabase.next();
      }
      else if (d == 'Random') {
        this.tipsDatabase.random();
      }

    });

  }
}
