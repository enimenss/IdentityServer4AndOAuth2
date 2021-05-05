import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  errorMessage: BehaviorSubject<string> = new BehaviorSubject<string>('');

  constructor() {}

  public pushErrorMessage(message: string) {
    this.errorMessage.next(message);
  }

  public clearErrorMessage() {
    this.errorMessage = new BehaviorSubject<string>('');
  }
}
