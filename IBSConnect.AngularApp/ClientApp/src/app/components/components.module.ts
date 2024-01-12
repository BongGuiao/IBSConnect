import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from '@angular/common';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { LoginComponent } from './login/login.component';
import { DataGridComponent } from "./data-grid/data-grid.component";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SearchTextComponent } from './search-text/search-text.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SessionExpiredComponent } from './session-expired/session-expired.component';

@NgModule({
  declarations: [
    ChangePasswordComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    LoginComponent,
    DataGridComponent,
    SearchTextComponent,
    SessionExpiredComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    PaginationModule.forRoot(),
    RouterModule,
    CommonModule,
  ],
  exports: [
    DataGridComponent,
    SearchTextComponent
  ]
})
export class ComponentsModule { }
