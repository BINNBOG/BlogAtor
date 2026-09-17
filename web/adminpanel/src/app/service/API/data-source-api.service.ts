import { Injectable } from '@angular/core';
import { APIServiceBaseService } from './apiservice-base.service';
import { HttpClient } from '@angular/common/http';
import { IDataSource } from '../../model/IDataSource';
import { ISearchResult } from '../../model/search/ISearchResult';

@Injectable({
  providedIn: 'root'
})
export class DataSourceAPIService extends APIServiceBaseService {
  readonly cDataSourceEndpoint = "/datasource";

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  public async getDataSources(): Promise<Array<IDataSource>> {
    return await this.get(this.cDataSourceEndpoint);
  }

  public async getDataSource(id: number): Promise<IDataSource> {
    return await this.get(`${this.cDataSourceEndpoint}/${id}`);
  }

  public async addDataSource(dataSource: IDataSource): Promise<Array<IDataSource>> {
    return await this.post(this.cDataSourceEndpoint, dataSource);
  }

  public async deleteDataSource(dataSource: IDataSource) {
    await this.delete(this.cDataSourceEndpoint, dataSource);
  }

  public async searchDataSources(params: any): Promise<ISearchResult<IDataSource>> {
    const resp = await this.post<ISearchResult<IDataSource>, any>(`${this.cDataSourceEndpoint}/search`, params);
    return resp;
  }

  public async exportDataSources(): Promise<any> {
    const resp = await this.get<any>(`${this.cDataSourceEndpoint}/export`);
    return resp;
  }

  public async importDataSources(data: any) {
    await this.post<any, any>(`${this.cDataSourceEndpoint}/import`, data);
  }
}
