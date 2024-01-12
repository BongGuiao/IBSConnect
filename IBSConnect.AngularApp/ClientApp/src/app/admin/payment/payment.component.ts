import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {

  paymentHours!: number;
  paymentMinutes!: number;
  amountPaid: number;

  errors!: any;

  @Input()
  totalMinutes!: number;

  @Input()
  rate!: number;

  get paymentTotalMinutes() {
    return (this.paymentHours ?? 0) * 60 + (this.paymentMinutes ?? 0);
  }

  constructor() { }

  ngOnInit(): void {
    let totalMinutes = this.totalMinutes;

    let mins = totalMinutes % 60;
    totalMinutes = totalMinutes - mins;

    let hours = Math.round(totalMinutes / 60);

    this.paymentHours = hours;
    this.paymentMinutes = mins;
  }

}
