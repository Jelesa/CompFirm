import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RequestListComponent } from './request-list.component';
import { LoaderModule } from '../loader/loader.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    RequestListComponent
  ],
  imports: [
    CommonModule,
    LoaderModule,
    NgSelectModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    RequestListComponent
  ]
})
export class RequestListModule { }
