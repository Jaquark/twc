import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:8080/api/auth';

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string): Observable<boolean> {
    return this.http.post<{ message: string, role: string }>(`${this.apiUrl}/login`, { username, password }).pipe(
      map(response => {
        sessionStorage.setItem('authToken', response.message);
        sessionStorage.setItem('username', username);
        sessionStorage.setItem('role', response.role); // Store user role
        return true;
      })
    );
  }

  logout(): void {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    return sessionStorage.getItem('authToken') !== null;
  }

  getRole(): string | null {
    return sessionStorage.getItem('role');
  }

  isLibrarian(): boolean {
    return this.getRole() === 'Librarian';
  }
}
