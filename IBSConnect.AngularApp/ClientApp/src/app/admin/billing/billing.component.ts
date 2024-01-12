import { Component, ViewChild, TemplateRef } from '@angular/core';
import { IMember, FilterRequest, IMemberBill } from "../../models/models";
import { ITableDef } from "../../components/data-grid/models";
import { DialogService } from "../../services/dialog.service";
import { MembersService } from "../../services/members.service";
import { MemberComponent } from "../member/member.component";
import { DialogSettings } from "../../components/dialog-modal/dialog-modal.component";


@Component({
  selector: 'app-billing',
  templateUrl: './billing.component.html',
  styleUrls: ['./billing.component.scss']
})
export class BillingComponent  {
  source!: ITableDef<IMemberBill>;
  selectedMember: IMemberBill = null;
  members!: IMemberBill[];
  total!: number;

  filter: FilterRequest & { showAll: boolean } = {
    pageSize: 50,
    page: 1,
    showAll: false
  }

  setFilterQuery(query) {
    this.filter.query = query;
    this.loadData();
  }

  constructor(
    private dialogService: DialogService,
    private membersService: MembersService) {

    this.source = {
      columns: [
        { name: 'idNo', text: "ID No." },
        { name: 'lastName', text: "Last Name" },
        { name: 'firstName', text: "First Name" },
        { name: 'middleName', text: "Middle Name" },
        { name: 'allottedTime', text: "Allotted Time" },
        { name: 'totalMinutes', text: "Total Minutes" },
        { name: 'excessMinutes', text: "Billable Minutes" },
      ]
    }

    this.loadData();
  }

  @ViewChild('billDialog')
  billDialog: TemplateRef<any>;

  //@ViewChild('memberView')
  //memberView: MemberComponent;


  loadData() {
    this.membersService.getMembersBill(this.filter).subscribe(d => {
      this.total = d.count;
      this.members = d.result;
    });
  }

  print() {
    window.print();
  }

  close() {
    this.selectedMember = null;
  }

  select(member: IMemberBill) {
    this.selectedMember = member;
  }

  setShowAll(event: any) {
    var checked = event.target.checked;
    this.filter.showAll = checked;
    this.loadData();
  }

}
