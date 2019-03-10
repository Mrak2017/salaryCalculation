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

  displayedColumns: string[] = ['id', 'fullName', 'startDate', 'currentGroup'];
  persons: Observable<Person[]>;

  constructor(private service: PersonsMainService) {
  }

  ngOnInit() {
    this.persons = this.service.getAllPersons();
  }

  addPerson() {
    this.service.addPerson();
  }

  deletePerson(id: number) {
    this.service.deletePerson(id);
  }


}
