import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportDataSourceModalComponent } from './import-data-source-modal.component';

describe('ImportDataSourceModalComponent', () => {
  let component: ImportDataSourceModalComponent;
  let fixture: ComponentFixture<ImportDataSourceModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ImportDataSourceModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ImportDataSourceModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
