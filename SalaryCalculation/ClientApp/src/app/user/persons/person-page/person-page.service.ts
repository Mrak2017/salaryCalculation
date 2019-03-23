import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { Person } from "../models/person.model";
import { merge, Subject } from "rxjs/index";
import { PersonsMainService } from "../persons-main.service";
import { map, shareReplay, switchMap, take, withLatestFrom } from "rxjs/internal/operators";
import { ActivatedRoute } from "@angular/router";
import { ComboBoxItemDTO } from "../../../shared/models/combobox-item-dto";
import { PersonGroup } from "../models/person-group.model";

@Injectable()
export class PersonPageService {

  public readonly person$: Observable<Person>;
  public readonly id$: Observable<number>;

  private readonly refreshSubj: Subject<void> = new Subject<void>();

  constructor(private service: PersonsMainService,
              private route: ActivatedRoute) {
    const refreshed$ = this.refreshSubj.asObservable().pipe(
        withLatestFrom(this.route.params.pipe(map(p => p.id))),
        switchMap(([_, id]) => this.service.getPerson(id)),
    );

    const initial$ = this.route.params.pipe(
        map(p => p.id),
        switchMap(id => this.service.getPerson(id)),
    );

    this.person$ = merge(initial$, refreshed$)
        .pipe(shareReplay(1));

    this.id$ = this.person$.pipe(
        map(p => p.id),
        shareReplay(1));
  }

  updatePerson(person: Person) {
    this.service.updatePerson(person)
        .toPromise()
        .then(() => this.refresh());
  }

  getPossibleChiefs(): Observable<ComboBoxItemDTO[]> {
    return this.service.getPossibleChiefs();
  }

  updateChief(newChiefId: number) {
    this.person$.pipe(
        map(p => p.id),
        take(1),
    )
        .toPromise()
        .then((personId) => this.service.updateChief(personId, newChiefId).toPromise())
        .then(() => this.refresh());
  }

  addGroup() {
    this.person$.pipe(
        map(p => p.id),
        take(1),
    )
        .toPromise()
        .then((personId) => this.service.addGroup(personId))
        .then(() => this.refresh());
  }

  editGroup(id: number) {
    this.service.updateGroup(id).then(() => this.refresh());
  }

  deleteGroup(group: PersonGroup) {
    this.service.deleteGroup(group).then(() => this.refresh());
  }

  private refresh() {
    this.refreshSubj.next();
  }
}
