import { Injectable, Injector } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class PopupNotificationService {

  constructor(private toastr: ToastrService,
    private injector: Injector) {}

  public success(key: string, params: Object = {}) {
    const translateService = this.injector.get(TranslateService);
    translateService.get(key, params).subscribe(translation => {
      this.toastr.success(translation);
    });
  }

  public error(key: string, params: Object = {}) {
    const translateService = this.injector.get(TranslateService);
    translateService.get(key, params).subscribe(translation => {
      this.toastr.error(translation);
    });
  }

  public warning(key: string, params: Object = {}) {
    const translateService = this.injector.get(TranslateService);
    translateService.get(key, params).subscribe(translation => {
      this.toastr.warning(translation);
    });
  }

  public info(key: string, params: Object = {}) {
    const translateService = this.injector.get(TranslateService);
    translateService.get(key, params).subscribe(translation => {
      this.toastr.info(translation);
    });
  }
}
