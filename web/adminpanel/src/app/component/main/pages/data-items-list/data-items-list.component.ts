import { Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { DataItemAPIService } from '../../../../service/API/data-item-api.service';
import { IDataItem } from '../../../../model/IDataItem';
import { UpdatableList } from '../../../../utils/updateable-list';
import { Subscription } from 'rxjs';
import { ChangeNotificationService } from '../../../../service/API/change-notification.service';
import { ChangeType, IChnageNotification } from '../../../../model/IChangeNotification';
import { ADTSettings } from '../../../../../../node_modules/angular-datatables/src/models/settings';
import { DatePipe } from '@angular/common';
import { IColumnOptions } from '../../../framework/table-page/table-column/IColumnOptions';
import { SimpleTableColumn } from '../../../framework/table-page/table-column/simple-column';
import { TemplateTableColumn } from '../../../framework/table-page/table-column/template-column';
import { PipedTableColumn } from '../../../framework/table-page/table-column/piped-column';
import { ISearchResult } from '../../../../model/search/ISearchResult';

@Component({
  selector: 'app-data-items-list',
  standalone: false,
  templateUrl: './data-items-list.component.html',
  styleUrl: './data-items-list.component.css'
})
export class DataItemsListComponent implements OnInit, OnDestroy {
  protected dataItems: UpdatableList<IDataItem> = new UpdatableList<IDataItem>(di => di.id);

  protected dataItemChangeSubscription?: Subscription;

  protected dtOptions?: ADTSettings;

  protected columns: Array<IColumnOptions>;

  @ViewChild('dataItemLink') linkColumn!: TemplateRef<any>;
  private linkColumnGetter = () => this.linkColumn;

  constructor(private dataItemAPI: DataItemAPIService,
              private changeNotificationService: ChangeNotificationService,
              private datePipeInstance: DatePipe) {

    this.columns = [
      new SimpleTableColumn("Id", "id", {
        width: "5%",
        searchable: false,
        sortable: false
      }),
      new TemplateTableColumn("Title", this.linkColumnGetter.bind(this), null, null, {
        width: "65%",
        searchable: false,
        sortable: true
      }),
      new PipedTableColumn("Updated", "updatedDate", this.datePipeInstance, ['YYYY-MM-dd'], {
        width: "15%",
        searchable: false,
        sortable: true
      }),
      new PipedTableColumn("Collected", "collectedDate", this.datePipeInstance, ['YYYY-MM-dd'], {
        width: "15%",
        searchable: false,
        sortable: true
      }),
    ]
  }

  protected that = this;
  protected async elementsGetter(params: any): Promise<ISearchResult<IDataItem>> {
    return await this.dataItemAPI.searchItems(params);
  }

  private onDataItemChange(update: IChnageNotification<IDataItem>) {
      if (update.changeType === ChangeType.added) {
        this.dataItems.addRange(update.entities);
      } else if (update.changeType === ChangeType.deleted) {
        this.dataItems.deleteRange(update.entities);
      }
    }

  async ngOnInit() {
    setTimeout(() => {
      const that = this;
      this.dtOptions = {
        language: {
          url: '/i18n/dt.ru.json',
        },
        serverSide: true,
        ajax: (params: any, callback) => {
          that.dataItemAPI.searchItems(params).then(resp => {
            callback({
              recordsTotal: resp.total,
              recordsFiltered: resp.filtered,
              data: resp.items
            });
          });
        },
        columns: [{
          title: 'ID',
          data: 'id'
        }, {
          title: 'Link',
          data: 'link',
          defaultContent: 'def',
          ngTemplateRef: {
            ref: that.linkColumn
          }
        }, {
          title : 'Updated',
          data: 'updatedDate',
          ngPipeInstance: that.datePipeInstance,
          ngPipeArgs: [ 'YYYY-MM-dd' ]
        }, {
          title : 'Collected',
          data: 'collectedDate',
          ngPipeInstance: that.datePipeInstance,
          ngPipeArgs: [ 'YYYY-MM-dd' ]
        }]
      };
    });

    const entities = await this.dataItemAPI.getDataItems();
    this.dataItems.addRange(entities);

    this.dataItemChangeSubscription = this.changeNotificationService.dataItemChanges.subscribe(this.onDataItemChange.bind(this));
  }

  async ngOnDestroy() {
    this.dataItemChangeSubscription?.unsubscribe();
  }
}