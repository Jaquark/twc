import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Checkout } from '../models/checkout.model';
import { Review } from '../models/review.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'http://localhost:8080/api/users';  // Your API base URL

  constructor(private http: HttpClient) {}

  signup(user: { Username: string; Email: string; Password: string; Role: string , Reviews: Review[], Checkouts: Checkout[]}): Observable<any> {
    user.Reviews = [];
    user.Checkouts = [];
    return this.http.post(this.apiUrl, user);
  }
}