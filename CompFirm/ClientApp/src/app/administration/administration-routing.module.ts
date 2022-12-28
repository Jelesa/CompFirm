import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { AdminRequestCardComponent } from './admin-request-card/admin-request-card.component';
import { AdministrationComponent } from './administration.component';
import { AddGroupComponent } from './group-list/add-group/add-group.component';
import { EditGroupComponent } from './group-list/edit-group/edit-group.component';
import { GroupListComponent } from './group-list/group-list.component';
import { ManufacturerListComponent } from './manufacturer-list/manufacturer-list.component';
import { EditProductComponent } from './product-list/edit-product/edit-product.component';
import { ProductListComponent } from './product-list/product-list.component';
import { CreateProductComponent } from "./product-list/create-product/create-product.component";
import { ProductsReceiptComponent } from "./products-moving/products-receipt/products-receipt.component";
import { AddReceiptComponent } from "./products-moving/products-receipt/add-receipt/add-receipt.component";
import { ProductsRefundComponent } from "./products-moving/products-refund/products-refund.component";
import { AddRefundComponent } from "./products-moving/products-refund/add-refund/add-refund.component";
import { ProductsGivingComponent } from "./products-moving/products-giving/products-giving.component";
import { CharacteristicsListComponent } from "./characteristics-list/characteristics-list.component";
import { ReportsComponent } from "./reports/reports.component";

const routes: Routes = [
  {
    path: '',
    component: AdministrationComponent,
    children: [
      {
        path: '',
        component: AdminHomeComponent
      },
      {
        path: 'request-list',
        component: AdminHomeComponent
      },
      {
        path: 'product-list',
        component: ProductListComponent
      },
      {
        path: 'admin-request-card/:id',
        component: AdminRequestCardComponent
      },
      {
        path: 'create-product',
        component: CreateProductComponent
      },
      {
        path: 'edit-product/:id',
        component: EditProductComponent
      },
      {
        path: 'group-list',
        component: GroupListComponent
      },
      {
        path: 'edit-group/:id',
        component: EditGroupComponent
      },
      {
        path: 'create-group',
        component: AddGroupComponent
      },
      {
        path: 'manufacturer-list',
        component: ManufacturerListComponent
      },
      {
        path: 'characteristic-list',
        component: CharacteristicsListComponent
      },
      {
        path: 'products-moving/receipts',
        component: ProductsReceiptComponent
      },
      {
        path: 'products-moving/receipts/add',
        component: AddReceiptComponent
      },
      {
        path: 'products-moving/refunds',
        component: ProductsRefundComponent
      },
      {
        path: 'products-moving/refunds/add',
        component: AddRefundComponent
      },
      {
        path: 'products-moving/givings',
        component: ProductsGivingComponent
      },
      {
        path: 'reports',
        component: ReportsComponent
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
    AdministrationComponent
  ]
})
export class AdministrationRoutingModule { }
