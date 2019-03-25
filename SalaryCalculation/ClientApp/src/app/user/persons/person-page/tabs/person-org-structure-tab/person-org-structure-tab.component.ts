import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { combineLatest, Observable } from "rxjs/index";
import { ComboBoxItemDTO } from "../../../../../shared/models/combobox-item-dto";
import { PersonPageService } from "../../person-page.service";
import { filter, map, shareReplay, withLatestFrom } from "rxjs/internal/operators";
import { CheckUtils } from "../../../../../utils/check-utils";
import { Subscriber } from "../../../../../shared/subscriber";
import { OrgStructureItem } from "../../../models/org-structure-item";
import { NestedTreeControl } from "@angular/cdk/tree";
import { MatTreeNestedDataSource } from "@angular/material";

@Component({
  selector: 'app-person-org-structure-tab',
  templateUrl: './person-org-structure-tab.component.html',
  styleUrls: ['./person-org-structure-tab.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PersonOrgStructureTabComponent extends Subscriber implements OnInit {

  hasCurrentGroup$: Observable<boolean>;
  currentChiefId$: Observable<number>;

  chiefs$: Observable<ComboBoxItemDTO[]>;
  selectedChief: ComboBoxItemDTO;

  personId$: Observable<number>;
  treeControl = new NestedTreeControl<OrgStructureItem>(node => node.children);
  dataSource = new MatTreeNestedDataSource<OrgStructureItem>();

  hasChild = (_: number, node: OrgStructureItem) => !!node.children && node.children.length > 0;

  constructor(private service: PersonPageService) {
    super();
  }

  ngOnInit() {
    const currentGroup$ = this.service.person$.pipe(
        filter(p => CheckUtils.isExists(p.currentGroup)),
        map(p => p.currentGroup.name),
    );
    this.hasCurrentGroup$ = currentGroup$.pipe(map(CheckUtils.isExists));
    this.currentChiefId$ = this.service.person$.pipe(
        map(p => CheckUtils.isExists(p.currentChief) ? p.currentChief.id : null),
        shareReplay(1),
    );

    this.chiefs$ = combineLatest(
        this.service.getPossibleChiefs(),
        this.service.id$,
    ).pipe(
        map(([chiefs, id]) => chiefs.filter(dto => dto.id !== id)),
        shareReplay(1),
    );

    this.personId$ = this.service.person$.pipe(map(p => p.id));

    const updateSelectedChiefSubscription = this.chiefs$.pipe(withLatestFrom(this.currentChiefId$))
        .subscribe(([chiefs, id]) => this.selectedChief = chiefs.find(val => val.id === id));
    this.subscribed(updateSelectedChiefSubscription);

    const childrenSubscription = this.service.person$
        .pipe(map(p => p.children))
        .subscribe(children => this.dataSource.data = new Array(children));
    this.subscribed(childrenSubscription);
  }

  updateChief() {
    const newChiefId: number = CheckUtils.isExists(this.selectedChief) ? this.selectedChief.id : 0;
    this.service.updateChief(newChiefId);
  }

}
