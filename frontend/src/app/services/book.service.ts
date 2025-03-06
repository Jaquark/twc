import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model';
@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = 'http://localhost:8080/api/books';

  constructor(private http: HttpClient) { }

  searchBooks(query: string, sortBy: string, sortOrder: string, page: number, pageSize: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/search?query=${query}&sortBy=${sortBy}&sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`);
  }

  addBook(book: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add`, book);
  }

  getBookDetails(bookId: number): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${bookId}`);
  }

  updateBook(bookId: number, book: Book): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/edit/${bookId}`, book);
  }

  deleteBook(bookId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete/${bookId}`);
  }
}