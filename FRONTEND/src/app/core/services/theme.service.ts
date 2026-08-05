import { Injectable, signal, effect } from '@angular/core';

export type Theme = 'light' | 'dark';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly THEME_KEY = 'app-theme-preference';

  // Inicializamos la signal de forma síncrona
  private readonly _theme = signal<Theme>(this.getInitialTheme());
  
  // Exponemos la signal como readonly para los componentes
  readonly theme = this._theme.asReadonly();

  constructor() {
    // El effect de Angular 16+ se encarga de reaccionar cada vez que la señal cambie.
    // Guarda en localStorage y actualiza el <body>.
    effect(() => {
      const currentTheme = this._theme();
      this.applyTheme(currentTheme);
      localStorage.setItem(this.THEME_KEY, currentTheme);
    });
  }

  toggleTheme(): void {
    this._theme.update(t => t === 'dark' ? 'light' : 'dark');
  }

  setTheme(theme: Theme): void {
    this._theme.set(theme);
  }

  private getInitialTheme(): Theme {
    // Evitamos errores si hay Server Side Rendering (SSR)
    if (typeof window === 'undefined' || typeof localStorage === 'undefined') {
      return 'dark'; // Valor por defecto en el servidor
    }

    const savedTheme = localStorage.getItem(this.THEME_KEY) as Theme;
    if (savedTheme === 'light' || savedTheme === 'dark') {
      return savedTheme;
    }

    // Si no hay preferencia, leemos la del sistema operativo
    const prefersLight = window.matchMedia('(prefers-color-scheme: light)').matches;
    return prefersLight ? 'light' : 'dark';
  }

  private applyTheme(theme: Theme): void {
    if (typeof document === 'undefined') return;

    const body = document.body;
    
    // Limpiamos clases anteriores
    body.classList.remove('light-mode', 'dark-mode');
    
    // Aplicamos la nueva
    body.classList.add(`${theme}-mode`);
  }
}
