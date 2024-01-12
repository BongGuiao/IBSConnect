import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ITableDef } from "../../components/data-grid/models";
import { IUser, FilterRequest } from "../../models/models";
import { DialogService } from "../../services/dialog.service";
import { ToastrService } from "ngx-toastr";
import { MembersService } from "../../services/members.service";
import { MemberComponent } from "../member/member.component";
import { DialogSettings, DialogSettings as IDialogSettings } from "../../components/dialog-modal/dialog-modal.component";
import { UserComponent } from "../user/user.component";
import { UserService } from "../../services/users.service";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent {
  source!: ITableDef<IUser>;
  mode!: 'add' | 'edit';
  selectedUser!: IUser;
  users!: IUser[];
  total!: number;

  filter: FilterRequest = {
    pageSize: 50,
    page: 1
  }

  setFilterQuery(query) {
    this.filter.query = query;
    this.loadData();
  }

  constructor(
    private dialogService: DialogService,
    private toastrService: ToastrService,
    private userService: UserService) {

    this.source = {
      columns: [
        { name: 'userName', text: "UserName" },
        { name: 'lastName', text: "Last Name" },
        { name: 'firstName', text: "First Name" },
        { name: 'middleName', text: "Middle Name" },
      ]
    }

    this.loadData();
  }

  @ViewChild('userDialog')
  userDialog: TemplateRef<any>;

  @ViewChild('userView')
  userView: UserComponent;

  @ViewChild('deleteDialog')
  deleteDialog: TemplateRef<any>;

  confirmDelete!: string;

  add() {
    this.mode = 'add';
    this.selectedUser = <IUser>{};

    let dlgSettings = <DialogSettings>{
      title: "Add User",
      buttons: ["Save", "Cancel"],
      template: this.userDialog,
      closeOnClick: {
        "Save": false
      },
      size: "lg"
    }
    let result = this.dialogService.openCustom(dlgSettings);

    result.observable.subscribe(d => {
      if (d == "Save") {
        if (this.userView.isValid()) {
          return this.userService.addUser(this.userView.save()).subscribe(() => {
            this.toastrService.success("User added");
            result.instance.close();
            this.loadData();
          });
        }
      }
    });

  }

  loadData() {
    this.userService.getUsers().subscribe(d => {
      this.users = d;
    //  this.total = d.count;
    //  this.users = d.result;
    });
  }

  select(user: IUser) {
    this.mode = 'edit';
    this.selectedUser = user;

    let dlgSettings = <IDialogSettings>{
      title: "Update User",
      buttons: ["Save","Delete", "Cancel"],
      template: this.userDialog,
      closeOnClick: {
        "Save": false
      },
      size: "lg"
    }

    let result = this.dialogService.openCustom(dlgSettings);

    result.observable.subscribe(d => {
      if (d == "Save") {

        if (this.userView.isValid()) {
          return this.userService.updateUser(this.selectedUser.id, this.userView.save()).subscribe(() => {
            this.toastrService.success("User updated");
            result.instance.close();
            this.loadData();
          });;
        }
      };
      if (d == "Delete") {

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
               return this.userService.deleteUser(this.selectedUser.id).subscribe(() => {
             this.toastrService.success("User deleted");
             result.instance.close();
             this.loadData();
           });;

          }
        });

      }
    });
  }

}
