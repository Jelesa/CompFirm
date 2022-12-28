import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  private errors: string[] = [];

  constructor() {
  }

  pushError(message: string) {
    this.errors.push(message);

    setTimeout(_ => this.errors.shift(), 5000);
  }

  getErrors(): string[] {
    return this.errors;
  }
}
