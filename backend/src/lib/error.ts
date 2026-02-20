export class LambdaError extends Error {
  constructor (name: string, message: string) {
    super(message);
    this.name = name;
    Object.setPrototypeOf(this, LambdaError.prototype);
  }
}
