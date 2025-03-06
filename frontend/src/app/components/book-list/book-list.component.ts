import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/book.service';
import { CheckoutService } from '../../services/checkout.service';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { Book } from 'src/app/models/book.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css'],
  imports: [FormsModule, CommonModule]
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  message: string = '';

  constructor(
    private bookService: BookService,
    private checkoutService: CheckoutService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.bookService.searchBooks(' ', 'title', 'asc', 1, 10).subscribe({
      next: data => this.books = data.books,
      error: err => console.error('Failed to load books', err)
    });
  }

  checkoutBook(bookId: number): void {
    if (!this.authService.isAuthenticated()) {
      this.message = 'You must be logged in to check out books.';
      return;
    }

    this.checkoutService.checkoutBook(bookId).subscribe({
      next: () => {
        this.message = 'Book checked out successfully!';
        this.loadBooks(); // Refresh book list after checkout
      },
      error: err => {
        this.message = err.error.message || 'Failed to check out book.';
      }
    });
  }
}
