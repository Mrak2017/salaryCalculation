import { Inject, Injectable } from '@angular/core';
import { merge, Observable, Subject } from "rxjs/index";
import { HttpClient } from "@angular/common/http";
import { filter, map, shareReplay, switchMap, take } from "rxjs/internal/operators";
import { CheckUtils } from "../../utils/check-utils";
import { MatDialog } from "@angular/material";
import { ConfigurationItem } from "./models/configuration-item.model";
import { Configuration } from "./models/configuration.model";
import { AddConfigurationDialogComponent } from "./add-configuration-dialog/add-configuration-dialog.component";

@Injectable()
export class ConfigurationsMainService {

  public readonly allConfigs$: Observable<ConfigurationItem[]>;

  private readonly refreshSubj: Subject<void> = new Subject<void>();

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private baseUrl: string,
              private dialog: MatDialog) {

    const refreshed$ = this.refreshSubj.asObservable().pipe(
        switchMap(() => this.getAllConfigs()),
    );

    const initial$ = this.getAllConfigs();

    this.allConfigs$ = merge(initial$, refreshed$).pipe(shareReplay(1));
  }

  refresh() {
    this.refreshSubj.next();
  }

  addConfig() {
    const dialogRef = this.dialog.open(AddConfigurationDialogComponent, {
      width: '400px',
      disableClose: true,
    });

    dialogRef.afterClosed()
        .pipe(
            filter(CheckUtils.isExists),
            map((result: Configuration) => (
                {
                  code: result.code,
                  value: result.configValue,
                  description: result.description
                })),
            switchMap(dto => this.http.post(this.restUrl() + 'AddConfig', dto)),
            take(1))
        .toPromise()
        .then(() => this.refresh());
  }

  private getAllConfigs(): Observable<ConfigurationItem[]> {
    return this.http.get<ConfigurationItem[]>(this.restUrl() + 'AllConfigs')
        .pipe(
            map(data => data.map(value => new ConfigurationItem(value))),
        )
  }

  private restUrl(): string {
    return this.baseUrl + 'api/configuration/';
  }
}
