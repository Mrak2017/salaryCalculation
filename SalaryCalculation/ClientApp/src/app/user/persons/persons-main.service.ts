import { Inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { merge, Observable, Subject } from "rxjs/index";
import { filter, map, shareReplay, switchMap, take } from "rxjs/internal/operators";
import { MatDialog } from "@angular/material";
import { AddPersonDialogComponent } from "./add-person-dialog/add-person-dialog.component";
import { CheckUtils } from "../../utils/check-utils";
import { PersonItem } from "./models/person-item.model";
import { Person } from "./models/person.model";
import { ComboBoxItemDTO } from "../../shared/models/combobox-item-dto";

@Injectable()
export class PersonsMainService {

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
            map(PersonsMainService.convertToDTO),
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

  updatePerson(person: Person): Observable<{}> {
    return this.http.put(this.restUrl() + 'UpdatePerson/' + person.id, PersonsMainService.convertToDTO(person));
  }

  getPossibleChiefs():Observable<ComboBoxItemDTO[]> {
    return this.http.get<any[]>(this.restUrl() + 'GetPossibleChiefs')
        .pipe(
            map(data => data.map(value => new ComboBoxItemDTO(value.id, value.name))),
        )
  }

  private getAllPersons(): Observable<PersonItem[]> {
    return this.http.get<PersonItem[]>(this.restUrl() + 'GetAllPersons')
        .pipe(
            map(data => data.map(value => new PersonItem(value))),
        )
  }

  private static convertToDTO(person: Person) {
    return {
      id: person.id,
      login: person.login,
      password: person.password,
      firstName: person.firstName,
      middleName: person.middleName,
      lastName: person.lastName,
      startDate: person.startDate,
      endDate: CheckUtils.isExists(person.endDate) ? person.endDate : null,
      currentGroup: CheckUtils.isExists(person.currentGroup) ? person.currentGroup.code : null,
      baseSalaryPart: person.baseSalaryPart,
    };
  }

  private restUrl(): string {
    return this.baseUrl + 'api/persons/';
  }
}
