import { Component, OnInit } from '@angular/core';
import { Observable } from "rxjs/index";
import { PersonsMainService } from "../persons-main.service";
import { PersonItem } from "../models/person-item.model";
import { Router } from "@angular/router";

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
    'editColumn'];

  persons$: Observable<PersonItem[]>;

  constructor(private service: PersonsMainService, private router: Router) {
  }

  ngOnInit() {
    this.persons$ = this.service.allPersons$;
  }

  addPerson() {
    this.service.addPerson();
  }

  editPerson(id: number) {
    this.router.navigate(['persons', id]);
  }

  deletePerson(id: number) {
    this.service.deletePerson(id);
  }


}
