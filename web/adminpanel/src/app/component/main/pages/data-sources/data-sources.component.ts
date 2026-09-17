import { Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { IDataSource } from '../../../../model/IDataSource';
import { DataSourceAPIService } from '../../../../service/API/data-source-api.service';
import { AddModalComponent } from './add-modal/add-modal.component';
import { Subscription } from 'rxjs';
import { ChangeNotificationService } from '../../../../service/API/change-notification.service';
import { ChangeType, IChnageNotification } from '../../../../model/IChangeNotification';
import { UpdatableList } from '../../../../utils/updateable-list';
import { ISearchResult } from '../../../../model/search/ISearchResult';
import { IColumnOptions } from '../../../framework/table-page/table-column/IColumnOptions';
import { SimpleTableColumn } from '../../../framework/table-page/table-column/simple-column';
import { TemplateTableColumn } from '../../../framework/table-page/table-column/template-column';
import { TablePageComponent } from '../../../framework/table-page/table-page.component';
import { FileHelperServiceService } from '../../../../service/file-helper-service.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ImportDataSourceModalComponent } from './import-data-source-modal/import-data-source-modal.component';

@Component({
  selector: 'app-data-sources',
  standalone: false,
  templateUrl: './data-sources.component.html',
  styleUrl: './data-sources.component.css'
})
export class DataSourcesComponent implements OnInit, OnDestroy {

  protected dataSources: UpdatableList<IDataSource> = new UpdatableList<IDataSource>((ds) => ds.id!);

  protected that: DataSourcesComponent = this;

  protected createModalComponent = AddModalComponent;

  protected dataSourceChangeSubscription?: Subscription;

  @ViewChild('sourceNameColumn') nameColumn!: TemplateRef<any>;
  private nameColumnGetter = () => this.nameColumn;
  @ViewChild('actionsColumn') actionsColumn!: TemplateRef<any>;
  private actionsColumnGetter = () => this.actionsColumn;

  protected columns: Array<IColumnOptions> = [
    new SimpleTableColumn("Id", "id", {
      width: "5%",
      searchable: false,
      sortable: false
    }),
    new TemplateTableColumn("Name", this.nameColumnGetter.bind(this), null),
    new TemplateTableColumn("Actions", this.actionsColumnGetter.bind(this), null, "id", {
      width: "10%",
      searchable: false,
      sortable: false
    }),
  ];

  @ViewChild(TablePageComponent, { static: false }) table!: TablePageComponent;

  constructor(private datasourceApi: DataSourceAPIService,
    private notificationChangesService: ChangeNotificationService,
    private fileHelperService: FileHelperServiceService,
    private modalService: BsModalService
  ) {
  }

  private onDataSourceChange(update: IChnageNotification<IDataSource>) {
    if (update.changeType === ChangeType.added) {
      this.dataSources.addRange(update.entities);
    } else if (update.changeType === ChangeType.deleted) {
      this.dataSources.deleteRange(update.entities);
    }
  }
  
  async ngOnInit() {
    const entities = await this.datasourceApi.getDataSources();
    this.dataSources.addRange(entities);

    this.dataSourceChangeSubscription = this.notificationChangesService.dataSourceChanges.subscribe(this.onDataSourceChange.bind(this));
  }

  async ngOnDestroy() {
    this.dataSourceChangeSubscription?.unsubscribe();
  }

  protected async elementsGetter(params: any): Promise<ISearchResult<IDataSource>> {
    const result = this.datasourceApi.searchDataSources(params);
    return result;
  }

  protected async elementCreator(dataSource: IDataSource): Promise<void> {
    await this.datasourceApi.addDataSource(dataSource);
    await this.table.reload();
  }

  protected async elementDeletter(key: string): Promise<void> {
    const id = Number.parseInt(key);
    const dataSource = this.dataSources.getElementByKey(id);
    if (dataSource !== null) {
      await this.datasourceApi.deleteDataSource(dataSource);
      await this.table.reload();
    }
  }

  protected async exportDataSources() {
    const exportData = await this.datasourceApi.exportDataSources();
    this.fileHelperService.saveStringToFile("export.json", JSON.stringify(exportData));
  }

  protected importModalRef?: BsModalRef;
  protected onImportButtonClick() {
    const config: ModalOptions = {
      initialState: {
        importFunc: this.importData.bind(this)
      }
    };
    this.importModalRef = this.modalService.show(ImportDataSourceModalComponent, config);
  }

  protected async importData(data: any) {
    await this.datasourceApi.importDataSources(data);
    await this.table.reload();
  }
}
