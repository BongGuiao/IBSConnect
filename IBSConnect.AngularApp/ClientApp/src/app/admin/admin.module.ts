import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { UserComponent } from './user/user.component';
import { UsersComponent } from './users/users.component';
import { MemberComponent } from './member/member.component';
import { MembersComponent } from './members/members.component';
import { CategoriesComponent } from './categories/categories.component';
import { YearsComponent } from './years/years.component';
import { CoursesComponent } from './courses/courses.component';
import { SectionsComponent } from './sections/sections.component';
import { CollegesComponent } from './colleges/colleges.component';
import { ApplicationsComponent } from './applications/applications.component';
import { ComponentsModule } from '../components/components.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DirectivesModule } from '../directives/directives.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PipesModule } from "../pipes/pipes.module";
import { ImportComponent } from './import/import.component';
import { SettingsComponent } from './settings/settings.component';
import { ReportsComponent } from './reports/reports.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BillingComponent } from './billing/billing.component';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BillComponent } from './bill/bill.component';
import { DatabaseComponent } from './database/database.component';
import { UnitAreasComponent } from "./unitareas/unitareas.component";
import { PaymentComponent } from './payment/payment.component';
import { SessionsComponent } from './sessions/sessions.component';
import { ArrearsComponent } from './arrears/arrears.component';

@NgModule({
  declarations: [
    UserComponent,
    UsersComponent,
    MemberComponent,
    MembersComponent,
    CategoriesComponent,
    YearsComponent,
    CoursesComponent,
    SectionsComponent,
    CollegesComponent,
    ApplicationsComponent,
    UnitAreasComponent,
    ImportComponent,
    SettingsComponent,
    ReportsComponent,
    BillingComponent,
    BillComponent,
    DatabaseComponent,
    PaymentComponent,
    ArrearsComponent,
    SessionsComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    PipesModule,
    TabsModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot(), // ToastrModule added
    TooltipModule.forRoot(),
    FontAwesomeModule,
    ComponentsModule,
    AdminRoutingModule,
    DirectivesModule
  ]
})
export class AdminModule {


}
