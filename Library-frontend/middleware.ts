import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
import { jwtDecode } from 'jwt-decode';

const protectedRoutes = ['/books'];
const adminRoutes = ['/books/new'];
const adminRoutePatterns = [
  /^\/books\/[^/]+\/edit$/, 
];
const publicRoutes = ['/login'];

interface DecodedToken {
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string;
}

export function middleware(request: NextRequest) {
  const token = request.cookies.get('token')?.value;
  const { pathname } = request.nextUrl;

  const isProtectedRoute = protectedRoutes.some(route => pathname.startsWith(route));

  const isAdminRoute = adminRoutes.some(route => pathname === route) || 
                      adminRoutePatterns.some(pattern => pattern.test(pathname));

  const isPublicRoute = publicRoutes.some(route => pathname === route);

  if (!token && (isProtectedRoute || isAdminRoute)) {
    return NextResponse.redirect(new URL('/login', request.url));
  }

  if (token && isAdminRoute) {
    try {
      const decoded = jwtDecode<DecodedToken>(token);
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (role !== 'admin') {
        return NextResponse.redirect(new URL('/books', request.url));
      }
    } catch (error) {
      console.error('Error decoding token:', error);
      return NextResponse.redirect(new URL('/login', request.url));
    }
  }

  if (token && isPublicRoute) {
    return NextResponse.redirect(new URL('/books', request.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: [
    '/((?!api|_next/static|_next/image|favicon.ico).*)',
  ],
}; 