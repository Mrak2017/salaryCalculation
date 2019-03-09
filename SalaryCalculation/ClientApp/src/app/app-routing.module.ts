import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { CounterComponent } from "./counter/counter.component";
import { HomeComponent } from "./home/home.component";
import { FetchDataComponent } from "./fetch-data/fetch-data.component";
import { PersonsJournalComponent } from "./person/persons-journal/persons-journal.component";

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
  }, {
    path: 'counter',
    component: CounterComponent,
  }, {
    path: 'fetch-data',
    component: FetchDataComponent,
  }, {
    path: 'persons',
    component: PersonsJournalComponent,
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
