import { Component, OnInit } from '@angular/core';
import { Observable } from "rxjs/index";
import { PersonsMainService } from "../persons-main.service";
import { PersonItem } from "../models/person-item.model";

@Component({
  selector: 'app-persons-journal',
  templateUrl: './persons-journal.component.html',
  styleUrls: ['./persons-journal.component.css'],
})
export class PersonsJournalComponent implements OnInit {

  displayedColumns: string[] = [
    'id',
    'login',
    'fullName',
    'startDate',
    'currentGroup',
    'baseSalaryPart',
    'currentSalary',
    'editColumn',
  ];

  persons$: Observable<PersonItem[]>;

  constructor(private service: PersonsMainService) {
  }

  ngOnInit() {
    this.persons$ = this.service.allPersons$;
  }

  addPerson() {
    this.service.addPerson();
  }

  applyFilter(search: string) {
    this.service.refresh(search);
  }

}
