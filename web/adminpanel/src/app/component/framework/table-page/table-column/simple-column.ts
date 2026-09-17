import { IColumnOptions } from "./IColumnOptions";
import { ADTColumns } from '../../../../../../node_modules/angular-datatables/src/models/settings';
import { IColumnExtraOptions } from "./IColumnExtraOptions";

export class SimpleTableColumn implements IColumnOptions {
    constructor(protected title: string, protected data: string | null, protected extraOptions: IColumnExtraOptions | null = null) {
    }

    public toAdtSettings(): ADTColumns {
        let result: ADTColumns = {
            title: this.title,
            name: this.title,
            data: this.data,
        };
        if (this.extraOptions !== null) {
            result.searchable = this.extraOptions.searchable;
            result.orderable = this.extraOptions.sortable;
            result.width = this.extraOptions.width;
        }
        return result;
    }
}