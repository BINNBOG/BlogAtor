import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSourceCollectorModalComponent } from './add-source-collector-modal.component';

describe('AddModalComponent', () => {
  let component: AddSourceCollectorModalComponent;
  let fixture: ComponentFixture<AddSourceCollectorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddSourceCollectorModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddSourceCollectorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
