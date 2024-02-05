import { Component, Input, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { mustMatch, mustMatchInGroup, requiredIf, requiredInGroupIf } from 'src/app/common/validators';
import { IMember, Item, ISession, IPayment, IPaymentArrears, IIBSTranHistory } from 'src/app/models/models';
import { MembersService } from 'src/app/services/members.service';
import { MetaDataService } from 'src/app/services/metadata.service';
import { DialogService } from '../../services/dialog.service';
import { DialogSettings } from "../../components/dialog-modal/dialog-modal.component";

@Component({
  selector: 'member',
  templateUrl: './member.component.html',
  styleUrls: ['./member.component.scss']
})
export class MemberComponent implements OnInit {
  isSubmitted: boolean = false;
  isSessionSubmitted: boolean = false;

  @Input()
  member!: IMember;

  @Input()
  mode!: 'add' | 'edit';

  sessions!: ISession[];
  payments!: IPayment[];
  arrears!: IPaymentArrears[];
  totalArrears!: IIBSTranHistory[];

  get f() {
    return this.form.controls;
  }

  get sf() {
    return this.sessionForm.controls;
  }

  form: FormGroup;
  sessionForm: FormGroup;

  years!: Item[];
  categories!: Item[];
  colleges!: Item[];
  courses!: Item[];
  picture: string;
  pictureChanged: boolean = false;
  changePassword: boolean = false;

  constructor(
    private toastrService: ToastrService,
    private dialogService: DialogService,
    private metaDataService: MetaDataService,
    private membersService: MembersService,
    private formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      "idNo": ["", Validators.required],
      "firstName": ["", Validators.required],
      "middleName": ["", Validators.required],
      "lastName": ["", Validators.required],
      "age": ["", Validators.required],
      "categoryId": ["", Validators.required],
      "collegeId": ["", Validators.required],
      "yearId": ["", Validators.required],
      "section": ["", Validators.required],
      "courseId": ["", Validators.required],
      "password": [""],
      "confirmPassword": [""],
      "notes": [""],
    });

    this.sessionForm = formBuilder.group({
      "id": [""],
      "startTime": ["", Validators.required],
      "endTime": ["", Validators.required],
    });

    this.metaDataService.getCategories().subscribe(d => {
      this.categories = d;
    });

    this.metaDataService.getYears().subscribe(d => {
      this.years = d;
    });

    this.metaDataService.getColleges().subscribe(d => {
      this.colleges = d;
    });

    this.metaDataService.getCourses().subscribe(d => {
      this.courses = d;
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

  credits: number;
  totalMinutes: number;
  remainingMinutes: number;

  totalTimeArrears: number;

  arrearsHour = 0;
  arrearsMinute = 0;

  loadData(member: IMember) {
    this.credits = member.credits;
    this.totalMinutes = member.totalMinutes;
    this.remainingMinutes = member.remainingMinutes;
    if (member.id) {
      this.picture = `/api/members/image?filename=${member.picture}`;
    } else {
      this.picture = "/assets/placeholder.png";
    }
    this.form.reset(member);
    // this.form.patchValue(this.member);
    this.form.markAsPristine();
    if (this.mode == "add") {
      this.requirePassword(true);
    }
  }

  ngOnInit(): void {
  }

  refresh() {
    this.membersService.getMember(this.member.id)
      .subscribe(d => {
        this.loadData(d);
        this.loadSessions();
        this.loadPayments();
        this.loadPaymentArrears();
        this.loadTotalArrears();
      });
  }

  ngOnChanges() {
    this.refresh();
  }

  loadSessions() {
    this.membersService.getMemberSessions(this.member.id)
      .subscribe(d => {
        this.sessions = d;
      });
  }

  loadPayments() {
    this.membersService.getPayments(this.member.id)
      .subscribe(d => {
        this.payments = d;
      });
  }
  loadPaymentArrears() {
    this.membersService.getPaymentArrears(this.member.id)
      .subscribe(d => {
        this.arrears = d;
      });
  }
  loadTotalArrears() {
    this.membersService.getTotalArrears(this.member.id)
      .subscribe(d => {
        this.totalArrears = d;
        this.totalTimeArrears = this.totalArrears[0].totalMinutes;
        this.membersService.totalTimeArrears = this.totalTimeArrears;
        this.getTimeArrears();
      });
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

  private getTimeArrears() {
    setTimeout(() => {
      let totalMinutesArrears = this.membersService.totalTimeArrears;

      let minsarrears = totalMinutesArrears % 60;

      totalMinutesArrears = totalMinutesArrears - minsarrears;

      let hoursarrears = Math.round(totalMinutesArrears / 60);
      this.arrearsHour = hoursarrears;
      this.arrearsMinute = minsarrears;

    }, 2000)
  }

  @ViewChild('editSession')
  editSession: TemplateRef<any>;

  edit(session: ISession) {
    this.isSessionSubmitted = false;
    let settings = <DialogSettings>{
      title: "Edit Session",
      template: this.editSession,
      buttons: ["Save", "Cancel"],
      closeOnClick: {
        "Save": false
      }
    }

    this.sessionForm.reset(session);

    let dialog = this.dialogService.openCustom(settings);

    dialog.observable.subscribe(d => {
      if (d == "Save") {
        this.isSessionSubmitted = true;
        if (this.sessionForm.valid) {
          
          this.membersService.updateMemberSession(this.member.id, this.sessionForm.value).subscribe(() => {
            this.toastrService.success("Session updated");
            dialog.instance.close();
            this.loadSessions();
          });
        }
      }
    });

  }
}
