import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from "../services/authentication.service";
import { AbstractControl, FormControl, FormGroup, FormBuilder, Validators } from "@angular/forms";
import { MetaDataService } from "../services/metadata.service";
import { Item } from "../models/models";

@Component({
  selector: 'app-home',
  styleUrls: ['./home.component.scss'],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  isSubmitted: boolean = false;
  isLoggedin: boolean = false;
  applications!: Item[];
  unitAreas!: Item[];

  form!: FormGroup;
  idNo!: AbstractControl;
  password!: AbstractControl;

  selectedApplications: Record<number, boolean> = {};
  unitArea!: string;

  constructor(
    private router: Router,
    private metaDataService: MetaDataService,
    private authenticationService: AuthenticationService,
    private formBuilder: FormBuilder
  ) {
    this.idNo = new FormControl("", Validators.required);
    this.password = new FormControl("", Validators.required);

    this.form = formBuilder.group({
      "idNo": this.idNo,
      "password": this.password,
    });
  }

  ngOnInit() {
    this.metaDataService.getApplications().subscribe(d => {
      this.applications = d;
    });

    this.metaDataService.getUnitAreas().subscribe(d => {
      this.unitAreas = d;
    });

    if (this.authenticationService.currentUserValue && this.authenticationService.currentUserValue.role === "Member") {
      this.router.navigate(["/session"]);
    }
  }

  errors: Record<string, any> = {};

  toggleApplication(id: number) {
    this.selectedApplications[id] = !this.selectedApplications[id];
  }

  setUnitArea(id: string) {
    this.unitArea = id;
  }

  login() {
    this.isSubmitted = true;

    let appIds = <number[]>[];
    for (let key in this.selectedApplications) {
      if (this.selectedApplications[key]) {
        appIds.push(parseInt(key));
      }
    }

    this.errors = {};

    if (appIds.length == 0) {
      this.errors.applications = {
        required: true
      }
    }

    if (!this.unitArea || this.unitArea.length == 0) {
      this.errors.unitArea = {
        required: true
      };
    }

    if (!this.form.valid || this.errors.applications || this.errors.unitArea) {
      return;
    }

    let request = this.form.value;

    request.applications = appIds;
    request.unitArea = parseInt(this.unitArea);

    this.authenticationService.loginMember(request).subscribe(() => {
      this.router.navigate(["/session"]);
    }, (err) => {
      this.errors.message = "Invalid username or password";
    });
  }
}
