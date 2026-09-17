import { IColumnOptions } from "./IColumnOptions";
import { ADTColumns } from '../../../../../../node_modules/angular-datatables/src/models/settings';
import { SimpleTableColumn } from "./simple-column";
import { PipeTransform } from "@angular/core";
import { IColumnExtraOptions } from "./IColumnExtraOptions";

export class PipedTableColumn extends SimpleTableColumn implements IColumnOptions {
    constructor(title: string, data: string | null, private pipe: PipeTransform, private pipeArgs: any[] = [], extraOptions: IColumnExtraOptions | null = null) {
        super(title, data, extraOptions);
    }

    public override toAdtSettings(): ADTColumns {
        let result = super.toAdtSettings();
        result.ngPipeInstance = this.pipe;
        result.ngPipeArgs = this.pipeArgs;
        return result;
    }
}