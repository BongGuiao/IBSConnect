import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminAuthGuard } from '../authguard';
import { AdminComponent } from './admin.component';
import { ApplicationsComponent } from './applications/applications.component';
import { CategoriesComponent } from './categories/categories.component';
import { CollegesComponent } from './colleges/colleges.component';
import { CoursesComponent } from './courses/courses.component';
import { MembersComponent } from './members/members.component';
import { SectionsComponent } from './sections/sections.component';
import { UsersComponent } from './users/users.component';
import { YearsComponent } from './years/years.component';
import { ImportComponent } from './import/import.component';
import { BillingComponent } from "./billing/billing.component";
import { SettingsComponent } from "./settings/settings.component";
import { ReportsComponent } from "./reports/reports.component";
import { DatabaseComponent } from "./database/database.component";
import { UnitAreasComponent } from "./unitareas/unitareas.component";
import { SessionsComponent } from './sessions/sessions.component';


const routes: Routes = [
  {
    path: 'admin', component: AdminComponent, canActivate: [AdminAuthGuard],
    children: [
      { path: 'settings', component: SettingsComponent, canActivate: [AdminAuthGuard] },
      { path: 'reports', component: ReportsComponent, canActivate: [AdminAuthGuard] },
      { path: 'billing', component: BillingComponent, canActivate: [AdminAuthGuard] },
      { path: 'sessions', component: SessionsComponent, canActivate: [AdminAuthGuard] },
      { path: 'applications', component: ApplicationsComponent, canActivate: [AdminAuthGuard] },
      { path: 'unitareas', component: UnitAreasComponent, canActivate: [AdminAuthGuard] },
      { path: 'categories', component: CategoriesComponent, canActivate: [AdminAuthGuard] },
      { path: 'colleges', component: CollegesComponent, canActivate: [AdminAuthGuard] },
      { path: 'courses', component: CoursesComponent, canActivate: [AdminAuthGuard] },
      { path: 'years', component: YearsComponent, canActivate: [AdminAuthGuard] },
      { path: 'sections', component: SectionsComponent, canActivate: [AdminAuthGuard] },
      { path: 'members', component: MembersComponent, canActivate: [AdminAuthGuard] },
      { path: 'users', component: UsersComponent, canActivate: [AdminAuthGuard] },
      { path: 'import', component: ImportComponent, canActivate: [AdminAuthGuard] },
      { path: 'database', component: DatabaseComponent, canActivate: [AdminAuthGuard] },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
