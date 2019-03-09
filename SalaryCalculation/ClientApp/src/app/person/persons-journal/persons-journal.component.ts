import { Component, OnInit } from '@angular/core';
import { PersonsService } from "../persons.service";
import { Observable } from "rxjs/Observable";
import { Person } from "../models/person";


@Component({
  selector: 'app-persons-journal',
  templateUrl: './persons-journal.component.html',
  styleUrls: ['./persons-journal.component.css'],
})
export class PersonsJournalComponent implements OnInit {

  persons$: Observable<Person[]>;

  constructor(private service: PersonsService) {
  }

  ngOnInit() {
    this.persons$ = this.service.getAllPersons();
  }

  createPerson() {

  }

  deletePerson(id: number) {
    this.service.deletePerson(id);
  }
}
