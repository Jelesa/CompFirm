import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CabinetComponent } from './cabinet.component';
import { CabinetRoutingModule } from './cabinet-routing.module';
import { RequestCardComponent } from './request-card/request-card.component';
import { UserCardComponent } from './user-card/user-card.component';
import { CancelRequestComponent } from "../shared/components/cancel-request/cancel-request.component";
import { EditUserCardComponent } from './edit-user-card/edit-user-card.component';
import { CabinetHomeComponent } from './cabinet-home/cabinet-home.component';
import { LoaderModule } from '../shared/components/loader/loader.module';
import { RequestListModule } from '../shared/components/request-list/request-list.module';

@NgModule({
  declarations: [
    CabinetComponent,
    RequestCardComponent,
    UserCardComponent,
    EditUserCardComponent,
    CancelRequestComponent,
    CabinetHomeComponent
  ],
  imports: [
    CommonModule,
    CabinetRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    LoaderModule,
    RequestListModule
  ]
})
export class CabinetModule { }
