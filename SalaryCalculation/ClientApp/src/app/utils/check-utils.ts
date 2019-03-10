export class CheckUtils {
  static IsExists(value: any): boolean {
    return (value !== null) && (value !== undefined) && (value !== '');
  }
}