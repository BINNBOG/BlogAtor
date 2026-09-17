import { TestBed } from '@angular/core/testing';

import { PopupNotificationService } from './popup-notification.service';

describe('PopupNotificationService', () => {
  let service: PopupNotificationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PopupNotificationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
