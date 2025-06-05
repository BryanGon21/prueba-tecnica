import { useState } from 'react';
import { Book } from '../../lib/types';
import ConfirmDialog from '../../components/ConfirmDialog';

interface BookActionsProps {
  book: Book;
  onEdit?: (book: Book) => void;
  onDelete?: (book: Book) => void;
  onToggleStatus: (book: Book) => void;
}

export default function BookActions({ book, onEdit, onDelete, onToggleStatus }: BookActionsProps) {
  const [showConfirm, setShowConfirm] = useState(false);

  const handleDeleteClick = () => setShowConfirm(true);
  const handleCancel = () => setShowConfirm(false);
  const handleConfirm = () => {
    setShowConfirm(false);
    onDelete?.(book);
  };

  return (
    <div className="flex gap-2">
      {book.status === 0 ? (
        <button
          onClick={() => onToggleStatus(book)}
          className="px-2 py-1 rounded text-sm font-medium bg-blue-500 hover:bg-blue-600 text-white"
        >
          Prestar
        </button>
      ) : (
        <button
          onClick={() => onToggleStatus(book)}
          className="px-2 py-1 rounded text-sm font-medium bg-green-500 hover:bg-green-600 text-white"
        >
          Devolver
        </button>
      )}
      {onEdit && (
        <button
          onClick={() => onEdit(book)}
          className="px-2 py-1 rounded text-sm font-medium bg-yellow-500 hover:bg-yellow-600 text-white"
        >
          Editar
        </button>
      )}
      {onDelete && (
        <button
          onClick={handleDeleteClick}
          className="px-2 py-1 rounded text-sm font-medium bg-red-500 hover:bg-red-600 text-white"
        >
          Eliminar
        </button>
      )}
      <ConfirmDialog
        open={showConfirm}
        title="¿Eliminar libro?"
        description={`¿Seguro que deseas eliminar "${book.title}"? Esta acción no se puede deshacer.`}
        confirmText="Sí, eliminar"
        cancelText="Cancelar"
        onConfirm={handleConfirm}
        onCancel={handleCancel}
      />
    </div>
  );
} 