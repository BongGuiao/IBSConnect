import { Component, OnInit } from '@angular/core';
import { TipsService } from "../../services/tips.service";
import { TipsDatabase } from "../../help/tips";

@Component({
  selector: 'tips',
  templateUrl: './tips.component.html',
  styleUrls: ['./tips.component.scss']
})
export class TipsComponent implements OnInit {
  tipsDatabase: TipsDatabase;

  constructor(private tipsService: TipsService) {
    this.tipsDatabase = tipsService.tipsDatabase;
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.tipsService.setInstance(this);
  }

}
