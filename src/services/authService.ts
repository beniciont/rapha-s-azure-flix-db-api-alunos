import { API_CONFIG } from '@/config/api';
import { apiClient } from './apiClient';
import { 
  AuthResponse, 
  LoginRequest, 
  RegisterRequest, 
  User 
} from '@/types/api';

export const authService = {
  async login(credentials: LoginRequest): Promise<AuthResponse> {
    const response = await apiClient.post<AuthResponse>(
      API_CONFIG.ENDPOINTS.AUTH.LOGIN,
      credentials,
      false // Don't include auth header for login
    );

    // Store tokens
    apiClient.setToken(response.token);
    apiClient.setRefreshToken(response.refreshToken);

    return response;
  },

  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await apiClient.post<AuthResponse>(
      API_CONFIG.ENDPOINTS.AUTH.REGISTER,
      data,
      false
    );

    // Store tokens
    apiClient.setToken(response.token);
    apiClient.setRefreshToken(response.refreshToken);

    return response;
  },

  async logout(): Promise<void> {
    try {
      await apiClient.post(API_CONFIG.ENDPOINTS.AUTH.LOGOUT);
    } finally {
      apiClient.clearTokens();
    }
  },

  async getCurrentUser(): Promise<User & { roles: string[] }> {
    return apiClient.get<User & { roles: string[] }>(API_CONFIG.ENDPOINTS.AUTH.ME);
  },

  async refreshToken(): Promise<AuthResponse> {
    const refreshToken = apiClient.getRefreshToken();
    
    if (!refreshToken) {
      throw new Error('No refresh token available');
    }

    const response = await apiClient.post<AuthResponse>(
      API_CONFIG.ENDPOINTS.AUTH.REFRESH,
      { refreshToken },
      false
    );

    apiClient.setToken(response.token);
    apiClient.setRefreshToken(response.refreshToken);

    return response;
  },

  isAuthenticated(): boolean {
    return !!apiClient.getToken();
  },

  getToken(): string | null {
    return apiClient.getToken();
  },
};
