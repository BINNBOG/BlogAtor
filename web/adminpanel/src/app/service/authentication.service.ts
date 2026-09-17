import { Injectable, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MeAPIService } from './API/me-api.service';
import { IMeInfo } from '../model/authentication/IMeInfo';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private cAuthTokenStorageKey = "authToken";

  private _authToken: string | null = null;

  private _meInfo: IMeInfo | null = null;

  constructor(private meApiService: MeAPIService, 
    private router: Router) { 

    this.meApiService.getMeInfo().then(meInfo => {
      this._meInfo = meInfo;
    });
  }

  public logout() {
    localStorage.removeItem(this.cAuthTokenStorageKey);
    this._authToken = null;
    this.router.navigate(["login"]);
  }

  public setAuthToken(token: string) {
    localStorage.setItem(this.cAuthTokenStorageKey, token);
    this._authToken = token;
  }

  public getAuthToken(): string | null {
    if (this._authToken === null) {
      this._authToken = localStorage.getItem(this.cAuthTokenStorageKey);
    }
    return this._authToken;
  }

  public async getMeInfo(): Promise<IMeInfo | null> {
    if (this._meInfo === null) {
      this._meInfo = await this.meApiService.getMeInfo();
    }
    return this._meInfo;
  }

  public getMeInfoSynced(): IMeInfo | null {
    return this._meInfo;
  }
}
