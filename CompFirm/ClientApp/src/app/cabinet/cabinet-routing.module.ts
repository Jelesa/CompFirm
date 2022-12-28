import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CabinetComponent } from './cabinet.component';
import { RequestCardResolver } from './request-card/request-card-resolver.service';
import { RequestCardComponent } from './request-card/request-card.component';
import { CancelRequestComponent } from "../shared/components/cancel-request/cancel-request.component";
import { EditUserCardComponent } from './edit-user-card/edit-user-card.component';
import { CabinetHomeComponent } from './cabinet-home/cabinet-home.component';

const routes: Routes = [
  {
    path: '',
    component: CabinetComponent,
    children: [
      {
        path: '',
        component: CabinetHomeComponent
      },
      {
        path: 'request-list',
        component: CabinetHomeComponent
      },
      {
        path: 'request-card/:id',
        component: RequestCardComponent,
        resolve: {
          request: RequestCardResolver
        }
      },
      {
        path: 'request-card/cancel/:id',
        component: CancelRequestComponent,
        resolve: {
          request: RequestCardResolver
        }
      },
      {
        path: 'edit-user-card',
        component: EditUserCardComponent
      }
    ]
  }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ],
  bootstrap: [
    CabinetComponent
  ]
})
export class CabinetRoutingModule { }
