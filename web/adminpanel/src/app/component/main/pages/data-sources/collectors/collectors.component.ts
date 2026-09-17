import { Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ISourceCollector, ISourceCollectorInfo } from '../../../../../model/ISourceCollector';
import { SourceCollectorAPIService } from '../../../../../service/API/source-collector-api.service';
import { ActivatedRoute } from '@angular/router';
import { AddSourceCollectorModalComponent } from './add-modal/add-source-collector-modal.component';
import { IDataSource } from '../../../../../model/IDataSource';
import { DataSourceAPIService } from '../../../../../service/API/data-source-api.service';
import { UpdatableList } from '../../../../../utils/updateable-list';
import { ChangeNotificationService } from '../../../../../service/API/change-notification.service';
import { Subscription } from 'rxjs';
import { ChangeType, IChnageNotification } from '../../../../../model/IChangeNotification';
import { IRssConfiguration } from '../../../../../model/config/rss-configuration';
import { ISearchResult } from '../../../../../model/search/ISearchResult';
import { IColumnOptions } from '../../../../framework/table-page/table-column/IColumnOptions';
import { SimpleTableColumn } from '../../../../framework/table-page/table-column/simple-column';
import { TranslatePipe } from '@ngx-translate/core';
import { PipedTableColumn } from '../../../../framework/table-page/table-column/piped-column';
import { TemplateTableColumn } from '../../../../framework/table-page/table-column/template-column';
import { TablePageComponent } from '../../../../framework/table-page/table-page.component';

@Component({
  selector: 'app-collectors',
  standalone: false,
  templateUrl: './collectors.component.html',
  styleUrl: './collectors.component.css'
})
export class CollectorsComponent implements OnInit, OnDestroy {
  private dataSourceId: number | null = null;
  protected sourceCollectors: UpdatableList<ISourceCollector> = new UpdatableList((sc) => sc.id!);
  protected dataSource: IDataSource | null = null;

  protected sourceCollectorChangeSubscription?: Subscription;

  protected that: CollectorsComponent = this;
  protected columns: Array<IColumnOptions>;

  protected collectorCreateModalComponent = AddSourceCollectorModalComponent;

  @ViewChild('actionsColumn') actionsColumn!: TemplateRef<any>;
  private actionsColumnGetter = () => this.actionsColumn;

  constructor(
    private route: ActivatedRoute,
    private sourceCollectorAPI: SourceCollectorAPIService,
    private dataSourceAPI: DataSourceAPIService,
    private changeNotificationService: ChangeNotificationService,
    private translatePipe: TranslatePipe) {

    this.columns = [
      new SimpleTableColumn("Id", "sourceCollector.id", {
        width: "5%",
        searchable: false,
        sortable: false
      }),
      new PipedTableColumn("Type", "sourceCollector.collectorType", this.translatePipe, []),
      new TemplateTableColumn("Actions", this.actionsColumnGetter.bind(this), null, "sourceCollector.id", {
        width: "10%",
        searchable: false,
        sortable: false
      }),
    ]
  }
  
  async ngOnInit() {
    this.route.paramMap.subscribe(async params => {
      const paramId = params.get("id");
      if (paramId) {
        this.dataSourceId = +paramId;

        this.dataSource = await this.dataSourceAPI.getDataSource(this.dataSourceId);
        const entities = await this.sourceCollectorAPI.getDataSourceCollectors(this.dataSourceId);
        this.sourceCollectors.clear();
        this.sourceCollectors.addRange(entities);
      }
    });

    this.sourceCollectorChangeSubscription = this.changeNotificationService.sourceCollectionChanges.subscribe(this.onSourceCollectorChange.bind(this));
  }

  @ViewChild(TablePageComponent, { static: false }) table!: TablePageComponent;

  private onSourceCollectorChange(update: IChnageNotification<ISourceCollector>) {
    if (update.changeType === ChangeType.added) {
      this.sourceCollectors.addRange(update.entities);
    } else if (update.changeType === ChangeType.deleted) {
      this.sourceCollectors.deleteRange(update.entities);
    }
  }

  async ngOnDestroy() {
    this.sourceCollectorChangeSubscription?.unsubscribe();
  }

  protected async elementsGetter(params: any): Promise<ISearchResult<ISourceCollectorInfo>> {
    return await this.sourceCollectorAPI.searchSourceCollectors(this.dataSourceId!, params);
  }

  protected async rssCollectorCreator(config: IRssConfiguration): Promise<void> {
    if (this.dataSourceId !== null) {
      await this.sourceCollectorAPI.addRssCollector(this.dataSourceId, config);
      await this.table.reload();
    }
  }

  protected async elementDeletter(key: string): Promise<void> {
    const id = Number.parseInt(key);
    const sourceCollector = this.sourceCollectors.getElementByKey(id);
    if (sourceCollector !== null) {
      await this.sourceCollectorAPI.deleteSourceCollector(sourceCollector);
      await this.table.reload();
    }
  }
}