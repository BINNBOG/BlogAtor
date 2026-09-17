import { TestBed } from '@angular/core/testing';

import { ChangeNotificationService } from './change-notification.service';

describe('ChangeNotificationService', () => {
  let service: ChangeNotificationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChangeNotificationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
