import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { SourceCollectorAPIService } from '../../../../../../service/API/source-collector-api.service';
import { IRssConfiguration } from '../../../../../../model/config/rss-configuration';

@Component({
  selector: 'app-add-modal',
  standalone: false,
  templateUrl: './add-source-collector-modal.component.html',
  styleUrl: './add-source-collector-modal.component.css'
})
export class AddSourceCollectorModalComponent implements OnInit {
  protected createSourceCollectorForm: FormGroup;

  @Input() public dataSourceId: number = 0;
  protected collectorTypes: Array<string> = [];
  protected selectedType: string | null = null;

  @Input() creator!: (config: IRssConfiguration) => Promise<void>;

  constructor(public bsModalRef: BsModalRef, 
    private fb: FormBuilder,
    private sourceCollectorAPI: SourceCollectorAPIService) {

    this.createSourceCollectorForm = fb.group({
      'type': [''],
      'url': new FormControl({value: '', disabled: true}, Validators.required),
      'useProxy': new FormControl({value: '', disabled: true}, Validators.required),
    });
  }

  async ngOnInit() {
    this.collectorTypes = await this.sourceCollectorAPI.getCollectorTypes();
  }

  protected collectorTypeChange(collectorType: any) {
    this.selectedType = collectorType.target.value as string;
    this.createSourceCollectorForm.get('url')?.enable();
    this.createSourceCollectorForm.get('useProxy')?.enable();
  }

  protected async createSourceCollector() {
    const val = this.createSourceCollectorForm.value;
    if (val.type && val.url) {
      await this.creator({
        url: val.url,
        useProxy: val.useProxy === "" ? false : val.useProxy
      });

      this.bsModalRef.hide();
    } else {
      //this.notificationService.warning("courseCreateErrorProvideData");
    }
  }
}
