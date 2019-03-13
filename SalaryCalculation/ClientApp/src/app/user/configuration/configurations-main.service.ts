import { Inject, Injectable } from '@angular/core';
import { merge, Observable, Subject } from "rxjs/index";
import { Configuration } from "./models/configuration.model";
import { HttpClient } from "@angular/common/http";
import { map, shareReplay } from "rxjs/internal/operators";

@Injectable()
export class ConfigurationsMainService {

  public readonly allConfigs$: Observable<Configuration[]>;

  private readonly refreshSubj: Subject<void> = new Subject<void>();

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private baseUrl: string) {

    const refreshed$ = this.refreshSubj.asObservable().pipe(
        map(() => this.getAllConfigs()),
    );

    const initial$ = this.getAllConfigs();

    this.allConfigs$ = merge(initial$, refreshed$).pipe(shareReplay(1));
  }

  refresh() {
    this.refreshSubj.next();
  }

  private getAllConfigs(): Observable<Configuration[]> {
    return this.http.get<Configuration[]>(this.restUrl() + 'AllConfigs')
        .pipe(
            map(data => data.map(value => new Configuration(value))),
        )
  }

  private restUrl(): string {
    return this.baseUrl + 'api/configuration/';
  }
}
