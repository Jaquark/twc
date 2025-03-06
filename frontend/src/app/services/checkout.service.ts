import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private apiUrl = 'http://localhost:8080/api/checkout';

  constructor(private http: HttpClient) {}

  getUserCheckouts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/my-checkouts`);
  }

  checkoutBook(bookId: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/checkout/${bookId}`, {});
  }
}