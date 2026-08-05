export interface AuthResponse {
  token: string;
  email: string;
  name: string;
  refreshToken: string;
  expiresAt: string; 
}

export interface RefreshTokenResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
}
