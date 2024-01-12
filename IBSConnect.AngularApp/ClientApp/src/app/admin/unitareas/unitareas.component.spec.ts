import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnitAreasComponent } from './unitareas.component';

describe('ApplicationsComponent', () => {
  let component: UnitAreasComponent;
  let fixture: ComponentFixture<UnitAreasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UnitAreasComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UnitAreasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
