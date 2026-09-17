import { IConfigBase } from "./config/config-base";

export interface ISourceCollector {
    id: number | undefined;
    dataSourceId: number;
    collectorType: string;
    collectorConfigId: number;
}

export interface ISourceCollectorInfo {
    sourceCollector: ISourceCollector,
    configuraiotn: IConfigBase<any>
}