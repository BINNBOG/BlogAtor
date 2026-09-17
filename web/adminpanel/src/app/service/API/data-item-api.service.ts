import { Injectable } from '@angular/core';
import { APIServiceBaseService } from './apiservice-base.service';
import { HttpClient } from '@angular/common/http';
import { IDataItem } from '../../model/IDataItem';
import { ISearchResult } from '../../model/search/ISearchResult';

@Injectable({
  providedIn: 'root'
})
export class DataItemAPIService extends APIServiceBaseService {
  readonly cDataItemEndpoint = "/dataitem";

  constructor(private http: HttpClient) {
    super(http);
  }

  public async getDataItems(): Promise<Array<IDataItem>> {
    return await this.get<Array<IDataItem>>(this.cDataItemEndpoint);
  }

  public async searchItems(params: any): Promise<ISearchResult<IDataItem>> {
    const resp = await this.post<ISearchResult<IDataItem>, any>(`${this.cDataItemEndpoint}/search`, params);
    return resp;
  }
}
