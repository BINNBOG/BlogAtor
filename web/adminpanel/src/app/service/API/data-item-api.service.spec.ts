import { TestBed } from '@angular/core/testing';

import { DataItemAPIService } from '../data-item-api.service';

describe('DataItemAPIService', () => {
  let service: DataItemAPIService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataItemAPIService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
