import Link from 'next/link';

export default function Navbar() {
  return (
    <nav className="bg-blue-700 text-white px-4 py-3 shadow flex items-center justify-between">
      <div className="font-bold text-xl">
        <Link href="/books">Biblioteca</Link>
      </div>
      <div className="flex gap-6 items-center">
        <Link href="/books" className="hover:underline">Libros</Link>
        <button className="bg-white text-blue-700 px-4 py-1 rounded hover:bg-blue-100 font-medium transition-colors">
          Login
        </button>
      </div>
    </nav>
  );
} 