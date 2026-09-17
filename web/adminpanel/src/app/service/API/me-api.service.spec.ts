import { TestBed } from '@angular/core/testing';

import { MeAPIService } from './me-api.service';

describe('MeAPIService', () => {
  let service: MeAPIService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeAPIService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
