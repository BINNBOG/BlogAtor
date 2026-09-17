import { importProvidersFrom, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClient, provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MainComponent } from './component/main/main.component';
import { RouterLink } from '@angular/router';
import { DataSourcesComponent } from './component/main/pages/data-sources/data-sources.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AddModalComponent } from './component/main/pages/data-sources/add-modal/add-modal.component';
import { CollectorsComponent } from './component/main/pages/data-sources/collectors/collectors.component';
import { TranslateLoader, TranslateModule, TranslatePipe } from '@ngx-translate/core'
import { TranslateHttpLoader } from '@ngx-translate/http-loader'
import { AddSourceCollectorModalComponent } from './component/main/pages/data-sources/collectors/add-modal/add-source-collector-modal.component';
import { DataItemsListComponent } from './component/main/pages/data-items-list/data-items-list.component';
import { HttpProxiesComponent } from './component/main/pages/http-proxies/http-proxies.component';
import { AddHttpProxyConfigModalComponent } from './component/main/pages/http-proxies/add-modal/add-modal.component';
import { TablePageComponent } from './component/framework/table-page/table-page.component';
import { InfoPageComponent } from './component/framework/info-page/info-page.component';
import { DataTablesModule } from 'angular-datatables';
import { DatePipe } from '@angular/common';
import { ImportDataSourceModalComponent } from './component/main/pages/data-sources/import-data-source-modal/import-data-source-modal.component';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { authenticationInterceptor } from './service/API/interceptors/authenticate-interceptor';
import { AccountMenuComponent } from './component/main/account-menu/account-menu.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { UsersComponent } from './component/main/pages/users/users.component';
import { EditUserModalComponent } from './component/main/pages/users/edit-user-modal/edit-user-modal.component';
import { AuthorizedComponent } from './component/framework/authorized/authorized.component';

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    DataSourcesComponent,
    AddModalComponent,
    CollectorsComponent,
    AddSourceCollectorModalComponent,
    DataItemsListComponent,
    HttpProxiesComponent,
    AddHttpProxyConfigModalComponent,
    TablePageComponent,
    InfoPageComponent,
    ImportDataSourceModalComponent,
    LoginComponent,
    RegisterComponent,
    AccountMenuComponent,
    UsersComponent,
    EditUserModalComponent,
    AuthorizedComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    RouterLink,
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
    TranslateModule,
    DataTablesModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot(),
  ],
  providers: [
    provideHttpClient(
      withInterceptors([authenticationInterceptor]),
    ),
    DatePipe, TranslatePipe,
    importProvidersFrom([TranslateModule.forRoot({
      defaultLanguage: 'ru',
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient]
      },
    })])
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
