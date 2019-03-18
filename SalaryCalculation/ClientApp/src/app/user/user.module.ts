import { NgModule } from '@angular/core';
import { PersonsJournalComponent } from './persons/persons-journal/persons-journal.component';
import { SharedModule } from "../shared/shared.module";
import { PersonsMainService } from "./persons/persons-main.service";
import { AddPersonDialogComponent } from './persons/add-person-dialog/add-person-dialog.component';
import { ReactiveFormsModule } from "@angular/forms";
import { ConfigurationsJournalComponent } from './configuration/configurations-journal/configurations-journal.component';
import { ConfigurationsMainService } from "./configuration/configurations-main.service";
import { EditConfigurationDialogComponent } from './configuration/edit-configuration-dialog/edit-configuration-dialog.component';
import { PersonPageComponent } from './persons/person-page/person-page.component';

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
  ],
  declarations: [
    PersonsJournalComponent,
    AddPersonDialogComponent,
    ConfigurationsJournalComponent,
    EditConfigurationDialogComponent,
    PersonPageComponent,
  ],
  exports: [
    PersonsJournalComponent,
    ConfigurationsJournalComponent,
  ],
  providers: [
    PersonsMainService,
    ConfigurationsMainService,
  ],
  entryComponents: [
    AddPersonDialogComponent,
    EditConfigurationDialogComponent,
  ],
})
export class UserModule {
}
