import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../services/book.service';
import { AuthService } from '../../services/auth.service';
import { Book } from '../../models/book.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.css'],
  imports: [FormsModule, CommonModule]
})
export class EditBookComponent implements OnInit {
  book!: Book;
  message = '';

  constructor(
    private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const bookId = Number(this.route.snapshot.paramMap.get('id'));

    if (!this.authService.isLibrarian()) {
      this.message = 'Only librarians can edit books.';
      return;
    }

    if (!bookId) {
      this.message = 'Invalid book ID';
      return;
    }

    this.bookService.getBookDetails(bookId).subscribe({
      next: (data) => {
        this.book = data;
      },
      error: (err) => {
        this.message = err.error.message || 'Book not found';
      }
    });
  }

  saveChanges(): void {
    if (this.book) {
      this.bookService.updateBook(this.book.id, this.book).subscribe({
        next: () => {
          this.message = 'Book updated successfully!';
          setTimeout(() => this.router.navigate(['/book', this.book?.id]), 1000);
        },
        error: (err) => {
          this.message = err.error.message || 'Failed to update book.';
        }
      });
    }
  }

  deleteBook(): void {
    if (this.book && confirm('Are you sure you want to delete this book? This action cannot be undone.')) {
      this.bookService.deleteBook(this.book.id).subscribe({
        next: () => {
          alert('Book deleted successfully.');
          this.router.navigate(['/']); // Redirect to homepage or book list
        },
        error: (err) => {
          this.message = err.error.message || 'Failed to delete book.';
        }
      });
    }
  }
}