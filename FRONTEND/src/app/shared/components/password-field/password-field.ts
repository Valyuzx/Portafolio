import { Component, input, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormField, MatLabel, MatSuffix, MatError } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton } from '@angular/material/button';

@Component({
  selector: 'app-password-field',
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatLabel,
    MatSuffix,
    MatError,
    MatInput,
    MatIcon,
    MatIconButton,
  ],
  template: `
    <mat-form-field appearance="outline" class="field-full">
      <mat-label>{{ label() }}</mat-label>
      <input
        matInput
        [type]="hidden() ? 'password' : 'text'"
        [formControl]="control()"
        [autocomplete]="autocomplete()"
      />
      <button
        matIconButton
        matSuffix
        type="button"
        [attr.aria-label]="hidden() ? 'Mostrar contraseña' : 'Ocultar contraseña'"
        (click)="hidden.update(v => !v)"
      >
        <mat-icon>{{ hidden() ? 'visibility_off' : 'visibility' }}</mat-icon>
      </button>
      @if (errorMessage()) {
        <mat-error>{{ errorMessage() }}</mat-error>
      }
    </mat-form-field>
  `,
  styles: `
    :host { display: block; width: 100%; }
    .field-full { width: 100%; }
  `,
})
export class PasswordField {
  readonly control = input.required<FormControl<string | null>>();
  readonly label = input('Contraseña');
  readonly autocomplete = input<string>('current-password');
  /** Texto del error a mostrar. Si está vacío, no se muestra mat-error. */
  readonly errorMessage = input<string>('');

  readonly hidden = signal(true);
}
