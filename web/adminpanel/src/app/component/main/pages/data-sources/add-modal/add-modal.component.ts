import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IDataSource } from '../../../../../model/IDataSource';

@Component({
  selector: 'app-add-modal',
  standalone: false,
  templateUrl: './add-modal.component.html',
  styleUrl: './add-modal.component.css'
})
export class AddModalComponent {
  protected createDataSourceForm: FormGroup;

  @Input() creator!: (data: IDataSource) => Promise<void>;
  

  constructor(public bsModalRef: BsModalRef, 
    private fb: FormBuilder) {

    this.createDataSourceForm = fb.group({
      'name': ['']
    });
  }

  protected async createDataSource() {
    const val = this.createDataSourceForm.value;
    if (val.name) {
      await this.creator({
        id: undefined,
        name: val.name
      });
      this.bsModalRef.hide();
    } else {
      //this.notificationService.warning("courseCreateErrorProvideData");
    }
  }
}
