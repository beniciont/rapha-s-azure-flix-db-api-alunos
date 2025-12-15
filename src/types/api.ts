// Tipos para comunicação com a API .NET Core

// ========================
// Entidades do Banco de Dados
// ========================

export interface User {
  id: string;
  email: string;
  name: string;
  createdAt: string;
  updatedAt: string;
}

export interface UserRole {
  id: string;
  userId: string;
  role: 'admin' | 'user';
}

export interface Movie {
  id: string;
  title: string;
  synopsis: string;
  imageUrl: string;
  backdropUrl: string;
  trailerUrl?: string;
  year: number;
  duration: string;
  rating: number;
  genre: string;
  // Optional fields for API compatibility
  rentalPrice?: number;
  isAvailable?: boolean;
  createdAt?: string;
  updatedAt?: string;
}

export interface Rental {
  id: string;
  userId: string;
  movieId: string;
  rentedAt: string;
  dueDate: string;
  returnedAt?: string;
  status: 'active' | 'returned' | 'overdue';
  totalPrice: number;
  // Dados relacionados (populated)
  movie?: Movie;
  user?: User;
}

export interface Genre {
  id: string;
  name: string;
  description?: string;
}

// ========================
// Requisições da API
// ========================

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface CreateRentalRequest {
  movieId: string;
  rentalDays?: number; // padrão: 7 dias
}

export interface CreateMovieRequest {
  title: string;
  synopsis: string;
  imageUrl: string;
  backdropUrl: string;
  trailerUrl?: string;
  year: number;
  duration: string;
  genre: string;
  rentalPrice: number;
}

export interface UpdateMovieRequest extends Partial<CreateMovieRequest> {
  id: string;
  isAvailable?: boolean;
}

export interface SearchMoviesParams {
  query?: string;
  genre?: string;
  year?: number;
  minRating?: number;
  page?: number;
  pageSize?: number;
  sortBy?: 'title' | 'year' | 'rating' | 'createdAt';
  sortOrder?: 'asc' | 'desc';
}

// ========================
// Respostas da API
// ========================

export interface AuthResponse {
  user: User;
  token: string;
  refreshToken: string;
  expiresAt: string;
  roles: string[];
}

export interface PaginatedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface ApiError {
  message: string;
  code: string;
  details?: Record<string, string[]>;
}

export interface AdminStats {
  totalUsers: number;
  totalMovies: number;
  activeRentals: number;
  overdueRentals: number;
  totalRevenue: number;
  monthlyRevenue: number;
}
