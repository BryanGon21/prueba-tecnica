'use client';

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useRouter } from 'next/navigation';
import api from '../../lib/axios';
import { Book } from '../../lib/types';
import BookActions from '../components/BookActions';
import Link from 'next/link';
import Loader from '../../components/Loader';
import ErrorMessage from '../../components/ErrorMessage';
import { useAuth } from '../../lib/auth/AuthContext';

function fetchBooks(): Promise<Book[]> {
  return api.get('/Books').then(res => res.data);
}

export default function BooksPage() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const { isAdmin, isUser, user } = useAuth();
  const { data: books, isLoading, isError } = useQuery({
    queryKey: ['books'],
    queryFn: fetchBooks,
  });

  const deleteMutation = useMutation({
    mutationFn: (book: Book) => api.delete(`/Books/${book.id}`),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['books'] });
    },
  });

  const toggleStatusMutation = useMutation({
    mutationFn: async (book: Book) => {
      if (book.status === 0) {
        return api.patch(`/Books/${book.id}/borrow`);
      }
      return api.patch(`/Books/${book.id}/return`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['books'] });
    },
    onError: (error: any) => {
      console.error('Error al cambiar el estado del libro:', error);
      const errorMessage = error.message || error.response?.data?.detailedMessage || error.response?.data?.message || 'No se pudo cambiar el estado del libro. Por favor, intenta de nuevo.';
      alert(errorMessage);
    }
  });

  const handleEdit = (book: Book) => {
    router.push(`/books/${book.id}/edit`);
  };

  const handleDelete = (book: Book) => {
      deleteMutation.mutate(book);
  };

  const handleToggleStatus = (book: Book) => {
    toggleStatusMutation.mutate(book);
  };

  if (isLoading) {
    return <Loader />;
  }

  if (isError) {
    return <ErrorMessage message="Error al cargar los libros. Por favor, intenta de nuevo." />;
  }

  console.log('Current user:', user);
  console.log('Is admin?', isAdmin());

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-800">Biblioteca</h1>
        {isAdmin() && (
          <Link
            href="/books/new"
            className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-lg font-medium"
          >
            Agregar Libro
          </Link>
        )}
      </div>

      <div className="bg-white rounded-lg shadow overflow-hidden">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Título</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Autor</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Año</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Género</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Estado</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Acciones</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {books?.map(book => (
                <tr key={book.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{book.title}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{book.author}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{book.publicationYear}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{book.genre}</td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      book.status === 0 
                        ? 'bg-green-100 text-green-800' 
                        : 'bg-red-100 text-red-800'
                    }`}>
                      {book.status === 0 ? 'Disponible' : 'Prestado'}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 flex gap-2">
                    <Link
                      href={`/books/${book.id}`}
                      className="px-2 py-1 rounded text-sm font-medium bg-indigo-500 hover:bg-indigo-600 text-white"
                    >
                      Ver
                    </Link>
                    {isUser() && (
                      <BookActions
                        book={book}
                        onEdit={isAdmin() ? handleEdit : undefined}
                        onDelete={isAdmin() ? handleDelete : undefined}
                        onToggleStatus={handleToggleStatus}
                      />
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
} 