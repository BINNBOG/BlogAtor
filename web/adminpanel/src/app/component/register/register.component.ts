import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RegisterAPIService } from '../../service/API/register-api.service';
import { PopupNotificationService } from '../../service/popup-notification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm: FormGroup;

  private cRegisterPasswordsDoNotMatchError = "RegisterPasswordsDoNotMatchError";
  private cRegisterEmptyFormError = "RegisterEmptyFormError";

  private cRegistrationSuccessfullMessage = "RegistrationSuccessfullMessage"

  constructor(private fb: FormBuilder,
    private registerService: RegisterAPIService,
    private notificationService: PopupNotificationService,
    private router: Router) {

    this.registerForm = fb.group({
      'username': [''],
      'password': [''],
      'passwordConfirm': ['']
    });
  }

  /**
   * register via HTTP API
   */
  protected async register() {
    const val = this.registerForm.value;
    if (val.username && val.password && val.passwordConfirm) {
      if (val.password !== val.passwordConfirm) {
        this.notificationService.error(this.cRegisterPasswordsDoNotMatchError);
      } else {
        await this.registerService.register(val.username, val.password);
        this.notificationService.success(this.cRegistrationSuccessfullMessage);
        this.router.navigate(["/login"]);
      }
    } else {
      this.notificationService.error(this.cRegisterEmptyFormError);
    }
  }
}
