import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from "@angular/forms";
import { finalize } from 'rxjs/operators';
import { UserService } from "../../services/users.service";
import * as CustomValidators from "../../common/validators";
import { Router, ActivatedRoute } from "@angular/router";
import { AuthenticationService } from "../../services/authentication.service";

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  changePasswordForm: FormGroup;
  loading = false;
  submitted = false;
  updated = false;
  returnUrl: string;
  errors: any;
  expired: boolean;

  messages = {
    "OldPasswordMustMatch": "Old password does not match the current password"
  };

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private userService: UserService,
  ) {

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    var reason = this.route.snapshot.queryParams['reason'];

    this.expired = reason == 'expired';

    this.changePasswordForm = this.formBuilder.group({
      oldPassword: ['', [Validators.required]],
      password: ['', [
        Validators.required,
        Validators.minLength(9),
        CustomValidators.validatePassword,
      ]],
      confirm: ['', [Validators.required]],
    }, {
      validators: [
        CustomValidators.mustNotMatchInGroup("password", "oldPassword"),
        CustomValidators.mustMatchInGroup("password", "confirm")
      ]
    });
  }

  ngOnInit() {


  }

  // convenience getter for easy access to form fields
  get f() { return this.changePasswordForm.controls; }

  submit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.changePasswordForm.invalid) {
      return;
    }

    this.loading = true;

    let changePasswordRequest = {
      oldPassword: this.f.oldPassword.value,
      newPassword: this.f.password.value
    };

    this.userService.changePassword(changePasswordRequest)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(
        data => {
          if (this.expired) {
            this.authenticationService.logoutUser();
          } else {
            this.authenticationService.resetUser();
          }
          this.updated = true;
        },
        data => {
          this.errors = data.error.errors;
        });
  }
}
