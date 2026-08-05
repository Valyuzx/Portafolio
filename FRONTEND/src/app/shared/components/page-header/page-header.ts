import { Component, input } from '@angular/core';
//CABECERA DE PÁGINA CON REUTILIZABLE
@Component({
  selector: 'app-page-header',
  template: `
    <header class="page-header">
      <div class="header-text">
        <h1 class="page-title">{{ title() }}</h1>
        @if (subtitle()) {
          <p class="page-subtitle">{{ subtitle() }}</p>
        }
      </div>
      <div class="header-actions">
        <ng-content />
      </div>
    </header>
  `,
  styles: `
    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      flex-wrap: wrap;
      gap: 1rem;
    }

    .header-text {
      display: flex;
      flex-direction: column;
      gap: 0.25rem;
    }

    .page-title {
      font: var(--mat-sys-headline-large);
      color: var(--app-text-primary);
      margin: 0;
    }

    .page-subtitle {
      font: var(--mat-sys-body-medium);
      color: var(--app-text-disabled);
      margin: 0;
    }

    .header-actions {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      flex-shrink: 0;
    }
  `,
})
export class PageHeader {
  readonly title = input.required<string>();
  readonly subtitle = input<string>('');
}
