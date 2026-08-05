import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap, catchError, EMPTY } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api.constants';
import { AuthResponse } from '../../shared/models/auth-response.model';
import { LoginRequest, RegisterRequest } from '../../shared/models/auth-request.model';

const TOKEN_KEY = 'access_token';
const REFRESH_KEY = 'refresh_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  // Estado reactivo con signals (Angular moderno)
  private readonly _currentUser = signal<AuthResponse | null>(
    this.loadStoredUser()
  );

  // Computed derivados — sin lógica duplicada
  readonly isAuthenticated = computed(() => this._currentUser() !== null);
  readonly currentUser = this._currentUser.asReadonly();

  login(credentials: LoginRequest) {
    return this.http.post<AuthResponse>(API_ENDPOINTS.auth.login, credentials).pipe(
      tap(response => this.storeSession(response))
    );
  }

  register(data: RegisterRequest) {
    return this.http.post<void>(API_ENDPOINTS.auth.register, data);
  }

  logout(): void {
    const refreshToken = localStorage.getItem(REFRESH_KEY);
    // Fire-and-forget al backend — invalida el refresh token
    this.http.post(API_ENDPOINTS.auth.logout, { refreshToken }).pipe(
      catchError(() => EMPTY) // Si falla, igual limpiamos localmente
    ).subscribe();
    this.clearSession();
    this.router.navigate(['/login']);
  }

  refreshToken() {
    const refreshToken = localStorage.getItem(REFRESH_KEY);
    if (!refreshToken) return EMPTY;
    return this.http.post<AuthResponse>(API_ENDPOINTS.auth.refresh, { refreshToken }).pipe(
      tap(response => this.storeSession(response))
    );
  }

  getAccessToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private storeSession(response: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, response.token);
    localStorage.setItem(REFRESH_KEY, response.refreshToken);
    this._currentUser.set(response);
  }

  private clearSession(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_KEY);
    this._currentUser.set(null);
  }

  private loadStoredUser(): AuthResponse | null {
    const token = localStorage.getItem(TOKEN_KEY);
    
    return token ? { token } as AuthResponse : null;
  }
}
