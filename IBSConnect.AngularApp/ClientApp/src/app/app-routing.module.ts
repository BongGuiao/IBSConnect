import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard, AdminAuthGuard } from './authguard';

import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { LoginComponent } from "./components/login/login.component";
import { ForgotPasswordComponent } from "./components/forgot-password/forgot-password.component";
import { ChangePasswordComponent } from "./components/change-password/change-password.component";
import { ResetPasswordComponent } from "./components/reset-password/reset-password.component";
import { SessionComponent } from './session/session.component';
import { SessionExpiredComponent } from "./components/session-expired/session-expired.component";

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'session', component: SessionComponent, canActivate: [AuthGuard]  },
  { path: 'forgotpass', component: ForgotPasswordComponent },
  { path: 'changepass', component: ChangePasswordComponent },
  { path: 'resetpass', component: ResetPasswordComponent },
  { path: 'session-expired', component: SessionExpiredComponent },
  { path: 'admin', component: AdminComponent, canActivate: [AdminAuthGuard] },
  { path: '', component: HomeComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
