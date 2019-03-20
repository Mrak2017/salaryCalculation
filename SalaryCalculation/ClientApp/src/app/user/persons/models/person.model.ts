import { GroupTypeEnum } from "./group-type-enum";
import { CheckUtils } from "../../../utils/check-utils";

export class Person {
  public static readonly NAME_MAX_LENGTH = 100;

  public id: number;
  public login: string;
  public password: string;
  public firstName: string;
  public middleName: string;
  public lastName: string;
  public startDate: Date;
  public endDate: Date;
  public currentGroup: GroupTypeEnum;
  public baseSalaryPart: number;
  public currentChief: Person;

  constructor(data: any) {
    if (CheckUtils.isExists(data)) {
      this.id = data.id;
      this.login = data.login;
      this.password = data.password;
      this.firstName = data.firstName;
      this.middleName = data.middleName;
      this.lastName = data.lastName;
      this.startDate = new Date(data.startDate);
      this.endDate = CheckUtils.isExists(data.endDate) ? new Date(data.endDate) : null;
      if (CheckUtils.isExists(data.currentGroup)) {
        this.currentGroup = GroupTypeEnum.valueOf(data.currentGroup);
      }
      this.baseSalaryPart = data.baseSalaryPart;
      if(CheckUtils.isExists(data.currentChief)) {
        this.currentChief = new Person(data.currentChief);
      }
    }
  }
}
