export interface Review {
    id: number;
    bookId: number;
    userId: number;
    username: string;
    rating: number; // 1-5 stars
    comment: string;
  }