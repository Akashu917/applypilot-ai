export interface UserProfile {
  firstName: string;
  lastName: string;
  headline?: string | null;
  phoneNumber?: string | null;
  country: string;
  city: string;
  linkedInUrl?: string | null;
  gitHubUrl?: string | null;
  portfolioUrl?: string | null;
  preferredCurrency: string;
}

export interface User {
  id: string;
  email: string;
  role: 'Candidate' | 'Administrator';
  isEmailVerified: boolean;
  profile?: UserProfile | null;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors?: Record<string, string[]>;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  country: string;
  city: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface UpdateProfileRequest {
  firstName: string;
  lastName: string;
  headline?: string | null;
  phoneNumber?: string | null;
  country: string;
  city: string;
  linkedInUrl?: string | null;
  gitHubUrl?: string | null;
  portfolioUrl?: string | null;
  preferredCurrency: string;
}

