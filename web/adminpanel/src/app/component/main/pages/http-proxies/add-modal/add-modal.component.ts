import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { IHttpProxyConfiguration } from '../../../../../model/config/http-proxy';

@Component({
  selector: 'app-add-modal',
  standalone: false,
  templateUrl: './add-modal.component.html',
  styleUrl: './add-modal.component.css'
})
export class AddHttpProxyConfigModalComponent {
  @Input() creator!: (data: IHttpProxyConfiguration) => Promise<void>;

  protected createHttpProxyConfigForm: FormGroup;
  
  constructor(public bsModalRef: BsModalRef, 
    private fb: FormBuilder) {

    this.createHttpProxyConfigForm = fb.group({
      'url': [''],
      'auth': [''],
      'username': [{value: '', disabled: true}],
      'password': [{value: '', disabled: true}]
    });
  }

  protected onChange(event: Event) {
    const auth = this.createHttpProxyConfigForm.get('auth');

    if (auth?.value) {
      this.createHttpProxyConfigForm.get('username')?.enable();
      this.createHttpProxyConfigForm.get('password')?.enable();
    } else {
      this.createHttpProxyConfigForm.get('username')?.disable();
      this.createHttpProxyConfigForm.get('password')?.disable();
    }
  }

  protected async createHttpProxyConfig() {
    const val = this.createHttpProxyConfigForm.value;
    if (val.url) {
      await this.creator({
        url: val.url,
        authentication: val.auth,
        username: val.auth ? val.username : null,
        password: val.auth ? val.password : null
      });
      this.bsModalRef.hide();
    } else {
      //this.notificationService.warning("courseCreateErrorProvideData");
    }
  }
}
