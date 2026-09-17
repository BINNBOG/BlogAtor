import { TestBed } from '@angular/core/testing';

import { FileHelperServiceService } from './file-helper-service.service';

describe('FileHelperServiceService', () => {
  let service: FileHelperServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FileHelperServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
