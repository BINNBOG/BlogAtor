import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { UpdatableList } from '../../../../utils/updateable-list';
import { IHttpProxyConfiguration } from '../../../../model/config/http-proxy';
import { IConfigBase } from '../../../../model/config/config-base';
import { HttpProxyConfigAPIService } from '../../../../service/API/http-proxy-config-api.service';
import { AddHttpProxyConfigModalComponent } from './add-modal/add-modal.component';
import { ISearchResult } from '../../../../model/search/ISearchResult';
import { IColumnOptions } from '../../../framework/table-page/table-column/IColumnOptions';
import { SimpleTableColumn } from '../../../framework/table-page/table-column/simple-column';
import { TablePageComponent } from '../../../framework/table-page/table-page.component';
import { TemplateTableColumn } from '../../../framework/table-page/table-column/template-column';

@Component({
  selector: 'app-http-proxies',
  standalone: false,
  templateUrl: './http-proxies.component.html',
  styleUrl: './http-proxies.component.css'
})
export class HttpProxiesComponent implements OnInit {
  protected proxies: UpdatableList<IConfigBase<IHttpProxyConfiguration>> 
    = new UpdatableList<IConfigBase<IHttpProxyConfiguration>>(pc => pc.id!);

  protected that: HttpProxiesComponent = this;

  protected createModalComponent = AddHttpProxyConfigModalComponent;

  @ViewChild('actionsColumn') actionsColumn!: TemplateRef<any>;
  private actionsColumnGetter = () => this.actionsColumn;

  protected columns: Array<IColumnOptions> = [
    new SimpleTableColumn("Id", "id", {
      width: "5%",
      searchable: false,
      sortable: false
    }),
    new SimpleTableColumn("Url", "configuration.url", {
      width: "55%",
      searchable: false,
      sortable: false
    }),
    new SimpleTableColumn("Auth", "configuration.authentication", {
      width: "5%",
      searchable: false,
      sortable: false
    }),
    new SimpleTableColumn("Username", "configuration.username"),
    new TemplateTableColumn("Actions", this.actionsColumnGetter.bind(this), null, "id", {
      width: "10%",
      searchable: false,
      sortable: false
    }),
  ];

  @ViewChild(TablePageComponent, { static: false }) table!: TablePageComponent;

  constructor(private proxyConfigAPI: HttpProxyConfigAPIService) {
  }

  async ngOnInit() {
    const entities = await this.proxyConfigAPI.getProxies();
    this.proxies.addRange(entities);
  }

  protected async elementsGetter(params: any): Promise<ISearchResult<IConfigBase<IHttpProxyConfiguration>>> {
    return await this.proxyConfigAPI.searchProxies(params);
  }

  protected async elementCreator(config: IHttpProxyConfiguration): Promise<void> {
    const result = await this.proxyConfigAPI.addProxy(config);
    this.proxies.addRange([result]);
    await this.table.reload();
  }

  protected async elementDeletter(key: string): Promise<void> {
    const id = Number.parseInt(key);
    await this.proxyConfigAPI.deleteProxy(id);
    this.proxies.deleteSingle(id);
    await this.table.reload();
  }
}
