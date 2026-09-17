import { Component, OnInit } from '@angular/core';
import { MeAPIService } from '../../../service/API/me-api.service';
import { IMeInfo } from '../../../model/authentication/IMeInfo';
import { AuthenticationService } from '../../../service/authentication.service';

@Component({
  selector: 'app-account-menu',
  standalone: false,
  templateUrl: './account-menu.component.html',
  styleUrl: './account-menu.component.css'
})
export class AccountMenuComponent implements OnInit {
  protected me: IMeInfo | null = null;

  constructor (private meApiService: MeAPIService,
    private authService: AuthenticationService) {
  }

  protected getUsername(): string | null {
    if (this.me === null) {
      return null;
    }
    return this.me.me.username;
  }

  async ngOnInit() {
    this.me = await this.meApiService.getMeInfo();
  }

  protected logout() {
    this.authService.logout();
  }
}
