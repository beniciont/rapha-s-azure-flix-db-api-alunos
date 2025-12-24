import { API_CONFIG } from '@/config/api';
import { apiClient } from './apiClient';
import { 
  Movie, 
  PaginatedResponse, 
  SearchMoviesParams,
  CreateMovieRequest,
  UpdateMovieRequest,
} from '@/types/api';

export const movieService = {
  async getAll(params?: SearchMoviesParams): Promise<PaginatedResponse<Movie>> {
    return apiClient.get<PaginatedResponse<Movie>>(
      API_CONFIG.ENDPOINTS.MOVIES.LIST,
      params as Record<string, string | number | undefined>
    );
  },

  async getById(id: string): Promise<Movie> {
    return apiClient.get<Movie>(API_CONFIG.ENDPOINTS.MOVIES.BY_ID(id));
  },

  async getByGenre(genre: string): Promise<Movie[]> {
    const response = await apiClient.get<PaginatedResponse<Movie>>(
      API_CONFIG.ENDPOINTS.MOVIES.BY_GENRE(genre)
    );
    return response.data;
  },

  async search(query: string, params?: Omit<SearchMoviesParams, 'query'>): Promise<PaginatedResponse<Movie>> {
    return apiClient.get<PaginatedResponse<Movie>>(
      API_CONFIG.ENDPOINTS.MOVIES.SEARCH,
      { query, ...params } as Record<string, string | number | undefined>
    );
  },

  async getGenres(): Promise<string[]> {
    return apiClient.get<string[]>(API_CONFIG.ENDPOINTS.MOVIES.GENRES);
  },

  // Admin operations
  async create(data: CreateMovieRequest): Promise<Movie> {
    return apiClient.post<Movie>(API_CONFIG.ENDPOINTS.ADMIN.MOVIES, data);
  },

  async update(data: UpdateMovieRequest): Promise<Movie> {
    return apiClient.put<Movie>(
      API_CONFIG.ENDPOINTS.MOVIES.BY_ID(data.id),
      data
    );
  },

  async delete(id: string): Promise<void> {
    return apiClient.delete(API_CONFIG.ENDPOINTS.MOVIES.BY_ID(id));
  },
};
