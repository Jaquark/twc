import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { BookService } from '../../services/book.service';
import { ReviewService } from '../../services/review.service';
import { Book } from '../../models/book.model';
import { Review } from '../../models/review.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-book-details',
    templateUrl: './book-details.component.html',
    styleUrls: ['./book-details.component.css'],
    imports: [FormsModule]
  })
  export class BookDetailsComponent implements OnInit {
    book: Book | null = null;
    reviews: Review[] = [];
    errorMessage = '';
  
    constructor(
      private bookService: BookService,
      private reviewService: ReviewService,
      private authService: AuthService,
      private route: ActivatedRoute
    ) {}
  
    ngOnInit(): void {
      const bookId = Number(this.route.snapshot.paramMap.get('id'));
  
      if (!bookId) {
        this.errorMessage = 'Invalid book ID';
        return;
      }
  
      // Fetch book details
      this.bookService.getBookDetails(bookId).subscribe({
        next: (data) => {
          this.book = data;
        },
        error: (err) => {
          this.errorMessage = err.error.message || 'Book not found';
        }
      });
  
      // Fetch reviews separately
      this.reviewService.getReviews(bookId).subscribe({
        next: (data) => {
          this.reviews = data;
        },
        error: (err) => {
          console.error('Error loading reviews', err);
        }
      });
    }
  
    isLibrarian(): boolean {
      return this.authService.isLibrarian();
    }
  }