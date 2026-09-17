import { Injectable } from '@angular/core';
import { APIServiceBaseService } from './apiservice-base.service';
import { HttpClient } from '@angular/common/http';
import { IConfigBase } from '../../model/config/config-base';
import { IHttpProxyConfiguration } from '../../model/config/http-proxy';
import { ISearchResult } from '../../model/search/ISearchResult';

@Injectable({
  providedIn: 'root'
})
export class HttpProxyConfigAPIService extends APIServiceBaseService {
  readonly cProxyEndpoint = "/proxy";

  constructor(private http: HttpClient) {
    super(http);
  }

  public async getProxies(): Promise<Array<IConfigBase<IHttpProxyConfiguration>>> {
    return await this.get(this.cProxyEndpoint);
  }

  public async addProxy(config: IHttpProxyConfiguration): Promise<IConfigBase<IHttpProxyConfiguration>> {
    const resp = await this.post<IConfigBase<IHttpProxyConfiguration>, IHttpProxyConfiguration>(this.cProxyEndpoint, config);
    return resp;
  }

  public async deleteProxy(id: number) {
    await this.delete(`${this.cProxyEndpoint}/${id}`, {});
  }

  public async searchProxies(params: any): Promise<ISearchResult<IConfigBase<IHttpProxyConfiguration>>> {
    return await this.post<ISearchResult<IConfigBase<IHttpProxyConfiguration>>, any>(`${this.cProxyEndpoint}/search`, params);
  }
}
