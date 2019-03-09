import { Inject, Injectable } from '@angular/core';
import { Observable } from "rxjs/Observable";
import { Person } from "./models/person";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";

@Injectable()
export class PersonsService {

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private baseUrl: string) {
  }

  private restUrl(): string {
    return this.baseUrl + 'api/persons/';
  }

  getAllPersons(): Observable<Person[]> {
    return this.http.get<Person[]>(this.restUrl() + 'AllPersons')
                    .pipe(map(data => data.map(value => new Person(value))));
  }

  deletePerson(id: number): void {
    this.http.delete(this.restUrl() + id);
  }

  createPerson() {

  }
}
