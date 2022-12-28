import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoaderModule } from "../shared/components/loader/loader.module";
import { AdministrationComponent } from './administration.component';
import { AdministrationRoutingModule } from './administration-routing.module';
import { MenuComponent } from './menu/menu.component';
import { ProductListComponent } from './product-list/product-list.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { RequestListModule } from '../shared/components/request-list/request-list.module';
import { AdminRequestCardComponent } from './admin-request-card/admin-request-card.component';
import { EditProductComponent } from './product-list/edit-product/edit-product.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { ManufacturerListComponent } from './manufacturer-list/manufacturer-list.component';
import { GroupListComponent } from './group-list/group-list.component';
import { EditGroupComponent } from './group-list/edit-group/edit-group.component';
import { AddGroupComponent } from './group-list/add-group/add-group.component';
import { CreateProductComponent } from "./product-list/create-product/create-product.component";
import { ProductsReceiptComponent } from './products-moving/products-receipt/products-receipt.component';
import { AddReceiptComponent } from './products-moving/products-receipt/add-receipt/add-receipt.component';
import { ProductsRefundComponent } from "./products-moving/products-refund/products-refund.component";
import { AddRefundComponent } from "./products-moving/products-refund/add-refund/add-refund.component";
import { ProductsGivingComponent } from "./products-moving/products-giving/products-giving.component";
import { CharacteristicsListComponent } from "./characteristics-list/characteristics-list.component";
import { ReportsComponent } from './reports/reports.component';

@NgModule({
  declarations: [
    AdministrationComponent,
    MenuComponent,
    ProductListComponent,
    AdminHomeComponent,
    AdminRequestCardComponent,
    CreateProductComponent,
    EditProductComponent,
    ManufacturerListComponent,
    GroupListComponent,
    EditGroupComponent,
    AddGroupComponent,
    ProductsReceiptComponent,
    AddReceiptComponent,
    ProductsRefundComponent,
    AddRefundComponent,
    ProductsGivingComponent,
    CharacteristicsListComponent,
    ReportsComponent
  ],
  imports: [
    CommonModule,
    AdministrationRoutingModule,
    FormsModule,
    NgSelectModule,
    ReactiveFormsModule,
    LoaderModule,
    RequestListModule
  ]
})
export class AdministrationModule { }
