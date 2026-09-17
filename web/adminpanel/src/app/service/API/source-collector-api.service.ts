import { Injectable } from '@angular/core';
import { APIServiceBaseService } from './apiservice-base.service';
import { HttpClient } from '@angular/common/http';
import { ISourceCollector, ISourceCollectorInfo } from '../../model/ISourceCollector';
import { IRssConfiguration } from '../../model/config/rss-configuration';
import { ISearchResult } from '../../model/search/ISearchResult';

@Injectable({
  providedIn: 'root'
})
export class SourceCollectorAPIService extends APIServiceBaseService {
  readonly cSourceCollector = "/sourcecollector";

  constructor(private http: HttpClient) {
    super(http);
  }

  public async getCollectorTypes(): Promise<Array<string>> {
    var resp = await this.get<Array<string>>(`${this.cSourceCollector}/types`);
    return resp;
  }

  public async getDataSourceCollectors(dataSourceId: number): Promise<Array<ISourceCollector>> {
    var resp = await this.get<Array<ISourceCollector>>(`${this.cSourceCollector}/${dataSourceId}`);
    return resp;
  }

  public async addSourceCollector(sourceCollector: ISourceCollector) {
    var resp = await this.post<ISourceCollector, ISourceCollector>(this.cSourceCollector, sourceCollector);
    return resp;
  }

  public async addRssCollector(dataSourceId: number, configuration: IRssConfiguration) {
    var resp = await this.post<ISourceCollector, IRssConfiguration>(`${this.cSourceCollector}/${dataSourceId}/rss`, configuration);
    return resp;
  }

  public async deleteSourceCollector(sourceCollector: ISourceCollector) {
    await this.delete(this.cSourceCollector, sourceCollector);
  }

  public async searchSourceCollectors(dataSourceId: number, params: any): Promise<ISearchResult<ISourceCollectorInfo>> {
    const result = await this.post<ISearchResult<ISourceCollectorInfo>, any>(`${this.cSourceCollector}/${dataSourceId}/search`, params);
    return result;
  }
}
