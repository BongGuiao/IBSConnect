import { Component, OnInit, Input, Output } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { IUser } from '../models/models';
import { UserService } from '../services/users.service';
import { EventEmitter } from '@angular/core';

function MustMatch(controlName: string, matchingControlName: string) {
  return (formGroup: FormGroup) => {
    const control = formGroup.controls[controlName];
    const matchingControl = formGroup.controls[matchingControlName];

    if (matchingControl.errors && !matchingControl.errors.mustMatch) {
      // return if another validator has already found an error on the matchingControl
      return;
    }

    // set error on matchingControl if validation fails
    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ mustMatch: true });
    } else {
      matchingControl.setErrors(null);
    }
  }
}

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  @Input()
  user: IUser;

  @Input()
  mode: 'add' | 'edit' | 'delete';

  @Output()
  onClose = new EventEmitter<boolean>();

  roles = [
    'Reserver',
    'Approver',
    'Administrator',
  ];

  statuses = [
    'Active',
    'Inactive',
  ];

  selectedRole = 'Reserver';
  selectedStatus = 'Active';
  isSubmitted = false;
  isSaving = false;
  userForm: FormGroup;
  editPassword = false;
  setRole(role: string) {
    this.selectedRole = role;
    return false;
  }

  setStatus(status: string) {
    this.selectedStatus = status;
    return false;
  }

  constructor(private formBuilder: FormBuilder, private userService: UserService) {
    this.userForm = this.formBuilder.group({
      userName: new FormControl('', [Validators.required, Validators.minLength(8)]),
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(8)]),
      confirm: new FormControl('', [Validators.required]),
    }, {
      validator: MustMatch('password', 'confirm')
    });
  }

  get form() { return this.userForm.controls; }

  ngOnInit(): void {
    this.userForm.patchValue(this.user);
    if (this.mode == 'edit') {
      this.selectedRole = this.user.role;
      this.selectedStatus = this.user.isActive ? 'Active' : 'Inactive';
      this.form.password.setValue('************');
      this.form.confirm.setValue('************');
      this.form.password.disable();
      this.form.confirm.disable();
      this.form.userName.disable();
    }
  }

  togglePassword() {
    this.editPassword = !this.editPassword;
    if (this.editPassword) {
      this.form.password.setValue("");
      this.form.confirm.setValue("");
      this.form.password.enable();
      this.form.confirm.enable();
    }
    else {
      this.form.password.setValue("************");
      this.form.confirm.setValue("************");
      this.form.password.disable();
      this.form.confirm.disable();
    }
  }

  save() {
    this.isSubmitted = true;

    if (!this.userForm.invalid) {
      if (this.mode == 'edit' && this.editPassword) {
        this.user.password = this.form.password.value;
      } else if (this.mode == 'add') {
        this.user.password = this.form.password.value;
      }
      this.user.userName = this.form.userName.value;
      this.user.firstName = this.form.firstName.value;
      this.user.lastName = this.form.lastName.value;
      this.user.role = this.selectedRole;
      this.user.isActive = this.selectedStatus == "Active";

      if (this.mode == 'add') {
        this.userService.addUser(this.user)
          .subscribe(() => {
            this.onClose.emit(true);
          });
      }
      if (this.mode == 'edit') 
       {
        this.userService.updateUser(this.user.id, this.user)
          .subscribe(() => {
            this.onClose.emit(true);
          });
      }
      if (this.mode == 'delete') {
        this.userService.deleteUser(this.user.id)
          .subscribe(() => {
            this.onClose.emit(true);
          });
      }
    }
  }

  cancel() {
    this.onClose.emit(false);
  }

}
