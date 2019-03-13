import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddConfigurationDialogComponent } from './add-configuration-dialog.component';

describe('AddConfigurationDialogComponent', () => {
  let component: AddConfigurationDialogComponent;
  let fixture: ComponentFixture<AddConfigurationDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddConfigurationDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddConfigurationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
