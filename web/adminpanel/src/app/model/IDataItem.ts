import moment from 'moment';

export interface IDataItem {
    id: number;
    dataSourceId: number;
    collectorId: number;

    title: string;
    content: string;
    link: string;

    uid: string;
    collectedDate: string;
    updatedDate: string;
}