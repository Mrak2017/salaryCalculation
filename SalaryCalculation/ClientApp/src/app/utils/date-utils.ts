import * as moment from 'moment';
import { Moment } from "moment";

export class DateUtils {

  static getMoment(value: any): Moment {
    return moment(value).locale('ru');
  }

  static formatWithoutTimeZone(value: Date): String {
    return this.getMoment(value).format("YYYY-MM-DDTHH:mm:ss")
  }

  static formatDateOnly(value: Date): String {
    return this.getMoment(value).format("DD.MM.YYYY");
  }
}