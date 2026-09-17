import { TestBed } from '@angular/core/testing';
import { APIServiceBaseService } from './apiservice-base.service';

describe('APIServiceBaseService', () => {
  let service: APIServiceBaseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(APIServiceBaseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
