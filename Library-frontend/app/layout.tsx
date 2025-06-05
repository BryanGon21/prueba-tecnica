import '../styles/globals.css';
import { ReactNode } from 'react';
import ReactQueryProvider from '../components/ReactQueryProvider';
import Navbar from '../components/Navbar';

export default function RootLayout({ children }: { children: ReactNode }) {
  return (
    <html lang="en">
      <body>
        <ReactQueryProvider>
          <Navbar />
          {children}
        </ReactQueryProvider>
      </body>
    </html>
  );
} 