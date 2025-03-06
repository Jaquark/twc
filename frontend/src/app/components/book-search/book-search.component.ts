import { Component } from '@angular/core';
import { BookService } from '../../services/book.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book-search',
  templateUrl: './book-search.component.html',
  styleUrls: ['./book-search.component.css'],
  imports: [FormsModule, CommonModule]
})
export class BookSearchComponent {
  searchQuery: string = '';
  books: any[] = [];
  sortBy: string = 'title';
  sortOrder: string = 'asc';
  pageNumber: number = 1;
  pageSize: number = 10;

  constructor(private bookService: BookService) {}

  onSearch() {
    if (this.searchQuery.trim() === '') {
      this.books = [];
      return;
    }

    this.bookService.searchBooks(this.searchQuery, this.sortBy, this.sortOrder, this.pageNumber, this.pageSize).subscribe(
      (response) => {
        this.books = response;
      },
      (error) => {
        console.error('Error fetching books:', error);
      }
    );
  }

  onSortChange() {
    this.onSearch(); // Refresh search results with new sort settings
  }
}