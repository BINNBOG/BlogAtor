import { Injectable } from '@angular/core';
import { APIServiceBaseService } from './apiservice-base.service';
import { HttpClient } from '@angular/common/http';
import { IConfigBase } from '../../model/config/config-base';

@Injectable({
  providedIn: 'root'
})
export class ConfigApiService extends APIServiceBaseService {
  readonly cDataItemEndpoint = "/config";

  constructor(private http: HttpClient) {
    super(http);
  }

  public async saveConfig<CT>(path: string, config: CT): Promise<number> {
    const result = await this.post<number, IConfigBase<CT>>(`${this.cDataItemEndpoint}/${path}`, {
      id: undefined,
      configuration: config
    });
    return result;
  }

  public async getConfig<CT>(id: number): Promise<CT> {
    const result = await this.get<CT>(`${this.cDataItemEndpoint}/${id}`);
    return result;
  }
}
