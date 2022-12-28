import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing.module';
import { ItemCardComponent } from './item-card/item-card.component';
import { FormsModule } from '@angular/forms';
import { LoaderModule } from '../shared/components/loader/loader.module';

@NgModule({
  declarations: [
    HomeComponent,
    ItemCardComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    HomeRoutingModule,
    LoaderModule
  ]
})
export class HomeModule { }
