import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpProxiesComponent } from './http-proxies.component';

describe('HttpProxiesComponent', () => {
  let component: HttpProxiesComponent;
  let fixture: ComponentFixture<HttpProxiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HttpProxiesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HttpProxiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
