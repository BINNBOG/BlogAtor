import { ADTColumns } from '../../../../../../node_modules/angular-datatables/src/models/settings';

export interface IColumnOptions {
    toAdtSettings(): ADTColumns;
}