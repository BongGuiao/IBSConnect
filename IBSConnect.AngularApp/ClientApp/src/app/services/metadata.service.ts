import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { Item, IUser, Item as IItem, CategoryItem } from '../models/models';

@Injectable({ providedIn: 'root' })
export class MetaDataService {

  constructor(private http: HttpClient) {
  }

  //#region Application

  createApplication(name: string) {
    return this.http.post<number>(`api/metadata/applications`, { name: name });
  }

  updateApplication(id: number, name: string) {
    return this.http.put(`api/metadata/applications/${id}`, { name: name });
  }

  deleteApplication(id: number) {
    return this.http.delete(`api/metadata/applications/${id}`);
  }

  getApplications() {
    return this.http.get<Item[]>(`api/metadata/applications`);
  }

  //#endregion Application


  //#region UnitArea

  createUnitArea(name: string) {
    return this.http.post<number>(`api/metadata/unitareas`, { name: name });
  }

  updateUnitArea(id: number, name: string) {
    return this.http.put(`api/metadata/unitareas/${id}`, { name: name });
  }

  deleteUnitArea(id: number) {
    return this.http.delete(`api/metadata/unitareas/${id}`);
  }

  getUnitAreas() {
    return this.http.get<IItem[]>(`api/metadata/unitareas`);
  }

  //#endregion UnitArea


  //#region Category

  createCategory(name: string, isFreeTier: boolean) {
    return this.http.post<number>(`api/metadata/categories`, { name, isFreeTier });
  }

  updateCategory(id: number, name: string, isFreeTier: boolean) {
    return this.http.put(`api/metadata/categories/${id}`, { name, isFreeTier });
  }

  deleteCategory(id: number) {
    return this.http.delete(`api/metadata/categories/${id}`);
  }

  getCategories() {
    return this.http.get<CategoryItem[]>(`api/metadata/categories`);
  }

  //#endregion Category


  //#region Year

  createYear(name: string) {
    return this.http.post<number>(`api/metadata/years`, { name: name });
  }

  updateYear(id: number, name: string) {
    return this.http.put(`api/metadata/years/${id}`, { name: name });
  }

  deleteYear(id: number) {
    return this.http.delete(`api/metadata/years/${id}`);
  }

  getYears() {
    return this.http.get<Item[]>(`api/metadata/years`);
  }

  //#endregion Year


  //#region Section

  createSection(name: string) {
    return this.http.post<number>(`api/metadata/sections`, { name: name });
  }

  updateSection(id: number, name: string) {
    return this.http.put(`api/metadata/sections/${id}`, { name: name });
  }

  deleteSection(id: number) {
    return this.http.delete(`api/metadata/sections/${id}`);
  }

  getSections() {
    return this.http.get<Item[]>(`api/metadata/sections`);
  }

  //#endregion Section



  //#region Course

  createCourse(name: string) {
    return this.http.post<number>(`api/metadata/courses`, { name: name });
  }

  updateCourse(id: number, name: string) {
    return this.http.put(`api/metadata/courses/${id}`, { name: name });
  }

  deleteCourse(id: number) {
    return this.http.delete(`api/metadata/courses/${id}`);
  }

  getCourses() {
    return this.http.get<Item[]>(`api/metadata/courses`);
  }

  //#endregion Course


  //#region College

  createCollege(name: string) {
    return this.http.post<number>(`api/metadata/colleges`, { name: name });
  }

  updateCollege(id: number, name: string) {
    return this.http.put(`api/metadata/colleges/${id}`, { name: name });
  }

  deleteCollege(id: number) {
    return this.http.delete(`api/metadata/colleges/${id}`);
  }

  getColleges() {
    return this.http.get<Item[]>(`api/metadata/colleges`);
  }

  //#endregion College
}
