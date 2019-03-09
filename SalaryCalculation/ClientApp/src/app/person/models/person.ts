import { GroupTypeEnum } from "./group-type-enum";

export class Person {
  public id: number;
  public firstName: string;
  public middleName: string;
  public lastName: string;
  public startDate: Date;
  public endDate: Date;
  public currentGroup: GroupTypeEnum;

  constructor(data: any) {
    this.id = data.id;
    this.firstName = data.firstName;
    this.middleName = data.middleName;
    this.lastName = data.lastName;
    this.startDate = new Date(data.startDate);
    this.endDate = data.endDate ? new Date(data.endDate) : null;
    this.currentGroup = GroupTypeEnum.valueOf(data.currentGroup);
  }

  public fullName(): string {
    const middleName = this.middleName ? ' ' + this.middleName : '';
    return this.lastName + ' ' + this.firstName + middleName;
  }
}
