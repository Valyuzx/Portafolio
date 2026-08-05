import { Component, inject, input, output, computed } from '@angular/core';
import { MatToolbar } from '@angular/material/toolbar';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton } from '@angular/material/button';
import { MatTooltip } from '@angular/material/tooltip';
import { ThemeService } from '../../../core/services/theme.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [MatToolbar, MatIcon, MatIconButton, MatTooltip],
  templateUrl: './topbar.html',
  styleUrl: './topbar.scss'
})
export class Topbar {
  private readonly authService = inject(AuthService);
  private readonly themeService = inject(ThemeService);

  readonly showMenuButton = input<boolean>(false);
  readonly toggleMenu = output<void>();
  readonly user = this.authService.currentUser;

  // Computed Signals: Limpiamos la vista HTML para evitar la doble evaluación
  readonly isDarkMode = computed(() => this.themeService.theme() === 'dark');
  readonly themeIcon = computed(() => this.isDarkMode() ? 'light_mode' : 'dark_mode');
  readonly themeTooltip = computed(() => this.isDarkMode() ? 'Modo claro' : 'Modo oscuro');
  readonly displayName = computed(() => this.user()?.name ?? this.user()?.email ?? 'Usuario');

  onToggleTheme() {
    this.themeService.toggleTheme();
  }

  onLogout() {
    this.authService.logout();
  }
}
