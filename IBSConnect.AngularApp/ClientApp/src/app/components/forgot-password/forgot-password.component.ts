import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthenticationService } from "../../services/authentication.service";
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  loading = false;
  submitted = false;
  sent = false;
  returnUrl: string;
  error: any;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService
  ) {

  }

  ngOnInit() {

    this.forgotPasswordForm = this.formBuilder.group({
      emailAddress: ['', Validators.required]
    });

  }

  // convenience getter for easy access to form fields
  get f() { return this.forgotPasswordForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.forgotPasswordForm.invalid) {
      return;
    }

    this.loading = true;
    this.authenticationService.forgotPassword(this.f.emailAddress.value)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(
        data => {
          this.sent = true;
        });
  }
}
