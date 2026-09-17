import { TestBed } from '@angular/core/testing';

import { SourceCollectorAPIService } from '../source-collector-api.service';

describe('SourceCollectorAPIService', () => {
  let service: SourceCollectorAPIService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SourceCollectorAPIService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
