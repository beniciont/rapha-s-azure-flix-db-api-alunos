import { API_CONFIG } from '@/config/api';
import { apiClient } from './apiClient';
import { 
  Rental, 
  CreateRentalRequest,
  PaginatedResponse,
} from '@/types/api';

export const rentalService = {
  async getUserRentals(): Promise<Rental[]> {
    return apiClient.get<Rental[]>(API_CONFIG.ENDPOINTS.RENTALS.USER_RENTALS);
  },

  async getById(id: string): Promise<Rental> {
    return apiClient.get<Rental>(API_CONFIG.ENDPOINTS.RENTALS.BY_ID(id));
  },

  async create(data: CreateRentalRequest): Promise<Rental> {
    return apiClient.post<Rental>(API_CONFIG.ENDPOINTS.RENTALS.CREATE, data);
  },

  async returnRental(id: string): Promise<Rental> {
    return apiClient.post<Rental>(API_CONFIG.ENDPOINTS.RENTALS.RETURN(id));
  },

  // Check if a movie is currently rented by the user
  async isMovieRented(movieId: string): Promise<boolean> {
    const rentals = await this.getUserRentals();
    return rentals.some(
      rental => rental.movieId === movieId && rental.status === 'active'
    );
  },

  // Admin operations
  async getAll(params?: { page?: number; pageSize?: number; status?: string }): Promise<PaginatedResponse<Rental>> {
    return apiClient.get<PaginatedResponse<Rental>>(
      API_CONFIG.ENDPOINTS.ADMIN.RENTALS,
      params as Record<string, string | number | undefined>
    );
  },
};
