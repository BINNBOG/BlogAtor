import { Injectable } from '@angular/core';
import { APIServiceBaseService } from './apiservice-base.service';
import { HttpClient } from '@angular/common/http';
import { IUserEntryModel } from '../../model/IUserEntryModel';
import { ISearchResult } from '../../model/search/ISearchResult';
import { IUser, UserRole } from '../../model/IUser';

@Injectable({
  providedIn: 'root'
})
export class UsersAPIService extends APIServiceBaseService {
  private cUsersApiEndpoint = "/user"

  constructor(private http: HttpClient) {
    super(http);
  }

  public async getAvailableRoles(): Promise<Array<UserRole>> {
    return await this.get<Array<UserRole>>(`${this.cUsersApiEndpoint}/roles`);
  }

  public async getUsers(): Promise<Array<IUserEntryModel>> {
    return await this.get<Array<IUserEntryModel>>(this.cUsersApiEndpoint);
  }

  public async searchUsers(params: any): Promise<ISearchResult<IUserEntryModel>> {
    return await this.post<ISearchResult<IUserEntryModel>, any>(`${this.cUsersApiEndpoint}/search`, params);
  }

  public async editUserRoles(userId: number, roles: Array<UserRole>) {
    await this.post<any, Array<UserRole>>(`${this.cUsersApiEndpoint}/${userId}/roles`, roles);
  }

  public async deleteUser(user: IUser) {
    await this.delete<any, IUser>(this.cUsersApiEndpoint, user);
  }
}
