import { Component, input } from '@angular/core';

@Component({
  selector: 'app-form-section',
  template: `
    <section class="form-section">
      @if (title()) {
        <h2 class="section-title">{{ title() }}</h2>
      }
      <ng-content />
    </section>
  `,
  styles: `
    .form-section {
      background: var(--app-surface-card);
      border: 1px solid var(--app-border);
      border-radius: var(--app-radius-lg);
      padding: 1.5rem;
      display: flex;
      flex-direction: column;
      gap: 1.25rem;
    }

    .section-title {
      font: var(--mat-sys-title-small);
      color: var(--app-text-muted);
      text-transform: uppercase;
      letter-spacing: 0.075em;
      margin: 0 0 0.25rem;
      padding-bottom: 0.75rem;
      border-bottom: 1px solid var(--app-border);
    }
  `,
})
export class FormSection {
  readonly title = input<string>('');
}
