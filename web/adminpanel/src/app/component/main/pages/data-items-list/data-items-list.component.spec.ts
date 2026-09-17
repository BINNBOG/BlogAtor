import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataItemsListComponent } from './data-items-list.component';

describe('DataItemsListComponent', () => {
  let component: DataItemsListComponent;
  let fixture: ComponentFixture<DataItemsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DataItemsListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DataItemsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
