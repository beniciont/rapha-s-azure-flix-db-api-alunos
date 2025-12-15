import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { authService } from '@/services/authService';
import { User, ApiError } from '@/types/api';
import { toast } from 'sonner';

interface AuthUser extends User {
  roles: string[];
}

interface AuthContextType {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  isAdmin: boolean;
  login: (email: string, password: string) => Promise<boolean>;
  register: (name: string, email: string, password: string) => Promise<boolean>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Check for existing session on mount
  useEffect(() => {
    const checkAuth = async () => {
      if (authService.isAuthenticated()) {
        try {
          const userData = await authService.getCurrentUser();
          setUser(userData);
        } catch (error) {
          // Token invalid or expired
          authService.logout();
        }
      }
      setIsLoading(false);
    };

    checkAuth();

    // Listen for logout events (e.g., from 401 responses)
    const handleLogout = () => {
      setUser(null);
    };

    window.addEventListener('auth:logout', handleLogout);
    return () => window.removeEventListener('auth:logout', handleLogout);
  }, []);

  const login = async (email: string, password: string): Promise<boolean> => {
    try {
      const response = await authService.login({ email, password });
      setUser({
        ...response.user,
        roles: response.roles,
      });
      toast.success('Login realizado com sucesso!');
      return true;
    } catch (error) {
      const apiError = error as ApiError;
      toast.error(apiError.message || 'Erro ao fazer login');
      return false;
    }
  };

  const register = async (name: string, email: string, password: string): Promise<boolean> => {
    try {
      const response = await authService.register({ name, email, password });
      setUser({
        ...response.user,
        roles: response.roles,
      });
      toast.success('Conta criada com sucesso!');
      return true;
    } catch (error) {
      const apiError = error as ApiError;
      toast.error(apiError.message || 'Erro ao criar conta');
      return false;
    }
  };

  const logout = async () => {
    try {
      await authService.logout();
    } finally {
      setUser(null);
      toast.success('Logout realizado com sucesso');
    }
  };

  const isAdmin = user?.roles?.includes('admin') ?? false;

  return (
    <AuthContext.Provider value={{
      user,
      isAuthenticated: !!user,
      isLoading,
      isAdmin,
      login,
      register,
      logout
    }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
