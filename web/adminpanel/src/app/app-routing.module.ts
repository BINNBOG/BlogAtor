import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './component/main/main.component';
import { DataSourcesComponent } from './component/main/pages/data-sources/data-sources.component';
import { CollectorsComponent } from './component/main/pages/data-sources/collectors/collectors.component';
import { DataItemsListComponent } from './component/main/pages/data-items-list/data-items-list.component';
import { HttpProxiesComponent } from './component/main/pages/http-proxies/http-proxies.component';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { UsersComponent } from './component/main/pages/users/users.component';

const routes: Routes = [
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "register",
    component: RegisterComponent
  },
  {
    path: "",
    component: MainComponent,
    children: [
      {
        path: "datasources",
        component: DataSourcesComponent,
      },
      {
        path: "datasource/:id/collectors",
        component: CollectorsComponent
      },
      {
        path: "dataitems",
        component: DataItemsListComponent
      },
      {
        path: "users",
        component: UsersComponent
      },
      {
        path: "proxies",
        component: HttpProxiesComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
