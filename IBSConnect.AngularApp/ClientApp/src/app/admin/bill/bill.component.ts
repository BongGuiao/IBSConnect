import { TemplateRef } from '@angular/core';
import { Component, OnInit, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { DialogSettings } from '../../components/dialog-modal/dialog-modal.component';
import { ISession, IMember, IMemberBill } from "../../models/models";
import { DialogService } from '../../services/dialog.service';
import { MembersService } from "../../services/members.service";
import { PaymentComponent } from '../payment/payment.component';


@Component({
  selector: 'bill',
  templateUrl: './bill.component.html',
  styleUrls: ['./bill.component.scss']
})
export class BillComponent implements OnInit, OnChanges {

  @Input()
  member: IMemberBill;

  bill: IMemberBill;

  showMinutes: boolean = false;


  @ViewChild('paymentDialog')
  paymentDialog: TemplateRef<any>;

  @ViewChild('paymentInstance')
  paymentComponent!: PaymentComponent;

  constructor(private dialogService: DialogService, private toastrService: ToastrService, private membersService: MembersService) {

  }

  ngOnChanges(changes: SimpleChanges) {
    this.loadData();
  }

  loadData() {
    this.membersService.getMemberBill(this.member.memberId).subscribe(d => {
      this.bill = d;
    });
  }

  ngOnInit(): void {

  }

  showPayment() {
    let totalExcess = this.bill.excessMinutes;


    let result = this.dialogService.openCustom(<DialogSettings>{
      title: "Add Payment",
      template: this.paymentDialog,
      buttons: ["Add Payment", "Cancel"],
      closeOnClick: {
        "Add Payment": false,
      },
      buttonConditions: {
        "Add Payment": () => this.paymentComponent.paymentTotalMinutes > 0
      },

    });

    result.observable.subscribe(d => {
      if (d == "Add Payment") {

        if (this.paymentComponent.paymentTotalMinutes * this.bill.rate != this.paymentComponent.amountPaid) {
          this.paymentComponent.errors = { amountsNotEqual: true };
          return;
        }

        return this.membersService.addPayment(this.member.memberId, this.paymentComponent.amountPaid).subscribe(() => {
          this.toastrService.success("Payment added");
          result.instance.close();
          this.loadData();
        });

      }
    });
  }


}
