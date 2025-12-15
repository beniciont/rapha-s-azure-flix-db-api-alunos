// Configuração da API Backend
// Altere esta URL para apontar para seu servidor .NET Core no Azure
export const API_CONFIG = {
  // URL base da API - configure conforme seu ambiente
  BASE_URL: import.meta.env.VITE_API_URL || 'https://sua-api.azurewebsites.net/api',
  
  // Timeout padrão para requisições (em ms)
  TIMEOUT: 30000,
  
  // Endpoints da API
  ENDPOINTS: {
    // Autenticação
    AUTH: {
      LOGIN: '/auth/login',
      REGISTER: '/auth/register',
      LOGOUT: '/auth/logout',
      REFRESH: '/auth/refresh',
      ME: '/auth/me',
    },
    // Filmes
    MOVIES: {
      LIST: '/movies',
      BY_ID: (id: string) => `/movies/${id}`,
      BY_GENRE: (genre: string) => `/movies/genre/${genre}`,
      SEARCH: '/movies/search',
      GENRES: '/movies/genres',
    },
    // Aluguéis
    RENTALS: {
      LIST: '/rentals',
      BY_ID: (id: string) => `/rentals/${id}`,
      CREATE: '/rentals',
      RETURN: (id: string) => `/rentals/${id}/return`,
      USER_RENTALS: '/rentals/my-rentals',
    },
    // Admin
    ADMIN: {
      USERS: '/admin/users',
      USER_BY_ID: (id: string) => `/admin/users/${id}`,
      MOVIES: '/admin/movies',
      RENTALS: '/admin/rentals',
      STATS: '/admin/stats',
    },
  },
};
