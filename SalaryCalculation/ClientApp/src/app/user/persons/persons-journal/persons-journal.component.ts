import { Component, OnInit } from '@angular/core';
import { Person } from "../models/person.model";
import { Observable } from "rxjs/index";
import { PersonsMainService } from "../persons-main.service";

@Component({
  selector: 'app-persons-journal',
  templateUrl: './persons-journal.component.html',
  styleUrls: ['./persons-journal.component.css'],
})
export class PersonsJournalComponent implements OnInit {

  displayedColumns: (keyof Person)[] = [
    'id',
    'login',
    'fullName',
    'startDate',
    'currentGroup',
    'baseSalaryPart'];

  persons$: Observable<Person[]>;

  constructor(private service: PersonsMainService) {
  }

  ngOnInit() {
    this.persons$ = this.service.allPersons$;
    this.service.refreshAllPersons();
  }

  addPerson() {
    this.service.addPerson();
  }

  deletePerson(id: number) {
    this.service.deletePerson(id);
  }


}
