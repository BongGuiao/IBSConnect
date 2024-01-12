import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ITableDef } from 'src/app/components/data-grid/models';
import { FilterRequest, IIBSTranHistory, IMember } from 'src/app/models/models';
import { MembersService } from 'src/app/services/members.service';
import { MetaDataService } from 'src/app/services/metadata.service';
import { DialogService } from "../../services/dialog.service";
import { DialogSettings } from "../../components/dialog-modal/dialog-modal.component";
import { MemberComponent } from "../member/member.component";
import { SettingsService } from "../../services/settings.service";
import { PaymentComponent } from '../payment/payment.component';
import { ArrearsComponent } from '../arrears/arrears.component';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.scss']
})
export class MembersComponent {
  source!: ITableDef<IMember>;
  mode!: 'add' | 'edit';
  selectedMember!: IMember;
  members!: IMember[];
  total!: number;

  subjectArrearRate = new BehaviorSubject<number>(0);

  filter: FilterRequest = {
    pageSize: 50,
    page: 1
  }

  setFilterQuery(query) {
    if (typeof (query) == 'string') {
      this.filter.query = query;
      this.loadData();
    }
  }

  constructor(
    private settingsService: SettingsService,
    private dialogService: DialogService,
    private toastrService: ToastrService,
    public membersService: MembersService) {

    this.source = {
      columns: [
        { name: 'idNo', text: "ID No." },
        { name: 'lastName', text: "Last Name" },
        { name: 'firstName', text: "First Name" },
        { name: 'middleName', text: "Middle Name" },
      ]
    }

    this.loadData();
  }

  @ViewChild('memberDialog')
  memberDialog: TemplateRef<any>;

  @ViewChild('memberView')
  memberView: MemberComponent;

  @ViewChild('payment')
  paymentComponent: PaymentComponent;

  @ViewChild('arrears')
  arrearsComponent: ArrearsComponent;

  add() {
    this.mode = 'add';
    this.selectedMember = <IMember>{};

    let dlgSettings = <DialogSettings>{
      title: "Add Member",
      buttons: ["Save", "Cancel"],
      template: this.memberDialog,
      closeOnClick: {
        "Save": false
      },
      size: "lg"
    }
    let result = this.dialogService.openCustom(dlgSettings);

    result.observable.subscribe(d => {
      if (d == "Save") {
        if (this.memberView.isValid()) {
          return this.membersService.addMember(this.memberView.save()).subscribe(() => {
            this.toastrService.success("Member added");
            result.instance.close();
            this.loadData();
          });
        }
      }
    });

  }

  page(pageNo: any) {
    this.filter.page = pageNo;
    this.loadMembers();
  }

  loadData() {
    this.settingsService.get().subscribe(settings => {
      for (let setting of settings) {
        switch (setting.name) {
          case "Rate":
            this.rate = parseFloat(setting.value);
            break;
        }
      }
    });

    this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers(this.filter).subscribe(d => {
      this.total = d.count;
      this.members = d.result;
    });
  }

  @ViewChild('paymentDialog')
  paymentDialog: TemplateRef<any>;

  @ViewChild('paymentArrearsDialog')
  paymentArrearsDialog: TemplateRef<any>;

  rate!: number;
  excessMinutes!: number;

  arrearsrate!: number;
  arrearsexcessMinutes!: number;

  @ViewChild('addTimeDialog')
  addTimeDialog: TemplateRef<any>;

  addTimeHours!: number;
  addTimeMinutes!: number;
  totalArrears!: IIBSTranHistory[];

  @ViewChild('deleteDialog')
  deleteDialog: TemplateRef<any>;

  confirmDelete!: string;

  select(member: IMember) {
    this.mode = 'edit';
    this.selectedMember = member;
    this.confirmDelete = "";

    let dlgSettings = <DialogSettings>{
      title: "Update Member",
      buttons: ["Add Payment", "Pay Arrears","Add Time", "Delete", "Save", "Cancel"],
      buttonClasses: {
        "Add Time": ["btn-success"],
        "Delete": ["btn-danger"]
      },
      template: this.memberDialog,
      closeOnClick: {
        "Add Payment": false,
        "Pay Arrears": false,
        "Add Time": false,
        "Delete": false,
        "Save": false
      },
      size: "lg"
    }

    let result = this.dialogService.openCustom(dlgSettings);

    result.observable.subscribe(d => {
      if (d == "Save") {

        if (this.memberView.isValid()) {
          return this.membersService.updateMember(this.selectedMember.id, this.memberView.save()).subscribe(() => {
            this.toastrService.success("Member updated");
            result.instance.close();
            this.loadData();
          });;
        }
      }
      else if (d == "Add Payment") {

        this.membersService.getMember(this.selectedMember.id).subscribe(d => {
          if (d.remainingMinutes < 0) {
            this.excessMinutes = -d.remainingMinutes;
          }
        });



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

            if (this.paymentComponent.paymentTotalMinutes * this.rate != this.paymentComponent.amountPaid) {
              this.paymentComponent.errors = { amountsNotEqual: true };
              return;
            }

            return this.membersService.addPayment(this.selectedMember.id, this.paymentComponent.amountPaid).subscribe(() => {
              this.toastrService.success("Payment added");
              result.instance.close();
              this.memberView.refresh();
            });;

          }
        });
      }
      else if (d == "Pay Arrears") {

        this.membersService.getTotalArrears(this.selectedMember.id).subscribe(d => {
          this.totalArrears = d;
          this.sumTotalArrears();
          
        });
        let result = this.dialogService.openCustom(<DialogSettings>{
          title: "Add Payment Arrears",
          template: this.paymentArrearsDialog,
          buttons: ["Pay Arrears", "Cancel"],
          closeOnClick: {
            "Pay Arrears": false,
          },
          buttonConditions: {
            "Pay Arrears": () => this.arrearsComponent.paymentTotalMinutes > 0
          },

        });

        result.observable.subscribe(d => {
          if (d == "Pay Arrears") {

            if (this.arrearsComponent.paymentTotalMinutes * this.rate != this.arrearsComponent.amountPaid) {
              this.arrearsComponent.errors = { amountsNotEqual: true };
              return;
            }

            return this.membersService.addPaymentArrears(this.selectedMember.id, this.arrearsComponent.amountPaid).subscribe(() => {
              this.toastrService.success("Payment arrears added");
              result.instance.close();
              this.memberView.refresh();
            });;

          }
        });
      }
      else if (d == "Add Time") {
        this.addTimeHours = null;
        this.addTimeMinutes = null;

        if (this.memberView.remainingMinutes < 0) {

          let result = this.dialogService.openOk({
            title: "Add Time",
            message: "Please settle the members bill before adding time."
          });

        } else {

          let result = this.dialogService.openCustom(<DialogSettings>{
            title: "Add Time",
            template: this.addTimeDialog,
            buttons: ["Add Time", "Cancel"],
            buttonConditions: {
              "Add Time": () => (this.addTimeHours ?? 0) * 60 + (this.addTimeMinutes ?? 0) > 0
            },

          });

          result.observable.subscribe(d => {
            if (d == "Add Time") {

              var addTimeAmount = (this.addTimeHours ?? 0) * 60 + (this.addTimeMinutes ?? 0);

              if (addTimeAmount <= 0) return;

              return this.membersService.creditMinutes(this.selectedMember.id, addTimeAmount).subscribe(() => {
                this.toastrService.success("Time added");
                result.instance.close();
                this.memberView.refresh();
              });;

            }
          });
        }



      }
      else if (d == "Delete") {

        let result = this.dialogService.openCustom(<DialogSettings>{
          title: "Delete Member",
          template: this.deleteDialog,
          buttons: ["Delete", "Cancel"],
          buttonClasses: {
            "Delete": ["btn-danger"]
          },
          buttonConditions: {
            "Delete": () => this.confirmDelete === "confirm"
          },

        });

        result.observable.subscribe(d => {
          if (d == "Delete") {
            return this.membersService.deleteMember(this.selectedMember.id).subscribe(() => {
              this.toastrService.success("Member deleted");
              result.instance.close();
              this.loadData();
            });;

          }
        });

      }

    });
  }
  private sumTotalArrears() {
    let excessMinutes = 0 ;
    this.membersService.totalTimeArrears = 0;
    this.membersService.arrearsRate = 0;
    this.totalArrears.forEach(x => {
      excessMinutes += ( x.totalMinutes);
      this.excessMinutes =  excessMinutes;
      this.membersService.totalTimeArrears = excessMinutes;
      this.subjectArrearRate.next(x.rate);
      this.subjectArrearRate.subscribe(xrate => {this.rate = xrate});
      this.membersService.arrearsRate = this.rate;
      this.arrearsrate = this.rate;
      
      
    })
  }
  

}
