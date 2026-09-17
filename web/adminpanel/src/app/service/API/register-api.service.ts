import { Injectable } from '@angular/core';
import { APIServiceBaseService } from './apiservice-base.service';
import { HttpClient } from '@angular/common/http';
import { ILoginRegister } from '../../model/authentication/ILoginRegisterRequest';

@Injectable({
  providedIn: 'root'
})
export class RegisterAPIService extends APIServiceBaseService {
  readonly cRegisterEndpoint = "/register";

  constructor(private http: HttpClient) {
    super(http);
  }

  public async register(username: string, password: string): Promise<boolean> {
    await this.post<any, ILoginRegister>(this.cRegisterEndpoint, {
      username: username,
      password: password
    });
    return true;
  }
}
