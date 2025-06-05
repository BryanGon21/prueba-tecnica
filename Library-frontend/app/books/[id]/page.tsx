'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import api from '../../../lib/axios';
import { Book } from '../../../lib/types';
import Loader from '../../../components/Loader';
import ErrorMessage from '../../../components/ErrorMessage';

export default function BookDetailPage() {
  const { id } = useParams();
  const router = useRouter();
  const [book, setBook] = useState<Book | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!id) return;
    setIsLoading(true);
    api.get(`/Books/${id}`)
      .then(res => setBook(res.data))
      .catch(() => setError('No se pudo cargar el libro'))
      .finally(() => setIsLoading(false));
  }, [id]);

  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={error} />;
  if (!book) return <ErrorMessage message="Libro no encontrado" />;

  return (
    <div className="min-h-screen flex flex-col justify-center items-center bg-gray-50 py-8">
      <div className="bg-white rounded-lg shadow p-8 w-full max-w-md">
        <h2 className="text-2xl font-bold mb-4">Detalle del libro</h2>
        <div className="space-y-2">
          <div><span className="font-semibold">Título:</span> {book.title}</div>
          <div><span className="font-semibold">Autor:</span> {book.author}</div>
          <div><span className="font-semibold">Año de publicación:</span> {book.publicationYear}</div>
          <div><span className="font-semibold">Género:</span> {book.genre}</div>
          <div>
            <span className="font-semibold">Estado:</span> {book.status === 'Available' ? (
              <span className="ml-2 px-2 py-1 bg-green-100 text-green-800 rounded-full text-xs">Disponible</span>
            ) : (
              <span className="ml-2 px-2 py-1 bg-red-100 text-red-800 rounded-full text-xs">Prestado</span>
            )}
          </div>
        </div>
        <div className="flex gap-4 mt-8">
          <button
            onClick={() => router.push('/books')}
            className="bg-gray-200 hover:bg-gray-300 text-gray-800 px-4 py-2 rounded"
          >
            Volver
          </button>
          <button
            onClick={() => router.push(`/books/${book.id}/edit`)}
            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded"
          >
            Editar
          </button>
        </div>
      </div>
    </div>
  );
} 