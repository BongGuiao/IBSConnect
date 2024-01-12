import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from "@angular/router";
import { finalize, first } from 'rxjs/operators';
import { AuthenticationService } from "../../services/authentication.service";
import * as CustomValidators from "../../common/validators";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  loading = false;
  submitted = false;
  sent = false;
  returnUrl: string;
  error: any;
  token: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
    route.queryParams.pipe(first()).subscribe(d => {
      this.token = d.token;
    });

    this.resetPasswordForm = this.formBuilder.group({
      password: ['', [
        Validators.required,
        Validators.minLength(9),
        CustomValidators.validatePassword,
        CustomValidators.mustMatch('confirm')
      ]],
      confirm: ['', [Validators.required]],
    });
  }

  ngOnInit() {


  }

  // convenience getter for easy access to form fields
  get f() { return this.resetPasswordForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.resetPasswordForm.invalid) {
      return;
    }

    this.loading = true;
    this.authenticationService.resetPassword(this.token, this.f.password.value)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(
        data => {
          this.sent = true;
        },
        data => {
          this.error = { message: data.error.errors[0].message };
        });
  }

}
