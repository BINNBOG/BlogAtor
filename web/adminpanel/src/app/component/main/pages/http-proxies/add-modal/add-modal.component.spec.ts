import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddHttpProxyConfigModalComponent } from './add-modal.component';

describe('AddModalComponent', () => {
  let component: AddHttpProxyConfigModalComponent;
  let fixture: ComponentFixture<AddHttpProxyConfigModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddHttpProxyConfigModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddHttpProxyConfigModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
