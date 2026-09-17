import { HttpClient, HttpErrorResponse, HttpResponseBase } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, firstValueFrom } from 'rxjs';
import { AdminPanelConfig } from '../../app.config';
import { IAPIResponse } from '../../model/IAPIResponse';
import { IErrorInfo } from '../../model/IErrorInfo';
import { PopupNotificationService } from '../popup-notification.service';

@Injectable({
  providedIn: 'root'
})
export class APIServiceBaseService {
  readonly cApiPath = "/api";

  private notificationService: PopupNotificationService;
  
  constructor(private httpClient: HttpClient) {
    this.notificationService = inject(PopupNotificationService);
  }

  protected makePath(path: string): string {
    return AdminPanelConfig.cBaseUrl + this.cApiPath + path;
  }

  private async handleApiErrors(errorInfo: IErrorInfo) {
    this.notificationService.error(errorInfo.errorKey, errorInfo.errorData);
  }

  protected async get<RT>(path: string): Promise<RT> {
    const resp = await firstValueFrom(this.httpClient.get<IAPIResponse<RT>>(this.makePath(path)));
    if (!resp.isSuccess) {
      this.handleApiErrors(resp.errorInfo);
    }
    return resp.data;
  }

  protected async post<RT, DT>(path: string, data: DT): Promise<RT> {
    const resp = await firstValueFrom(this.httpClient.post<IAPIResponse<RT>>(this.makePath(path), data))
      .catch((error: HttpErrorResponse) => {
        const apiError = error.error as IAPIResponse<any>;
        this.handleApiErrors(apiError.errorInfo);
        return null;
      });
    if (resp === null) {
      throw new Error("error");
    }
    return resp.data;
  }

  protected async delete<RT, DT>(path: string, data: DT): Promise<RT> {
    const resp = await firstValueFrom(this.httpClient.delete<IAPIResponse<RT>>(this.makePath(path), { body: data }));
    return resp.data;
  }
}
