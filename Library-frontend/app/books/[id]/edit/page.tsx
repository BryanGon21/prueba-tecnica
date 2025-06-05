'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import BookForm from '../../../../components/BookForm';
import api from '../../../../lib/axios';
import { Book } from '../../../../lib/types';
import Loader from '../../../../components/Loader';
import ErrorMessage from '../../../../components/ErrorMessage';

export default function EditBookPage() {
  const { id } = useParams();
  const router = useRouter();
  const [book, setBook] = useState<Book | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    if (!id) return;
    setIsLoading(true);
    api.get(`/Books/${id}`)
      .then(res => setBook(res.data))
      .catch(() => setError('No se pudo cargar el libro'))
      .finally(() => setIsLoading(false));
  }, [id]);

  const handleUpdate = async (data: Omit<Book, 'id'>) => {
    if (!id) return;
    setIsSaving(true);
    setError('');
    setSuccess(false);
    try {
      await api.put(`/Books/${id}`, { ...data, id });
      setSuccess(true);
      setTimeout(() => {
        router.push('/books');
      }, 1000);
    } catch {
      setError('Error al actualizar el libro');
    } finally {
      setIsSaving(false);
    }
  };

  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={error} />;
  if (!book) return <ErrorMessage message="Libro no encontrado" />;

  return (
    <div className="min-h-screen flex flex-col justify-center items-center bg-gray-50 py-8">
      <h2 className="text-2xl font-bold mb-6">Editar libro</h2>
      <BookForm initialData={book} onSubmit={handleUpdate} isLoading={isSaving} />
      {success && (
        <div className="mt-4 text-green-600 font-semibold">¡Libro actualizado exitosamente!</div>
      )}
    </div>
  );
} 