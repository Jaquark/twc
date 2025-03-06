import { Component } from '@angular/core';
import { BookService } from '../../services/book.service';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';




@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css'],
  imports: [FormsModule, CommonModule]
})
export class AddBookComponent {
  book = {
    title: '',
    author: '',
    description: '',
    coverImage: '',
  };
  message = '';

  constructor(private bookService: BookService, private authService: AuthService) {}

  addBook(): void {
    if (!this.authService.isLibrarian()) {
      this.message = 'Only librarians can add books.';
      return;
    }

    this.bookService.addBook(this.book).subscribe({
      next: () => {
        this.message = 'Book added successfully!';
        this.book = { title: '', author: '', description: '', coverImage: '' };
      },
      error: (err) => {
        this.message = err.error.message || 'Failed to add book.';
      }
    });
  }
}