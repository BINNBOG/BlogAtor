import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { SimpleTableColumn } from '../../../framework/table-page/table-column/simple-column';
import { UsersAPIService } from '../../../../service/API/users-api.service';
import { ISearchResult } from '../../../../model/search/ISearchResult';
import { IUserEntryModel } from '../../../../model/IUserEntryModel';
import { TemplateTableColumn } from '../../../framework/table-page/table-column/template-column';
import { AuthenticationService } from '../../../../service/authentication.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { IUser, UserRole } from '../../../../model/IUser';
import { EditUserModalComponent } from './edit-user-modal/edit-user-modal.component';
import { TablePageComponent } from '../../../framework/table-page/table-page.component';

@Component({
  selector: 'app-users',
  standalone: false,
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  protected that = this;

  @ViewChild('rolesColumn') rolesColumn!: TemplateRef<any>;
    private rolesColumnGetter = () => this.rolesColumn;
  @ViewChild('actionsColumn') actionsColumn!: TemplateRef<any>;
    private actionsColumnGetter = () => this.actionsColumn;

  protected columns = [
    new SimpleTableColumn("Id", "user.id", {
      sortable: true,
      searchable: false,
      width: "5%"
    }),
    new SimpleTableColumn("Username", "user.username", {
      sortable: true,
      searchable: false,
      width: "15%"
    }),
    new TemplateTableColumn("Roles", this.rolesColumnGetter.bind(this), null, "roles", {
      sortable: false,
      searchable: false,
      width: "60%"
    }),
    new TemplateTableColumn("Actions", this.actionsColumnGetter.bind(this), null, "user.id", {
      searchable: false,
      sortable: false,
      width: "20%"
    })
  ];

  protected editUserModal?: BsModalRef;
  private availableRoles: Array<UserRole> = [];

  @ViewChild(TablePageComponent, { static: false }) table!: TablePageComponent;

  constructor(private usersApiService: UsersAPIService,
    protected authenticationService: AuthenticationService,
    private modalService: BsModalService) {
  }

  protected currentUserId: number | null = null;
  
  async ngOnInit() {
    const currentUser = await this.authenticationService.getMeInfo();
    this.availableRoles = await this.usersApiService.getAvailableRoles();

    this.currentUserId = currentUser?.me.id ?? null;
  }

  protected async elementsGetter(params: any): Promise<ISearchResult<IUserEntryModel>> {
    return await this.usersApiService.searchUsers(params);
  }

  protected async elementDeletter(user: IUser) {
    await this.usersApiService.deleteUser(user);
    await this.table.reload();
  }

  protected onEditUserClick(userEntry: IUserEntryModel) {
    const config: ModalOptions = {
      initialState: {
        currentUserId: this.currentUserId,
        user: userEntry,
        availableRoles: this.availableRoles,
        updateUserRolesCallback: this.updateUserRoles.bind(this),
      }
    };
    this.editUserModal = this.modalService.show(EditUserModalComponent, config);
  }

  private async updateUserRoles(id: number, roles: Array<UserRole>) {
    await this.usersApiService.editUserRoles(id, roles);
    await this.table.reload();
  }
}
