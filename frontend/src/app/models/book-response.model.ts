import { Book } from './book.model';

export interface BookResponse {
    books: Book[];
    totalBooks: number;
    totalPages: number;
  }