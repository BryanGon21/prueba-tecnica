import { Fragment } from 'react';
import { Dialog, Transition } from '@headlessui/react';

interface ConfirmDialogProps {
  open: boolean;
  title?: string;
  description?: string;
  confirmText?: string;
  cancelText?: string;
  onConfirm: () => void;
  onCancel: () => void;
}

export default function ConfirmDialog({
  open,
  title = '¿Estás seguro?',
  description = 'Esta acción no se puede deshacer.',
  confirmText = 'Sí, eliminar',
  cancelText = 'Cancelar',
  onConfirm,
  onCancel,
}: ConfirmDialogProps) {
  return (
    <Transition show={open} as={Fragment}>
      <Dialog as="div" className="relative z-50" onClose={onCancel}>
        <Transition show={open} as={Fragment}>
          <div className="fixed inset-0 bg-black bg-opacity-30 transition-opacity" />
        </Transition>
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
          <Transition show={open} as={Fragment}>
            <div className="bg-white rounded-lg shadow-xl max-w-sm w-full p-6">
              <h2 className="text-lg font-bold mb-2">{title}</h2>
              <p className="mb-4 text-gray-600">{description}</p>
              <div className="flex justify-end gap-2">
                <button
                  onClick={onCancel}
                  className="px-4 py-2 rounded bg-gray-200 hover:bg-gray-300 text-gray-800"
                >
                  {cancelText}
                </button>
                <button
                  onClick={onConfirm}
                  className="px-4 py-2 rounded bg-red-600 hover:bg-red-700 text-white font-semibold"
                >
                  {confirmText}
                </button>
              </div>
            </div>
          </Transition>
        </div>
      </Dialog>
    </Transition>
  );
} 