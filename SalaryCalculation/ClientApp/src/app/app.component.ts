import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {

  sideNavItems = [{
    name: 'Сотрудники',
    link: 'persons-journal',
  }, {
    name: 'Настройки',
    link: 'configurations-journal',
  }];
}
