import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PersonsJournalComponent } from "./user/persons/persons-journal/persons-journal.component";
import { ConfigurationsJournalComponent } from "./user/configuration/configurations-journal/configurations-journal.component";

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'persons-journal',
  }, {
    path: 'persons-journal',
    component: PersonsJournalComponent,
  }, {
    path: 'configurations-journal',
    component: ConfigurationsJournalComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
