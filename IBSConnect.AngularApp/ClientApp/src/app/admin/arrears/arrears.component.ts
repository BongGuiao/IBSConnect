import { Component, Input, OnInit } from '@angular/core';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'arrears',
  templateUrl: './arrears.component.html',
  styleUrls: ['./arrears.component.scss']
})
export class ArrearsComponent implements OnInit {

  paymentHours!: number;
  paymentMinutes!: number;
  amountPaid: number;
  arrearsHour: number;
  arrearsMinute: number;

  errors!: any;

  @Input()
  totalMinutes!: number;

  @Input()
  rate!: number;

  @Input()
  totalArrearsHours!: number;
  
  arrearsrate!: number;

  

  get paymentTotalMinutes() {
    return (this.paymentHours ?? 0) * 60 + (this.paymentMinutes ?? 0);
  }


  constructor(  public membersService: MembersService) { 
    
  }

  ngOnInit(): void {
    this.paymentHours = 0;
    this.paymentMinutes = 0;
    let totalMinutes = 0 ;
    totalMinutes = this.membersService.totalTimeArrears;
    let mins = totalMinutes % 60;
    totalMinutes = totalMinutes - mins;

    let hours = 0;
    hours = Math.round(totalMinutes / 60);



    setTimeout(() => {
      let totalMinutesArrears = this.membersService.totalTimeArrears;

      let minsarrears = totalMinutesArrears % 60;

      totalMinutesArrears = totalMinutesArrears - minsarrears;

      let hoursarrears = Math.round(totalMinutesArrears / 60);
      this.arrearsHour = hoursarrears;
      this.arrearsMinute = minsarrears;

    },2000)
    
  }

}
