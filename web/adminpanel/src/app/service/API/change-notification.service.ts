import { Injectable } from '@angular/core';

import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { IChnageNotification } from '../../model/IChangeNotification';
import { IDataSource } from '../../model/IDataSource';
import { ISourceCollector } from '../../model/ISourceCollector';
import { IDataItem } from '../../model/IDataItem';
import { AdminPanelConfig } from '../../app.config';

@Injectable({
  providedIn: 'root'
})
export class ChangeNotificationService {
  private _connection: HubConnection;

  private readonly cOnDataSourceChange = "OnDataSourceChange";
  private readonly cOnSourceCollectionChange = "OnSourceCollectorChange";
  private readonly cOnDataItemChange = "OnDataItemChange";

  private readonly cHubConnectionUrl = '/changes';

  private pendingDataSourceChangeData = new Subject<IChnageNotification<IDataSource>>();
  private pendingSourceCollectorChangeData = new Subject<IChnageNotification<ISourceCollector>>();
  private pendingDataItemChangeData = new Subject<IChnageNotification<IDataItem>>();
  
  public dataSourceChanges: Observable<IChnageNotification<IDataSource>> = this.pendingDataSourceChangeData.asObservable();
  public sourceCollectionChanges: Observable<IChnageNotification<ISourceCollector>> = this.pendingSourceCollectorChangeData.asObservable();
  public dataItemChanges: Observable<IChnageNotification<IDataItem>> = this.pendingDataItemChangeData.asObservable();

  constructor() { 
    this._connection = new HubConnectionBuilder()
      .withUrl(`${AdminPanelConfig.cBaseUrl}${this.cHubConnectionUrl}`, {
        transport: HttpTransportType.WebSockets
      })
      .build();

    this._connection
      .start()
      .then(() => console.log("[*] Connected to SignalR hub"))
      .catch(error => console.error("[!] Error connecting to SignalR hub", error));


    console.log("connected to SignalR");

    this._connection.on(this.cOnDataSourceChange, (changeNotification: IChnageNotification<IDataSource>) => {
      // console.log(`Received notification: ${JSON.stringify(changeNotification)}`);
      this.pendingDataSourceChangeData.next(changeNotification);
    });
    this._connection.on(this.cOnSourceCollectionChange, (changeNotification: IChnageNotification<ISourceCollector>) => {
      this.pendingSourceCollectorChangeData.next(changeNotification);
    });
    this._connection.on(this.cOnDataItemChange, (changeNotification: IChnageNotification<IDataItem>) => {
      this.pendingDataItemChangeData.next(changeNotification);
    });
  }
}
