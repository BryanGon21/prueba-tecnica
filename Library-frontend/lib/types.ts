export interface Book {
  id: string;
  title: string;
  author: string;
  publicationYear: number;
  genre: string;
  status: number; // 0 = Available, 1 = Borrowed
} 