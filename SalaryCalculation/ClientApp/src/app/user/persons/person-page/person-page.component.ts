import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { PersonPageService } from "./person-page.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { combineLatest, Observable } from "rxjs";
import { filter, map, shareReplay, take, tap, withLatestFrom } from "rxjs/internal/operators";
import { Subscriber } from "../../../shared/subscriber";
import { Person } from "../models/person.model";
import { ComboBoxItemDTO } from "../../../shared/models/combobox-item-dto";
import { CheckUtils } from "../../../utils/check-utils";

@Component({
  selector: 'app-person-page',
  templateUrl: './person-page.component.html',
  styleUrls: ['./person-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [PersonPageService],
})
export class PersonPageComponent extends Subscriber implements OnInit {

  personForm: FormGroup;
  login$: Observable<string>;
  currentGroup$: Observable<string>;
  currentChiefId$: Observable<number>;

  chiefs$: Observable<ComboBoxItemDTO[]>;
  selectedChief: ComboBoxItemDTO;

  constructor(private service: PersonPageService,
              private fb: FormBuilder) {
    super();
  }

  ngOnInit() {
    this.personForm = this.fb.group({
      password: ['', [
        Validators.required,
        Validators.maxLength(Person.NAME_MAX_LENGTH)]],
      lastName: ['', [
        Validators.required,
        Validators.maxLength(Person.NAME_MAX_LENGTH)]],
      firstName: ['', [
        Validators.required,
        Validators.maxLength(Person.NAME_MAX_LENGTH)]],
      middleName: [''],
      startDate: ['', [Validators.required]],
      endDate: [''],
      baseSalaryPart: ['', []],
    });

    this.login$ = this.service.person$.pipe(map(p => p.login));
    this.currentGroup$ = this.service.person$.pipe(
        filter(p => CheckUtils.isExists(p.currentGroup)),
        map(p => p.currentGroup.name),
    );
    this.currentChiefId$ = this.service.person$.pipe(
        map(p => CheckUtils.isExists(p.currentChief) ? p.currentChief.id : null),
        shareReplay(1),
    );

    const fillFormSubscription = this.service.person$.subscribe(person => this.fillForm(person));
    this.subscribed(fillFormSubscription);

    this.chiefs$ = combineLatest(
        this.service.getPossibleChiefs(),
        this.service.id$,
    ).pipe(
        map(([chiefs, id]) => chiefs.filter(dto => dto.id !== id)),
        shareReplay(1),
    );

    const updateSelectedChiefSubscription = this.chiefs$.pipe(withLatestFrom(this.currentChiefId$))
        .subscribe(([chiefs, id]) => this.selectedChief = chiefs.find(val => val.id === id));
    this.subscribed(updateSelectedChiefSubscription);
  }

  onSubmit() {
    this.service.person$.pipe(
        map(p => this.fillOnSave(p)),
        take(1),
    )
        .toPromise()
        .then(p => this.service.updatePerson(p));
  }

  updateChief() {
    const newChiefId: number = CheckUtils.isExists(this.selectedChief) ? this.selectedChief.id : 0;
    this.service.updateChief(newChiefId);
  }

  private fillForm(person: Person) {
    this.personForm.controls.password.setValue(person.password);
    this.personForm.controls.lastName.setValue(person.lastName);
    this.personForm.controls.firstName.setValue(person.firstName);
    this.personForm.controls.middleName.setValue(person.middleName);
    this.personForm.controls.startDate.setValue(person.startDate);
    this.personForm.controls.endDate.setValue(person.endDate);
    this.personForm.controls.baseSalaryPart.setValue(person.baseSalaryPart);
  }

  private fillOnSave(person: Person): Person {
    let clone = Object.assign({}, person);
    clone.password = this.personForm.value.password;
    clone.lastName = this.personForm.value.lastName;
    clone.firstName = this.personForm.value.firstName;
    clone.middleName = this.personForm.value.middleName;
    clone.startDate = this.personForm.value.startDate;
    clone.endDate = this.personForm.value.endDate;
    clone.baseSalaryPart = this.personForm.value.baseSalaryPart;
    return clone;
  }
}
