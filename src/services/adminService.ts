import { API_CONFIG } from '@/config/api';
import { apiClient } from './apiClient';
import { 
  User, 
  AdminStats, 
  PaginatedResponse,
} from '@/types/api';

export const adminService = {
  async getStats(): Promise<AdminStats> {
    return apiClient.get<AdminStats>(API_CONFIG.ENDPOINTS.ADMIN.STATS);
  },

  async getUsers(params?: { page?: number; pageSize?: number }): Promise<PaginatedResponse<User>> {
    return apiClient.get<PaginatedResponse<User>>(
      API_CONFIG.ENDPOINTS.ADMIN.USERS,
      params as Record<string, string | number | undefined>
    );
  },

  async getUserById(id: string): Promise<User> {
    return apiClient.get<User>(API_CONFIG.ENDPOINTS.ADMIN.USER_BY_ID(id));
  },

  async deleteUser(id: string): Promise<void> {
    return apiClient.delete(API_CONFIG.ENDPOINTS.ADMIN.USER_BY_ID(id));
  },

  async updateUserRole(userId: string, role: 'admin' | 'user'): Promise<void> {
    return apiClient.patch(`${API_CONFIG.ENDPOINTS.ADMIN.USER_BY_ID(userId)}/role`, { role });
  },
};
