export interface Checkout {
    id: number;
    bookId: number;
    title: string;
    borrowedBy?: string; // Only librarians need this field
    checkoutDate: string;
    dueDate: string;
  }