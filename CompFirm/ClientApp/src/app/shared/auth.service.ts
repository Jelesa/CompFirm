import { Injectable } from '@angular/core';

const IsLoggedInItemName: string = 'isLoggedIn';
const TokenItemName: string = 'token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  getToken() {
    return localStorage.getItem(TokenItemName); 
  }

  isUserLoggedIn() {
    return localStorage.getItem(IsLoggedInItemName) == "true";
  }

  setUserCredentials(token: string) {
    localStorage.setItem(IsLoggedInItemName, 'true');
    localStorage.setItem(TokenItemName, token);
  }

  logOut() {
    localStorage.setItem(IsLoggedInItemName, 'false');
    localStorage.setItem(TokenItemName, null);
  }
}
