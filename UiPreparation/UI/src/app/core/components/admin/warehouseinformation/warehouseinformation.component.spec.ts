import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseinformationComponent } from './warehouseinformation.component';

describe('WarehouseinformationComponent', () => {
  let component: WarehouseinformationComponent;
  let fixture: ComponentFixture<WarehouseinformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseinformationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseinformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
