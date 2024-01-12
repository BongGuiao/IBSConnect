import { Component, Input, OnInit } from '@angular/core';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons';
import { TipsService } from "../../services/tips.service";

@Component({
  selector: 'info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.scss']
})
export class InfoComponent implements OnInit {
  faInfoCircle = faInfoCircle;

  @Input()
  topic: string;

  constructor(private tipsService: TipsService) { }

  ngOnInit(): void {
  }

  showTopic() {
    this.tipsService.openTopic(this.topic);
  }

}
