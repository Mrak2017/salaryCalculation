import { CheckUtils } from "../../../utils/check-utils";

export interface OrgStructureItemInterface {
  personId: number;
  fullName: string;
  children?: OrgStructureItem[];
}

export class OrgStructureItem implements OrgStructureItemInterface {
  public personId: number;
  public fullName: string;
  public children?: OrgStructureItem[];

  constructor(data?: any) {
    if (CheckUtils.isExists(data)) {
      this.personId = data.personId;
      const middleName = data.middleName ? ' ' + data.middleName : '';
      this.fullName = data.lastName + ' ' + data.firstName + middleName;
      if (CheckUtils.isExists(data.children)) {
        this.children = data.children.map(val => new OrgStructureItem(val));
      }
    }
  }
}