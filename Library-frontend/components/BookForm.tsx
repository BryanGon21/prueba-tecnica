"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Book } from "../lib/types";
import { useState } from "react";

const bookSchema = z.object({
  title: z.string().min(1, "El título es obligatorio"),
  author: z.string().min(1, "El autor es obligatorio"),
  publicationYear: z
    .number({ invalid_type_error: "El año debe ser un número" })
    .min(1000, "Año inválido")
    .max(new Date().getFullYear(), "Año inválido"),
  genre: z.string().min(1, "El género es obligatorio"),
  status: z.enum(["Available", "Borrowed"]),
});

type BookFormData = z.infer<typeof bookSchema>;

interface BookFormProps {
  initialData?: Partial<Book>;
  onSubmit: (data: BookFormData) => Promise<void>;
  isLoading?: boolean;
}

export default function BookForm({ initialData = {}, onSubmit, isLoading }: BookFormProps) {
  const [formError, setFormError] = useState("");
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<BookFormData>({
    resolver: zodResolver(bookSchema),
    defaultValues: {
      title: initialData.title || "",
      author: initialData.author || "",
      publicationYear: initialData.publicationYear || new Date().getFullYear(),
      genre: initialData.genre || "",
      status: initialData.status || "Available",
    },
  });

  const submitHandler = async (data: BookFormData) => {
    setFormError("");
    try {
      await onSubmit(data);
      reset();
    } catch (err: any) {
      setFormError(err.message || "Error al guardar el libro");
    }
  };

  return (
    <form onSubmit={handleSubmit(submitHandler)} className="space-y-4 max-w-lg mx-auto bg-white p-6 rounded shadow">
      <div>
        <label className="block font-medium">Título</label>
        <input
          type="text"
          {...register("title")}
          className="mt-1 block w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        {errors.title && <span className="text-red-500 text-sm">{errors.title.message}</span>}
      </div>
      <div>
        <label className="block font-medium">Autor</label>
        <input
          type="text"
          {...register("author")}
          className="mt-1 block w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        {errors.author && <span className="text-red-500 text-sm">{errors.author.message}</span>}
      </div>
      <div>
        <label className="block font-medium">Año de publicación</label>
        <input
          type="number"
          {...register("publicationYear", { valueAsNumber: true })}
          className="mt-1 block w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        {errors.publicationYear && <span className="text-red-500 text-sm">{errors.publicationYear.message}</span>}
      </div>
      <div>
        <label className="block font-medium">Género</label>
        <input
          type="text"
          {...register("genre")}
          className="mt-1 block w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        {errors.genre && <span className="text-red-500 text-sm">{errors.genre.message}</span>}
      </div>
      <div>
        <label className="block font-medium">Estado</label>
        <select
          {...register("status")}
          className="mt-1 block w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="Available">Disponible</option>
          <option value="Borrowed">Prestado</option>
        </select>
        {errors.status && <span className="text-red-500 text-sm">{errors.status.message}</span>}
      </div>
      {formError && <div className="text-red-600 text-sm">{formError}</div>}
      <button
        type="submit"
        disabled={isLoading}
        className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded"
      >
        {isLoading ? "Guardando..." : "Guardar"}
      </button>
    </form>
  );
} 