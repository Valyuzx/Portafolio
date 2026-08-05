import { Component, input } from '@angular/core';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatIcon } from '@angular/material/icon';

/** Variantes visuales del botón */
export type ButtonVariant = 'primary' | 'secondary' | 'ghost';

/** Tamaños disponibles del botón */
export type ButtonSize = 'sm' | 'md' | 'lg';

/**
 * Botón compartido y centralizado para toda la aplicación.
 *
 * Soporta tres variantes: primary, secondary y ghost.
 * Todos los estilos consumen tokens de `variables.scss`.
 *
 * @example — Acción principal en un formulario
 * ```html
 * <app-button type="submit" [loading]="isLoading()" [disabled]="form.invalid">
 *   Guardar
 * </app-button>
 * ```
 *
 * @example — Acción secundaria
 * ```html
 * <app-button variant="secondary" icon="arrow_back" (click)="onCancel()">
 *   Cancelar
 * </app-button>
 * ```
 *
 * @example — Ancho completo con estado de carga
 * ```html
 * <app-button [fullWidth]="true" [loading]="loading()" loadingLabel="Iniciando...">
 *   Iniciar sesión
 * </app-button>
 * ```
 */
@Component({
  selector: 'app-button',
  imports: [MatProgressSpinner, MatIcon],
  template: `
    <button
      class="app-btn"
      [class]="'app-btn--' + variant()"
      [class.app-btn--sm]="size() === 'sm'"
      [class.app-btn--lg]="size() === 'lg'"
      [class.app-btn--full]="fullWidth()"
      [type]="type()"
      [disabled]="disabled() || loading()"
      [attr.aria-busy]="loading()"
    >
      @if (loading()) {
        <mat-spinner diameter="16" />
        <span>{{ loadingLabel() }}</span>
      } @else {
        @if (icon()) {
          <mat-icon aria-hidden="true">{{ icon() }}</mat-icon>
        }
        <span><ng-content /></span>
      }
    </button>
  `,
  styleUrl: './button.scss',
})
export class AppButton {
  /** Variante visual del botón. @default 'primary' */
  readonly variant = input<ButtonVariant>('primary');

  /** Tipo HTML nativo del botón. @default 'button' */
  readonly type = input<'button' | 'submit' | 'reset'>('button');

  /** Tamaño del botón. @default 'md' */
  readonly size = input<ButtonSize>('md');

  /** Muestra spinner y bloquea el botón mientras es true. */
  readonly loading = input(false);

  /** Deshabilita el botón independientemente del loading. */
  readonly disabled = input(false);

  /** Texto mostrado mientras loading es true. */
  readonly loadingLabel = input('Cargando...');

  /** Nombre del ícono de Material Icons a mostrar a la izquierda. */
  readonly icon = input('');

  /** Hace que el botón ocupe el 100% del ancho de su contenedor. */
  readonly fullWidth = input(false);
}
