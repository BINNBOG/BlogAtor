import { Component, Input } from '@angular/core';
import { UserRole } from '../../../model/IUser';
import { AuthenticationService } from '../../../service/authentication.service';

@Component({
  selector: '[authorized]',
  standalone: false,
  templateUrl: './authorized.component.html',
  styleUrl: './authorized.component.css'
})
export class AuthorizedComponent {
  @Input() roles!: Array<UserRole>;

  constructor(private authService: AuthenticationService) {}

  protected isAuthorized(): boolean {
    const userRoles = this.authService.getMeInfoSynced()?.roles ?? [];
    const matchingRoles = userRoles.filter(r => this.roles.includes(r)) ?? [];
    return matchingRoles.length > 0;
  }
}
