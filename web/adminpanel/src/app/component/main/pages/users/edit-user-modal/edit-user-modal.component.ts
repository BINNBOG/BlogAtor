import { Component, Input, OnInit } from '@angular/core';
import { IUserEntryModel } from '../../../../../model/IUserEntryModel';
import { UserRole } from '../../../../../model/IUser';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { PopupNotificationService } from '../../../../../service/popup-notification.service';

@Component({
  selector: 'app-edit-user-modal',
  standalone: false,
  templateUrl: './edit-user-modal.component.html',
  styleUrl: './edit-user-modal.component.css'
})
export class EditUserModalComponent implements OnInit {
  @Input() currentUserId!: number;
  @Input() user!: IUserEntryModel;
  @Input() availableRoles!: Array<UserRole>;
  @Input() updateUserRolesCallback!: (id: number, roles: Array<UserRole>) => Promise<any>;

  protected selectedRoles: Array<UserRole> = [];

  protected hasRole(role: UserRole): boolean {
    return this.selectedRoles.includes(role);
  }

  protected toggleRoleSelection(role: UserRole) {
    if (this.user.user.id == this.currentUserId
        && role == UserRole.Admin && this.hasRole(role)) {

      this.notificationService.warning("DropAdminRoleProhibitedWarning");
      return;
    }

    if (this.selectedRoles.includes(role)) {
      this.selectedRoles = this.selectedRoles.filter(r => r !== role);
    } else {
      this.selectedRoles.push(role);
    }
  }

  protected async saveChanges() {
    await this.updateUserRolesCallback(this.user.user.id, this.selectedRoles);
    this.bsModalRef.hide();
  }

  constructor(public bsModalRef: BsModalRef,
    private notificationService: PopupNotificationService) {
  }

  async ngOnInit() {
    this.selectedRoles = this.user.roles.slice();
  }
}
