import { Component, OnInit, Input } from '@angular/core';
import { IMember, Item, Item as IItem, Item as IItem1, Item as IItem2, IUser } from "../../models/models";
import { ToastrService } from "ngx-toastr";
import { MetaDataService } from "../../services/metadata.service";
import { MembersService } from "../../services/members.service";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { mustMatchInGroup } from "../../common/validators";

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  isSubmitted: boolean = false;

  @Input()
  user!: IUser;

  @Input()
  mode!: 'add' | 'edit';

  get f() {
    return this.form.controls;
  };

  form: FormGroup;

  picture: string;
  pictureChanged: boolean = false;
  changePassword: boolean = false;

  constructor(
    private toastrService: ToastrService,
    private metaDataService: MetaDataService,
    private membersService: MembersService,
    private formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      "userName": ["", Validators.required],
      "firstName": ["", Validators.required],
      "middleName": ["", Validators.required],
      "lastName": ["", Validators.required],
      "password": [""],
      "confirmPassword": [""],
    });

  }

  requirePassword(required: boolean) {
    if (required) {
      this.form.addValidators(mustMatchInGroup("password", "confirmPassword"));
      this.form.controls.password.addValidators(Validators.required);
      this.form.controls.confirmPassword.addValidators(Validators.required);
    } else {
      this.form.clearValidators();
      this.form.controls.password.clearValidators();
      this.form.controls.confirmPassword.clearValidators();
    }
    this.form.controls.password.updateValueAndValidity();
    this.form.controls.confirmPassword.updateValueAndValidity();
    this.form.updateValueAndValidity();
  }

  loadData() {
    //if (this.user.id) {
    //  this.picture = `/api/members/${this.user.id}/image`;
    //} else {
    //  this.picture = "/assets/placeholder.png";
    //}
    this.form.reset(this.user);
    // this.form.patchValue(this.member);
    this.form.markAsPristine();
    if (this.mode == "add") {
      this.requirePassword(true);
    }
  }

  ngOnInit(): void {
  }

  ngOnChanges() {
    this.loadData();
  }

  setChangePassword(event: any) {
    this.changePassword = event.target.checked;
    this.requirePassword(event.target.checked);
  }


  loadImage(e: any) {
    var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    var pattern = /image-*/;
    var reader = new FileReader();
    if (!file.type.match(pattern)) {
      alert('invalid format');
      return;
    }
    reader.onload = (e) => {
      let reader = e.target;
      if (reader) {
        this.picture = <string>reader.result;
        this.pictureChanged = true;
      }
    };

    reader.readAsDataURL(file);
  }

  isValid() {
    this.isSubmitted = true;

    if (!this.form.valid) {
      return false;
    }

    return true;
  }


  save() {
    let request = this.form.value;

    if (this.pictureChanged) {
      request.picture = this.picture;
    } else {
      request.picture = null;
    }

    return request;
  }
}
