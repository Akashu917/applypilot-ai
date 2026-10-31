import React, { createContext, useContext, useState, useEffect } from 'react';
import { User, UserProfile, RegisterRequest, LoginRequest, UpdateProfileRequest, ApiResponse, AuthResponse } from '../types/auth';
import { apiClient } from '../lib/api';

interface AuthContextType {
  user: User | null;
  profile: UserProfile | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (request: LoginRequest) => Promise<void>;
  register: (request: RegisterRequest) => Promise<void>;
  updateProfile: (request: UpdateProfileRequest) => Promise<void>;
  logout: () => Promise<void>;
  refreshUserData: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const initializeAuth = async () => {
      const storedToken = localStorage.getItem('applypilot_access_token');
      const storedUser = localStorage.getItem('applypilot_user');

      if (storedToken && storedUser) {
        try {
          const parsedUser: User = JSON.parse(storedUser);
          setUser(parsedUser);
          setProfile(parsedUser.profile || null);

          // Fetch fresh profile from API
          const response = await apiClient.get<ApiResponse<User>>('/auth/me');
          if (response.data.success && response.data.data) {
            setUser(response.data.data);
            setProfile(response.data.data.profile || null);
            localStorage.setItem('applypilot_user', JSON.stringify(response.data.data));
          }
        } catch (err) {
          console.warn('Initial session validation failed or token expired', err);
        }
      }
      setIsLoading(false);
    };

    initializeAuth();
  }, []);

  const login = async (request: LoginRequest) => {
    setIsLoading(true);
    try {
      const response = await apiClient.post<ApiResponse<AuthResponse>>('/auth/login', request);
      if (response.data.success && response.data.data) {
        const { accessToken, refreshToken, user: authUser } = response.data.data;
        localStorage.setItem('applypilot_access_token', accessToken);
        localStorage.setItem('applypilot_refresh_token', refreshToken);
        localStorage.setItem('applypilot_user', JSON.stringify(authUser));

        setUser(authUser);
        setProfile(authUser.profile || null);
      } else {
        throw new Error(response.data.message || 'Login failed.');
      }
    } finally {
      setIsLoading(false);
    }
  };

  const register = async (request: RegisterRequest) => {
    setIsLoading(true);
    try {
      const response = await apiClient.post<ApiResponse<AuthResponse>>('/auth/register', request);
      if (response.data.success && response.data.data) {
        const { accessToken, refreshToken, user: authUser } = response.data.data;
        localStorage.setItem('applypilot_access_token', accessToken);
        localStorage.setItem('applypilot_refresh_token', refreshToken);
        localStorage.setItem('applypilot_user', JSON.stringify(authUser));

        setUser(authUser);
        setProfile(authUser.profile || null);
      } else {
        throw new Error(response.data.message || 'Registration failed.');
      }
    } finally {
      setIsLoading(false);
    }
  };

  const updateProfile = async (request: UpdateProfileRequest) => {
    const response = await apiClient.put<ApiResponse<UserProfile>>('/users/profile', request);
    if (response.data.success && response.data.data) {
      setProfile(response.data.data);
      if (user) {
        const updatedUser = { ...user, profile: response.data.data };
        setUser(updatedUser);
        localStorage.setItem('applypilot_user', JSON.stringify(updatedUser));
      }
    } else {
      throw new Error(response.data.message || 'Failed to update profile.');
    }
  };

  const refreshUserData = async () => {
    const response = await apiClient.get<ApiResponse<User>>('/auth/me');
    if (response.data.success && response.data.data) {
      setUser(response.data.data);
      setProfile(response.data.data.profile || null);
      localStorage.setItem('applypilot_user', JSON.stringify(response.data.data));
    }
  };

  const logout = async () => {
    const refreshToken = localStorage.getItem('applypilot_refresh_token');
    try {
      if (refreshToken) {
        await apiClient.post('/auth/logout', { refreshToken });
      }
    } catch (err) {
      console.error('Logout error', err);
    } finally {
      localStorage.removeItem('applypilot_access_token');
      localStorage.removeItem('applypilot_refresh_token');
      localStorage.removeItem('applypilot_user');
      setUser(null);
      setProfile(null);
    }
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        profile,
        isAuthenticated: !!user,
        isLoading,
        login,
        register,
        updateProfile,
        logout,
        refreshUserData,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

