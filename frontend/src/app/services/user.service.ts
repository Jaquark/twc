import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'http://localhost:8080/api/users';  // Your API base URL

  constructor(private http: HttpClient) {}

  signup(user: { username: string; email: string; password: string; role: string }): Observable<any> {
    return this.http.post(this.apiUrl, user);
  }
}