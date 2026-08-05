import { Component, input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

/**
 * Banner de error reutilizable.
 * Usa los design tokens del sistema (--app-error-*) para consistencia.
 */
@Component({
  selector: 'app-error-banner',
  imports: [MatIcon],
  template: `
    @if (message()) {
      <div class="error-banner" role="alert" aria-live="assertive">
        <mat-icon aria-hidden="true">error_outline</mat-icon>
        <span>{{ message() }}</span>
      </div>
    }
  `,
  styles: `
    .error-banner {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      background: var(--app-error-bg);
      border: 1px solid var(--app-error-border);
      border-radius: var(--app-radius-sm);
      padding: 0.625rem 0.875rem;
      color: var(--app-error);
      font-size: 0.875rem;

      mat-icon {
        font-size: 1.1rem;
        width: 1.1rem;
        height: 1.1rem;
        flex-shrink: 0;
      }
    }
  `,
})
export class ErrorBanner {
  readonly message = input<string | null>(null);
}
