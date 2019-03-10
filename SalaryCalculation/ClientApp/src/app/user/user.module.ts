import { NgModule } from '@angular/core';
import { PersonsJournalComponent } from './persons/persons-journal/persons-journal.component';
import { SharedModule } from "../shared/shared.module";
import { PersonsMainService } from "./persons/persons-main.service";
import { AddPersonDialogComponent } from './persons/add-person-dialog/add-person-dialog.component';
import { ReactiveFormsModule } from "@angular/forms";

@NgModule({
  imports: [
    SharedModule,
    ReactiveFormsModule,
  ],
  declarations: [
    PersonsJournalComponent,
    AddPersonDialogComponent,
  ],
  exports: [
    PersonsJournalComponent,
  ],
  providers: [
    PersonsMainService,
  ],
  entryComponents: [
    AddPersonDialogComponent
  ]
})
export class UserModule {
}
