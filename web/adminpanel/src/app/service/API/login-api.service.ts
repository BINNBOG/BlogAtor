import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { APIServiceBaseService } from './apiservice-base.service';
import { ILoginRegister } from '../../model/authentication/ILoginRegisterRequest';

@Injectable({
  providedIn: 'root'
})
export class LoginAPIService extends APIServiceBaseService {
  readonly cRegisterEndpoint = "/login";

  constructor(private http: HttpClient) {
    super(http);
  }

  public async login(username: string, password: string): Promise<string> {
    return await this.post<string, ILoginRegister>(this.cRegisterEndpoint, {
      username: username,
      password: password
    });
  }
}