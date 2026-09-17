import { IColumnOptions } from "./IColumnOptions";
import { ADTColumns } from '../../../../../../node_modules/angular-datatables/src/models/settings';
import { SimpleTableColumn } from "./simple-column";
import { TemplateRef } from "@angular/core";
import { IColumnExtraOptions } from "./IColumnExtraOptions";

export class TemplateTableColumn extends SimpleTableColumn implements IColumnOptions {
    constructor(title: string, private templateRefGetter: () => TemplateRef<any>, private userData: any = null, data: string | null = null, extraOptions: IColumnExtraOptions | null = null) {
        super(title, data, extraOptions);
    }

    public override toAdtSettings(): ADTColumns {
        let result = super.toAdtSettings();
        result.ngTemplateRef = {
            ref: this.templateRefGetter(),
            context: {
                captureEvents: () => {},
                userData: this.userData
            }
        };
        return result;
    }
}