import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LoginAPIService } from '../../service/API/login-api.service';
import { AuthenticationService } from '../../service/authentication.service';
import { Router } from '@angular/router';
import { PopupNotificationService } from '../../service/popup-notification.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;

  private cEmptyLoginFormError = "EmptyLoginFormError";

  private cLoginSuccessMessage = "LoginSuccessMessage";

  constructor(private fb: FormBuilder,
    private loginService: LoginAPIService,
    private authService: AuthenticationService,
    private notificationService: PopupNotificationService,
    private router: Router) {
    this.loginForm = fb.group({
      'username': [''],
      'password': ['']
    });
  }

  /**
   * login via HTTP API
   */
  protected async login() {
    const val = this.loginForm.value;
    if (val.username && val.password) {
      const token = await this.loginService.login(val.username, val.password);
      this.authService.setAuthToken(token);
      this.notificationService.success(this.cLoginSuccessMessage);
      this.router.navigate(["/"]);
    } else {
      this.notificationService.error(this.cEmptyLoginFormError);
    }
  }
}
