import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { merge, Observable, Subject } from "rxjs/index";
import { filter, map, shareReplay, switchMap, take } from "rxjs/internal/operators";
import { MatDialog } from "@angular/material";
import { AddPersonDialogComponent } from "./add-person-dialog/add-person-dialog.component";
import { CheckUtils } from "../../utils/check-utils";
import { PersonItem } from "./models/person-item.model";
import { Person } from "./models/person.model";

@Injectable()
export class PersonsMainService {

  public static readonly NAME_MAX_LENGTH = 100;

  public readonly allPersons$: Observable<PersonItem[]>;

  private readonly refreshSubj: Subject<void> = new Subject<void>();

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private baseUrl: string,
              private dialog: MatDialog) {
    const refreshed$ = this.refreshSubj.asObservable().pipe(
        switchMap(() => this.getAllPersons()),
    );

    const initial$ = this.getAllPersons();

    this.allPersons$ = merge(initial$, refreshed$).pipe(shareReplay(1));
  }

  refresh() {
    this.refreshSubj.next();
  }

  private getAllPersons(): Observable<PersonItem[]> {
    return this.http.get<PersonItem[]>(this.restUrl() + 'AllPersons')
        .pipe(
            map(data => data.map(value => new PersonItem(value))),
        )
  }

  deletePerson(id: number): void {
    //this.http.delete(this.restUrl() + id);
  }

  addPerson() {
    const dialogRef = this.dialog.open(AddPersonDialogComponent, {
      width: '400px',
      disableClose: true,
    });

    dialogRef.afterClosed()
        .pipe(
            filter(CheckUtils.isExists),
            map((result: Person) => (
                {
                  login: result.login,
                  password: result.password,
                  firstName: result.firstName,
                  middleName: result.middleName,
                  lastName: result.lastName,
                  startDate: result.startDate,
                  currentGroup: result.currentGroup.code,
                  baseSalaryPart: result.baseSalaryPart,
                })),
            switchMap(dto => this.http.post(this.restUrl() + 'AddPerson', dto)),
            take(1))
        .toPromise()
        .then(() => this.refresh());
  }

  getPerson(id: number): Observable<Person> {
    return this.http.get<Person>(this.restUrl() + 'GetPerson/' + id)
        .pipe(
            map(data => new Person(data)),
        );
  }

  updatePerson(person: Person) {
    this.http.put(this.restUrl() + 'UpdatePerson/' + person.id, person);
  }

  private restUrl(): string {
    return this.baseUrl + 'api/persons/';
  }
}
