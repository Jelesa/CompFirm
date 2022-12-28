import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthResponse } from '../shared/models/auth-response.model';
import { AuthRequest } from '../shared/models/auth-resquest.model';
import { UserInfo, UpdateUserRequest } from "../home/models/user-info.model";

@Injectable({
  providedIn: 'root'
})
export class AuthHttpService {

  constructor(private http: HttpClient, @Inject('API_URL') private baseUrl: string) { }

  createUser(user: any) {
    return this.http.post(this.baseUrl + 'auth/sign-up', user);
  }

  logIn(request: AuthRequest): Observable<AuthResponse> {
    return this.http.get<AuthResponse>(this.baseUrl + `auth/login?login=${request.login}&password=${request.password}`);
  }

  getUserInfo(): Observable<UserInfo> {
    return this.http.get<UserInfo>(this.baseUrl + `auth/user-info`);
  }

  updateUser(userInfo: UpdateUserRequest) {
    return this.http.put(this.baseUrl + `auth/update-user-info`, userInfo);
  }
}
