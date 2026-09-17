import { TestBed } from '@angular/core/testing';

import { DataSourceAPIService } from '../data-source-api.service';

describe('DataSourceAPIService', () => {
  let service: DataSourceAPIService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataSourceAPIService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
