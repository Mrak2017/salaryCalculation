import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from "./shared/shared.module";
import { UserModule } from "./user/user.module";
import { HttpClientModule } from "@angular/common/http";
import { MAT_DATE_LOCALE } from "@angular/material";

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    AppRoutingModule,
    SharedModule,
    UserModule,
  ],
  providers: [{provide: MAT_DATE_LOCALE, useValue: 'ru-Ru'}],
  bootstrap: [AppComponent],
})
export class AppModule {
}
