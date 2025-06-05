'use client';

import BookForm from '../../../components/BookForm';
import api from '../../../lib/axios';
import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { Book } from '../../../lib/types';
import Loader from '../../../components/Loader';
import ErrorMessage from '../../../components/ErrorMessage';

export default function NewBookPage() {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');

  const handleCreate = async (data: Omit<Book, 'id'>) => {
    setIsLoading(true);
    setSuccess(false);
    setError('');
    try {
      await api.post('/Books', data);
      setSuccess(true);
      setTimeout(() => {
        router.push('/books');
      }, 1000);
    } catch {
      setError('Error al crear el libro');
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={error} />;

  return (
    <div className="min-h-screen flex flex-col justify-center items-center bg-gray-50 py-8">
      <h2 className="text-2xl font-bold mb-6">Agregar nuevo libro</h2>
      <BookForm onSubmit={handleCreate} isLoading={isLoading} />
      {success && (
        <div className="mt-4 text-green-600 font-semibold">¡Libro creado exitosamente!</div>
      )}
    </div>
  );
} 