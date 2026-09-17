import { Component, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-import-data-source-modal',
  standalone: false,
  templateUrl: './import-data-source-modal.component.html',
  styleUrl: './import-data-source-modal.component.css'
})
export class ImportDataSourceModalComponent {
  @Input() importFunc!: (data: any) => Promise<void>;

  private importFile: File | null = null;
  
  constructor(public bsModalRef: BsModalRef) {
  }

  protected async onImportSubmit() {
    if (this.importFile === null) {
      return;
    }

    const data = await this.importFile.text();
    const importData = JSON.parse(data);

    await this.importFunc(importData);
    this.bsModalRef.hide();
  }

  protected onImportPicked(event: Event) {
    const file = (event.target as HTMLInputElement).files![0];
    this.importFile = file;
  }
}
