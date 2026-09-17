import { TestBed } from '@angular/core/testing';

import { HttpProxyConfigAPIService } from '../http-proxy-config-api.service';

describe('HttpProxyConfigAPIService', () => {
  let service: HttpProxyConfigAPIService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HttpProxyConfigAPIService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
