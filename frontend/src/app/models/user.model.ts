export interface User {
    id: number;
    username: string;
    role: 'Librarian' | 'Customer'; // Role-based access control
  }