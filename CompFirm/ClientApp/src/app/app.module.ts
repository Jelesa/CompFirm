import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { AppRoutingModule } from './app-routing.module';
import { UserMenuComponent } from './nav-menu/user-menu/user-menu.component';
import { AuthHttpInterceptorService } from './core/auth-http-interceptor.service';
import { CartMenuComponent } from './nav-menu/cart-menu/cart-menu.component';
import { ErrorWrapperComponent } from "./error-wrapper/error-wrapper.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    UserMenuComponent,
    CartMenuComponent,
    ErrorWrapperComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptorService, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
