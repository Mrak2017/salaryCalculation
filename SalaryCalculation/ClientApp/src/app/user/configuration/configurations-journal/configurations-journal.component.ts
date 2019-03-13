import { Component, OnInit } from '@angular/core';
import { Observable } from "rxjs/index";
import { ConfigurationsMainService } from "../configurations-main.service";
import { Configuration } from "../models/configuration.model";

@Component({
  selector: 'app-configurations-journal',
  templateUrl: './configurations-journal.component.html',
  styleUrls: ['./configurations-journal.component.css']
})
export class ConfigurationsJournalComponent implements OnInit {

  displayedColumns: (keyof Configuration)[] = [
    'id',
    'insertDate',
    'updateDate',
    'code',
    'value',
    'description'];

  configs$: Observable<Configuration[]>;

  constructor(private service: ConfigurationsMainService) {
  }

  ngOnInit() {
    this.configs$ = this.service.allConfigs$;
  }

  addConfig() {
    //this.service.addConfig();
  }

}
