import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SettingsService } from "../../services/settings.service";
import { ToastrService } from "ngx-toastr";
import { faQuestionCircle } from "@fortawesome/free-regular-svg-icons";
import { DialogSettings } from 'src/app/components/dialog-modal/dialog-modal.component';
import { DialogService } from 'src/app/services/dialog.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
  faQuestionCircle = faQuestionCircle;

  form!: FormGroup;
  mode!: 'reset';
  @ViewChild('initializeDialog')
  initializeDialog!: TemplateRef<any>;

  confirmInitialize!: string;

  constructor(private formBuilder: FormBuilder, 
    private settingsService: SettingsService, 
    private dialogService: DialogService,
    private toastrService: ToastrService) {
    this.form = this.formBuilder.group({
      "id": [0, Validators.required],
      "rate": ["", Validators.required],
      "defaultTime": ["", Validators.required],
      "defaultPassword": ["", Validators.required],
    });
  }

  ngOnInit(): void {
    this.settingsService.get().subscribe(settings => {
      
      for (let setting of settings) {
        switch (setting.name) {
          case "Rate":
            this.form.controls.rate.setValue(setting.value);
            break;
          case "DefaultTime":
            this.form.controls.defaultTime.setValue(setting.value);
            break;
          case "DefaultPassword":
            this.form.controls.defaultPassword.setValue(setting.value);
            break;
          
        }
        
        this.form.controls.id.setValue(setting.id);
        
      }
    });
  }

  isSubmitted!: boolean;
  save() {
    this.isSubmitted = true;

    if (this.form.invalid)
      return;
    
    let settings = [];
    settings.push({ name: "Id", value: this.form.controls.id.value.toString() });
    settings.push({ name: "Rate", value: this.form.controls.rate.value.toString() });
    settings.push({ name: "DefaultTime", value: this.form.controls.defaultTime.value.toString() });
    settings.push({ name: "DefaultPassword", value: this.form.controls.defaultPassword.value.toString() });

    this.settingsService.update(settings).subscribe(() => {
      this.toastrService.success("Settings saved");
    });
  }
  reset() {
    this.mode = 'reset';
    var currentUserId = JSON.parse(localStorage.getItem("currentUser"));
    
    let dlgSettings = <DialogSettings>{
      title: "Initialize Student Alotted Time",
      buttons: ["Confirm", "Cancel"],
      template: this.initializeDialog,
      buttonClasses: {
        "Confirm": ["btn-danger"]
      },
      buttonConditions: {
        "Confirm": () => this.confirmInitialize !== ""
      },
    }
    let result = this.dialogService.openCustom(dlgSettings);

    result.observable.subscribe(d => {
      if (d == "Confirm") {
          
           return this.settingsService.resetHistroy(this.confirmInitialize,currentUserId.id).subscribe(() => {
             this.toastrService.success("Time ALOTTED Successfully initialized.");
             result.instance.close();
             
           });
         

      }
    });

  }

}
