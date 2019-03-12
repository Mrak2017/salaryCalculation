import { Inject, Injectable, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable, ReplaySubject } from "rxjs/index";
import { map } from "rxjs/internal/operators";
import { Person } from "./models/person.model";
import { MatDialog } from "@angular/material";
import { AddPersonDialogComponent } from "./add-person-dialog/add-person-dialog.component";
import { Subscriber } from "../../shared/subscriber";

@Injectable()
export class PersonsMainService extends Subscriber implements OnInit {

  private allPersonsSubject$ = new ReplaySubject<Person[]>(1);
  allPersons$: Observable<Person[]> = this.allPersonsSubject$.asObservable();

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private baseUrl: string,
              private dialog: MatDialog) {
    super();
  }

  ngOnInit(): void {

  }

  refreshAllPersons() {
    const subscribed = this.http.get<Person[]>(this.restUrl() + 'AllPersons')
        .pipe(
            map(data => data.map(value => new Person(value))),
        ).subscribe(value => this.allPersonsSubject$.next(value));

    this.subscribed(subscribed);
  }

  deletePerson(id: number): void {
    //this.http.delete(this.restUrl() + id);
  }

  addPerson() {
    const dialogRef = this.dialog.open(AddPersonDialogComponent, {
      width: '400px',
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((result: Person) => {
      let dto = {
        login: result.login,
        password: result.password,
        firstName: result.firstName,
        middleName: result.middleName,
        lastName: result.lastName,
        startDate: result.startDate,
        currentGroup: result.currentGroup.code,
        baseSalaryPart: result.baseSalaryPart,
      };
      this.http.post(this.restUrl() + 'AddPerson', dto).subscribe(
          () => this.refreshAllPersons(),
      )
    });
  }

  private restUrl(): string {
    return this.baseUrl + 'api/persons/';
  }
}
