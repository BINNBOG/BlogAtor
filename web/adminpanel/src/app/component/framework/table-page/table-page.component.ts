import { AfterViewInit, Component, Input, OnInit, Output, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ADTSettings } from '../../../../../node_modules/angular-datatables/src/models/settings';
import { ISearchResult } from '../../../model/search/ISearchResult';
import { IColumnOptions } from './table-column/IColumnOptions';
import { TranslateService } from '@ngx-translate/core';
import { firstValueFrom } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';

@Component({
  selector: 'table-page',
  standalone: false,
  templateUrl: './table-page.component.html',
  styleUrl: './table-page.component.css'
})
export class TablePageComponent implements AfterViewInit {
  @Input() pageName!: string;
  @Input() elementsGetter!: (params: any) => Promise<ISearchResult<any>>;

  @Input() elementDeletter?: (key: string) => Promise<void>;
  @Input() createEntryModalComponent?: any;
  @Input() elementCreator?: (data: any) => Promise<void>;

  @Input() columnOptions: Array<IColumnOptions> = [];
  protected dtOptions?: ADTSettings;

  @ViewChild(DataTableDirective, { static: false }) dtElement!: DataTableDirective;
  @Output() reload: () => Promise<boolean> = async () => {
    (await this.dtElement.dtInstance).ajax.reload();
    return true;
  }

  protected isCreateEnabled() {
    return this.createEntryModalComponent !== null && this.createEntryModalComponent !== undefined
      && this.elementCreator !== null && this.elementCreator !== undefined;
  }

  constructor(private modalService: BsModalService, private translateService: TranslateService) {
  }

  private readonly translationSuffic: string = "Column";

  async ngAfterViewInit() {
    let adtColumnOptions = this.columnOptions.map(co => co.toAdtSettings());
    for (let i in adtColumnOptions) {
      let co = adtColumnOptions[i];
      co.title = await firstValueFrom(this.translateService.get(`${this.pageName}${this.translationSuffic}${co.title}`));
    }

    const that = this;
    this.dtOptions = {
      language: {
        url: '/i18n/dt.ru.json',
      },
      orderMulti: false,
      serverSide: true,
      ajax: (params: any, callback) => {
        that.elementsGetter(params).then(resp => {
          callback({
            recordsTotal: resp.total,
            recordsFiltered: resp.filtered,
            data: resp.items
          });
        });
      },
      columns: adtColumnOptions
    };
  }

  protected createEntryModalRef?: BsModalRef;

  protected async deleteElement(key: string) {
    await this.elementDeletter!(key);
  }

  protected onCreateEntryClick() {
    if (this.isCreateEnabled()) {
      const config: ModalOptions = {
        initialState: {
          creator: this.elementCreator
        }
      };
      this.createEntryModalRef = this.modalService.show(this.createEntryModalComponent, config);
    }
  }
}
