export interface Book {
    id: number;
    title: string;
    author: string;
    description: string;
    coverImage: string;
    averageRating: number; // Dynamically computed from reviews
  }