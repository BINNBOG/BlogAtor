import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IMeInfo } from '../../model/authentication/IMeInfo';
import { APIServiceBaseService } from './apiservice-base.service';

@Injectable({
  providedIn: 'root'
})
export class MeAPIService extends APIServiceBaseService {
  readonly cMeInfoEndpoint = "/me";

  constructor(private http: HttpClient) {
    super(http);
  }

  public async getMeInfo(): Promise<IMeInfo> {
    return this.get<IMeInfo>(`${this.cMeInfoEndpoint}`);
  }
}
